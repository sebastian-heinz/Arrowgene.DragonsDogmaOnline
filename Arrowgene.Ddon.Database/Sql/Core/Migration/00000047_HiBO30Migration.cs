using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class HiBO30Migration(DatabaseSetting databaseSetting) : IMigrationStrategy
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(HiBO30Migration));

        public uint From => 46;
        public uint To => 47;

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            Logger.Info("Executing migration sql script ...");
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(databaseSetting, "Script/migration_hibo_3.0.sql");
            db.Execute(conn, adaptedSchema, true);

            Logger.Info("Collecting updated records ...");
            var rows = new List<(uint CharacterId, OrbTreeType OrbTreeType, JobId JobId, uint ReleaseId)>();
            db.ExecuteReader(conn, "SELECT * FROM ddon_skill_augmentation_released_elements;",
                command => { },
                reader =>
                {
                    while (reader.Read())
                    {
                        var characterId = db.GetUInt32(reader, "character_id");
                        var orbTreeType = (OrbTreeType) db.GetUInt32(reader, "orb_tree_type");
                        var jobId = (JobId) db.GetUInt32(reader, "job_id");
                        var releaseId = db.GetUInt32(reader, "release_id");
                        rows.Add((characterId, orbTreeType, jobId, releaseId));
                    }
                }
            );

            Logger.Info("Dropping existing released element tables ...");
            db.Execute(conn, "DELETE FROM ddon_skill_augmentation_released_elements;");

            Logger.Info("Inserting updated release ids ...");
            foreach (var row in rows)
            {
                uint newReleaseId = (row.ReleaseId | ((uint)row.OrbTreeType) << 24);
                db.ExecuteNonQuery(conn, "INSERT INTO ddon_skill_augmentation_released_elements (character_id, orb_tree_type, job_id, release_id) VALUES (@character_id, @orb_tree_type, @job_id, @release_id);",
                command =>
                {
                    db.AddParameter(command, "character_id", row.CharacterId);
                    db.AddParameter(command, "orb_tree_type", (uint) row.OrbTreeType);
                    db.AddParameter(command, "job_id", (uint) row.JobId);
                    db.AddParameter(command, "release_id", newReleaseId);
                });
            }

            return true;
        }
    }
}
