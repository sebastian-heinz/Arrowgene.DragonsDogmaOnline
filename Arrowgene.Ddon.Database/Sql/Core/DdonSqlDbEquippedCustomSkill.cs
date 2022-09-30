using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] EquippedCustomSkillFields = new string[]
        {
            "character_id", "job", "slot_no", "skill_id", "skill_lv"
        };

        private readonly string SqlInsertEquippedCustomSkill = $"INSERT INTO `ddon_equipped_custom_skill` ({BuildQueryField(EquippedCustomSkillFields)}) VALUES ({BuildQueryInsert(EquippedCustomSkillFields)});";
        private readonly string SqlReplaceEquippedCustomSkill = $"INSERT OR REPLACE INTO `ddon_equipped_custom_skill` ({BuildQueryField(EquippedCustomSkillFields)}) VALUES ({BuildQueryInsert(EquippedCustomSkillFields)});";
        private static readonly string SqlUpdateEquippedCustomSkill = $"UPDATE `ddon_equipped_custom_skill` SET {BuildQueryUpdate(EquippedCustomSkillFields)} WHERE `character_id`=@old_character_id AND `job`=@old_job AND `slot_no`=@old_slot_no;";
        private static readonly string SqlSelectEquippedCustomSkills = $"SELECT {BuildQueryField(EquippedCustomSkillFields)} FROM `ddon_equipped_custom_skill` WHERE `character_id`=@character_id;";
        private const string SqlDeleteEquippedCustomSkill = "DELETE FROM `ddon_equipped_custom_skill` WHERE `character_id`=@character_id AND `job`=@job AND `slot_no`=@slot_no;";

        public bool InsertEquippedCustomSkill(uint characterId, CustomSkill skill)
        {
            return ExecuteNonQuery(SqlInsertEquippedCustomSkill, command =>
            {
                AddParameter(command, characterId, skill);
            }) == 1;
        }
        
        public bool ReplaceEquippedCustomSkill(uint characterId, CustomSkill skill)
        {
            ExecuteNonQuery(SqlReplaceEquippedCustomSkill, command =>
            {
                AddParameter(command, characterId, skill);
            });
            return true;
        }

        public bool UpdateEquippedCustomSkill(uint characterId, JobId oldJob, byte oldSlotNo, CustomSkill updatedSkill)
        {
            return ExecuteNonQuery(SqlDeleteEquippedCustomSkill, command =>
            {
                AddParameter(command, characterId, updatedSkill);
                AddParameter(command, "@old_character_id", characterId);
                AddParameter(command, "@old_job", (byte) oldJob);
                AddParameter(command, "@old_slot_no", oldSlotNo);
            }) == 1;
        }

        public bool DeleteEquippedCustomSkill(uint characterId, JobId job, byte slotNo)
        {
            return ExecuteNonQuery(SqlDeleteEquippedCustomSkill, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@job", (byte) job);
                AddParameter(command, "@slot_no", slotNo);
            }) == 1;
        }

        private CustomSkill ReadCustomSkill(DbDataReader reader)
        {
            CustomSkill skill = new CustomSkill();
            skill.Job = (JobId) GetByte(reader, "job");
            skill.SlotNo = GetByte(reader, "slot_no");
            skill.SkillId = GetUInt32(reader, "skill_id");
            skill.SkillLv = GetByte(reader, "skill_lv");
            return skill;
        }

        private void AddParameter(TCom command, uint characterId, CustomSkill skill)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "job", (byte) skill.Job);
            AddParameter(command, "slot_no", skill.SlotNo);
            AddParameter(command, "skill_id", skill.SkillId);
            AddParameter(command, "skill_lv", skill.SkillLv);
        }
    }
}