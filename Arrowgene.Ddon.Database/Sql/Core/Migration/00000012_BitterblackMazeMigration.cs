using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class BitterblackMazeMigration : IMigrationStrategy
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(BitterblackMazeMigration));

        public uint From => 11;
        public uint To => 12;

        private readonly DatabaseSetting DatabaseSetting;

        public BitterblackMazeMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/bitterblack_maze_migration.sql");
            db.Execute(conn, adaptedSchema);

            var characterIds = new List<uint>();
            db.ExecuteReader(conn, "SELECT * from ddon_character;",
               command => { },
               reader =>
               {
                   while (reader.Read())
                   {
                       characterIds.Add(db.GetUInt32(reader, "character_id"));
                   }
               });

            foreach (var characterId in characterIds)
            {
                db.ExecuteNonQuery(conn, "INSERT INTO ddon_bbm_progress (character_id, start_time, content_id, content_mode, tier, killed_death, last_ticket_time) VALUES(@character_id, @start_time, @content_id, @content_mode, @tier, @killed_death, @last_ticket_time);",
                    command =>
                    {
                        db.AddParameter(command, "character_id", characterId);
                        db.AddParameter(command, "start_time", 0);
                        db.AddParameter(command, "content_id", 0);
                        db.AddParameter(command, "content_mode", 0);
                        db.AddParameter(command, "tier", 0);
                        db.AddParameter(command, "killed_death", 0);
                        db.AddParameter(command, "last_ticket_time", 0);
                    });

                db.ExecuteNonQuery(conn, "INSERT INTO ddon_bbm_rewards (character_id, gold_marks, silver_marks, red_marks) VALUES(@character_id, @gold_marks, @silver_marks, @red_marks);",
                    command =>
                    {
                        db.AddParameter(command, "character_id", characterId);
                        db.AddParameter(command, "gold_marks", 0);
                        db.AddParameter(command, "silver_marks", 0);
                        db.AddParameter(command, "red_marks", 0);
                    });
            }

            return true;
        }
    }
}
