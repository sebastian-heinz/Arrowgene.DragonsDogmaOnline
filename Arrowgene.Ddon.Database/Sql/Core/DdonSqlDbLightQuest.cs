using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public partial class DdonSqlDb : SqlDb
    {
        private static readonly string[] DdonLightQuestFields =
        [
            "variant_id",
            "quest_schedule_id", "quest_id", "target", "level", "count", 
            "reward_xp", "reward_g", "reward_r", "reward_ap", 
            "distribution_end"
        ];

        private readonly string SqlInsertLightQuestRecord = 
            $"INSERT INTO \"ddon_light_quests\" ({BuildQueryField(DdonLightQuestFields)}) VALUES ({BuildQueryInsert(DdonLightQuestFields)});";
        private readonly string SqlSelectLightQuestRecords = 
            $"SELECT {BuildQueryField(DdonLightQuestFields)} FROM \"ddon_light_quests\";";
        private readonly string SqlDeleteLightQuestRecord =
            $"DELETE FROM \"ddon_light_quests\" WHERE \"quest_schedule_id\"=@quest_schedule_id;";

        private void AddParameter(DbCommand command, LightQuestRecord record)
        {
            AddParameter(command, "variant_id", record.VariantIndex);
            AddParameter(command, "quest_schedule_id", record.QuestScheduleId);
            AddParameter(command, "quest_id", (uint)record.QuestId);
            AddParameter(command, "target", record.Target);
            AddParameter(command, "level", record.Level);
            AddParameter(command, "count", record.Count);
            AddParameter(command, "reward_xp", record.RewardXP);
            AddParameter(command, "reward_g", record.RewardG);
            AddParameter(command, "reward_r", record.RewardR);
            AddParameter(command, "reward_ap", record.RewardAP);
            AddParameter(command, "distribution_end", record.DistributionEnd.UtcDateTime);
        }

        private LightQuestRecord ReadLightQuestRecord(DbDataReader reader)
        {
            LightQuestRecord record = new();
            record.VariantIndex = GetUInt32(reader, "variant_id");
            record.QuestId = (QuestId)GetUInt32(reader, "quest_id");
            record.Target = GetInt32(reader, "target");
            record.Level = GetUInt16(reader, "level");
            record.Count = GetInt32(reader, "count");
            record.RewardXP = GetUInt32(reader, "reward_xp");
            record.RewardG = GetUInt32(reader, "reward_g");
            record.RewardR = GetUInt32(reader, "reward_r");
            record.RewardAP = GetUInt32(reader, "reward_ap");
            record.DistributionEnd = GetDateTime(reader, "distribution_end");

            return record;
        }

        public override bool InsertLightQuestRecord(LightQuestRecord record, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlInsertLightQuestRecord, command =>
                {
                    AddParameter(command, record);
                }) == 1;
            });
        }

        public override List<LightQuestRecord> SelectLightQuestRecords(DbConnection? connectionIn = null)
        {
            List<LightQuestRecord> results = new();
            ExecuteQuerySafe(connectionIn, connection =>
            {
                ExecuteReader(connection, SqlSelectLightQuestRecords, command =>
                {}, 
                reader =>
                {
                    while (reader.Read())
                    {
                        results.Add(ReadLightQuestRecord(reader));
                    }
                });
            });
            return results;
        }

        public override bool DeleteLightQuestRecord(uint scheduleId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(connection, SqlDeleteLightQuestRecord, command =>
                {
                    AddParameter(command, "@quest_schedule_id", scheduleId);
                }) == 1;
            });
        }


    }
}
