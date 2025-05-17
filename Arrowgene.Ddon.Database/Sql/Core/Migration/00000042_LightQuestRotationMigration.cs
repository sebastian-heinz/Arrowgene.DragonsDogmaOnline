using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Database.Sql.Core.Migration
{
    public class LightQuestRotationMigration : IMigrationStrategy
    {
        public uint From => 41;
        public uint To => 42;

        private readonly DatabaseSetting DatabaseSetting;

        public LightQuestRotationMigration(DatabaseSetting databaseSetting)
        {
            DatabaseSetting = databaseSetting;
        }

        public bool Migrate(IDatabase db, DbConnection conn)
        {
            string adaptedSchema = DdonDatabaseBuilder.GetAdaptedSchema(DatabaseSetting, "Script/migration_light_quest_refactor.sql");
            db.Execute(conn, adaptedSchema, true);

            List<QuestProgress> values = [];

            db.ExecuteReader(conn, "SELECT * FROM ddon_quest_progress;",
                command => {}, 
                reader =>
                {
                    while (reader.Read())
                    {
                        QuestProgress obj = new();
                        obj.CharacterCommonId = db.GetUInt32(reader, "character_common_id");
                        obj.QuestScheduleId = db.GetUInt32(reader, "quest_schedule_id");
                        obj.QuestType = (QuestType)db.GetUInt32(reader, "quest_type");
                        obj.Step = db.GetUInt32(reader, "step");
                        values.Add(obj);
                    }
                }
            );

            foreach(var progress in values)
            {
                uint questId = progress.QuestScheduleId;
                byte type = 0;
                byte group = (byte)(questId / 10000 % 100); // decimal digits 5 & 6
                byte subgroup = (byte)(questId / 100 % 100); //decimal digits 3 & 4
                byte index = (byte)(questId % 100); // rightmost 2 decimal digits
                uint newId = QuestScheduleId.GenerateScheduleId(type, group, subgroup, index, 0);
                db.ExecuteNonQuery(conn,
                    "UPDATE \"ddon_quest_progress\" SET \"quest_schedule_id\"=@new_schedule_id WHERE \"character_common_id\"=@character_common_id AND \"quest_schedule_id\"=@quest_schedule_id;",
                    command =>
                {
                    db.AddParameter(command, "character_common_id", progress.CharacterCommonId);
                    db.AddParameter(command, "quest_schedule_id", progress.QuestScheduleId);
                    db.AddParameter(command, "new_schedule_id", newId);
                });
            }


            return true;
        }

    }
}
