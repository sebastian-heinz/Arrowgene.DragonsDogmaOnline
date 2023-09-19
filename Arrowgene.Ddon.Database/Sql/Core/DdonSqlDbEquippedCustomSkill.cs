using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] EquippedCustomSkillFields = new string[]
        {
            "character_common_id", "job", "slot_no", "skill_id"
        };

        private readonly string SqlInsertEquippedCustomSkill = $"INSERT INTO \"ddon_equipped_custom_skill\" ({BuildQueryField(EquippedCustomSkillFields)}) VALUES ({BuildQueryInsert(EquippedCustomSkillFields)});";

        protected virtual string SqlReplaceEquippedCustomSkill { get; } =
            $"INSERT OR REPLACE INTO \"ddon_equipped_custom_skill\" ({BuildQueryField(EquippedCustomSkillFields)}) VALUES ({BuildQueryInsert(EquippedCustomSkillFields)});";

        private static readonly string SqlUpdateEquippedCustomSkill = $"UPDATE \"ddon_equipped_custom_skill\" SET {BuildQueryUpdate(EquippedCustomSkillFields)} WHERE \"character_common_id\"=@old_character_common_id AND \"job\"=@old_job AND \"slot_no\"=@old_slot_no;";
        private static readonly string SqlSelectEquippedCustomSkills = $"SELECT {BuildQueryField(EquippedCustomSkillFields)} FROM \"ddon_equipped_custom_skill\" WHERE \"character_common_id\"=@character_common_id;";
        private const string SqlDeleteEquippedCustomSkill = "DELETE FROM \"ddon_equipped_custom_skill\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"slot_no\"=@slot_no;";

        public bool InsertEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill)
        {
            return ExecuteNonQuery(SqlInsertEquippedCustomSkill, command =>
            {
                AddParameter(command, commonId, slotNo, skill);
            }) == 1;
        }
        
        public bool ReplaceEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill)
        {
            ExecuteNonQuery(SqlReplaceEquippedCustomSkill, command =>
            {
                AddParameter(command, commonId, slotNo, skill);
            });
            return true;
        }

        public bool UpdateEquippedCustomSkill(uint commonId, JobId oldJob, byte oldSlotNo, byte slotNo, CustomSkill updatedSkill)
        {
            return ExecuteNonQuery(SqlDeleteEquippedCustomSkill, command =>
            {
                AddParameter(command, commonId, updatedSkill);
                AddParameter(command, "@old_character_common_id", commonId);
                AddParameter(command, "@old_job", (byte) oldJob);
                AddParameter(command, "@old_slot_no", oldSlotNo);
            }) == 1;
        }

        public bool DeleteEquippedCustomSkill(uint commonId, JobId job, byte slotNo)
        {
            return ExecuteNonQuery(SqlDeleteEquippedCustomSkill, command =>
            {
                AddParameter(command, "@character_common_id", commonId);
                AddParameter(command, "@job", (byte) job);
                AddParameter(command, "@slot_no", slotNo);
            }) == 1;
        }

        private void AddParameter(TCom command, uint commonId, byte slotNo, CustomSkill skill)
        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "job", (byte) skill.Job);
            AddParameter(command, "slot_no", slotNo);
            AddParameter(command, "skill_id", skill.SkillId);
        }
    }
}
