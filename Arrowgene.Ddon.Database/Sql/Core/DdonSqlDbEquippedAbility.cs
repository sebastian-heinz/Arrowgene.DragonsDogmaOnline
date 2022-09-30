using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] EquippedAbilityFields = new string[]
        {
            "character_id", "equipped_to_job", "job", "slot_no", "ability_id", "ability_lv"
        };

        private readonly string SqlInsertEquippedAbility = $"INSERT INTO `ddon_equipped_ability` ({BuildQueryField(EquippedAbilityFields)}) VALUES ({BuildQueryInsert(EquippedAbilityFields)});";
        private readonly string SqlReplaceEquippedAbility = $"INSERT OR REPLACE INTO `ddon_equipped_ability` ({BuildQueryField(EquippedAbilityFields)}) VALUES ({BuildQueryInsert(EquippedAbilityFields)});";
        private static readonly string SqlUpdateEquippedAbility = $"UPDATE `ddon_equipped_ability` SET {BuildQueryUpdate(EquippedAbilityFields)} WHERE `character_id`=@old_character_id AND `equipped_to_job`=@old_equipped_to_job AND `slot_no`=@old_slot_no;";
        private static readonly string SqlSelectEquippedAbilities = $"SELECT {BuildQueryField(EquippedAbilityFields)} FROM `ddon_equipped_ability` WHERE `character_id`=@character_id ORDER BY equipped_to_job, slot_no;";
        private const string SqlDeleteEquippedAbility = "DELETE FROM `ddon_equipped_ability` WHERE `character_id`=@character_id AND `equipped_to_job`=@equipped_to_job AND `slot_no`=@slot_no;";
        private const string SqlDeleteEquippedAbilities = "DELETE FROM `ddon_equipped_ability` WHERE `character_id`=@character_id AND `equipped_to_job`=@equipped_to_job;";

        public bool InsertEquippedAbility(uint characterId, Ability ability)
        {
            return ExecuteNonQuery(SqlInsertEquippedAbility, command =>
            {
                AddParameter(command, characterId, ability);
            }) == 1;
        }
        
        public bool ReplaceEquippedAbility(uint characterId, Ability ability)
        {
            ExecuteNonQuery(SqlReplaceEquippedAbility, command =>
            {
                AddParameter(command, characterId, ability);
            });
            return true;
        }

        public bool ReplaceEquippedAbilities(uint characterId, JobId equippedToJob, List<Ability> abilities)
        {
            return ExecuteInTransaction(connection =>
            {
                // Remove previously equipped abilities
                ExecuteNonQuery(connection, SqlDeleteEquippedAbilities, command =>
                {
                    AddParameter(command, "@character_id", characterId);
                    AddParameter(command, "@equipped_to_job", (byte) equippedToJob);
                });

                // Insert new ones
                foreach(Ability ability in abilities)
                {
                    ExecuteNonQuery(connection, SqlInsertEquippedAbility, command =>
                    {
                        AddParameter(command, characterId, ability);
                    });
                }
            });
        }

        public bool UpdateEquippedAbility(uint characterId, JobId oldEquippedToJob, byte oldSlotNo, Ability updatedability)
        {
            return ExecuteNonQuery(SqlDeleteEquippedAbility, command =>
            {
                AddParameter(command, characterId, updatedability);
                AddParameter(command, "@old_character_id", characterId);
                AddParameter(command, "@old_equipped_to_job", (byte) oldEquippedToJob);
                AddParameter(command, "@old_slot_no", oldSlotNo);
            }) == 1;
        }

        public bool DeleteEquippedAbility(uint characterId, JobId equippedToJob, byte slotNo)
        {
            return ExecuteNonQuery(SqlDeleteEquippedAbility, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@equipped_to_job", (byte) equippedToJob);
                AddParameter(command, "@slot_no", slotNo);
            }) == 1;
        }

        public bool DeleteEquippedAbilities(uint characterId, JobId equippedToJob)
        {
            return ExecuteNonQuery(SqlDeleteEquippedAbilities, command =>
            {
                AddParameter(command, "@character_id", characterId);
                AddParameter(command, "@equipped_to_job", (byte) equippedToJob);
            }) == 1;
        }

        private Ability ReadAbility(DbDataReader reader)
        {
            Ability ability = new Ability();
            ability.EquippedToJob = (JobId) GetByte(reader, "equipped_to_job");
            ability.Job = (JobId) GetByte(reader, "job");
            ability.SlotNo = GetByte(reader, "slot_no");
            ability.AbilityId = GetUInt32(reader, "ability_id");
            ability.AbilityLv = GetByte(reader, "ability_lv");
            return ability;
        }

        private void AddParameter(TCom command, uint characterId, Ability ability)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "equipped_to_job", (byte) ability.EquippedToJob);
            AddParameter(command, "job", (byte) ability.Job);
            AddParameter(command, "slot_no", ability.SlotNo);
            AddParameter(command, "ability_id", ability.AbilityId);
            AddParameter(command, "ability_lv", ability.AbilityLv);
        }
    }
}