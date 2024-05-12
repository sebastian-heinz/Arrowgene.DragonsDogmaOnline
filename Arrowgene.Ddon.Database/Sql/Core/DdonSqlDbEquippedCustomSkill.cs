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
        private readonly string SqlInsertIfNotExistsEquippedCustomSkill = $"INSERT INTO \"ddon_equipped_custom_skill\" ({BuildQueryField(EquippedCustomSkillFields)}) SELECT {BuildQueryInsert(EquippedCustomSkillFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_equipped_custom_skill\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"slot_no\"=@slot_no);";
        private static readonly string SqlUpdateEquippedCustomSkill = $"UPDATE \"ddon_equipped_custom_skill\" SET {BuildQueryUpdate(EquippedCustomSkillFields)} WHERE \"character_common_id\"=@old_character_common_id AND \"job\"=@old_job AND \"slot_no\"=@old_slot_no;";
        private static readonly string SqlSelectEquippedCustomSkills = $"SELECT {BuildQueryField(EquippedCustomSkillFields)} FROM \"ddon_equipped_custom_skill\" WHERE \"character_common_id\"=@character_common_id;";
        private const string SqlDeleteEquippedCustomSkill = "DELETE FROM \"ddon_equipped_custom_skill\" WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"slot_no\"=@slot_no;";

        public bool InsertIfNotExistsEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsEquippedCustomSkill(connection, commonId, slotNo, skill);
        }
        
        public bool InsertIfNotExistsEquippedCustomSkill(TCon connection, uint commonId, byte slotNo, CustomSkill skill)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsEquippedCustomSkill, command =>
            {
                AddParameter(command, commonId, slotNo, skill);
            }) == 1;
        }
        
        public bool InsertEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill)
        {
            using TCon connection = OpenNewConnection();
            return InsertEquippedCustomSkill(connection, commonId, slotNo, skill);
        }
        
        public bool InsertEquippedCustomSkill(TCon connection, uint commonId, byte slotNo, CustomSkill skill)
        {
            return ExecuteNonQuery(connection, SqlInsertEquippedCustomSkill, command =>
            {
                AddParameter(command, commonId, slotNo, skill);
            }) == 1;
        }
        
        public bool ReplaceEquippedCustomSkill(uint commonId, byte slotNo, CustomSkill skill)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceEquippedCustomSkill(connection, commonId, slotNo, skill);
        }
        
        public bool ReplaceEquippedCustomSkill(TCon connection, uint commonId, byte slotNo, CustomSkill skill)
        {
            Logger.Debug("Inserting equipped custom skill.");
            if (!InsertIfNotExistsEquippedCustomSkill(connection, commonId, slotNo, skill))
            {
                Logger.Debug("Equipped custom skill already exists, replacing.");
                return UpdateEquippedCustomSkill(connection, commonId, skill.Job, slotNo, slotNo, skill);
            }
            return true;
        }

        public bool UpdateEquippedCustomSkill(uint commonId, JobId oldJob, byte oldSlotNo, byte slotNo, CustomSkill updatedSkill)
        {
            using TCon connection = OpenNewConnection();
            return UpdateEquippedCustomSkill(connection, commonId, oldJob, oldSlotNo, slotNo, updatedSkill);
        }

        public bool UpdateEquippedCustomSkill(TCon connection, uint commonId, JobId oldJob, byte oldSlotNo, byte slotNo, CustomSkill updatedSkill)
        {
            return ExecuteNonQuery(connection, SqlUpdateEquippedCustomSkill, command =>
            {
                AddParameter(command, commonId, slotNo, updatedSkill);
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
