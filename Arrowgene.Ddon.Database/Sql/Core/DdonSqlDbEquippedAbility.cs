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
            "character_common_id", "equipped_to_job", "job", "slot_no", "ability_id"
        };

        private readonly string SqlInsertEquippedAbility = $"INSERT INTO \"ddon_equipped_ability\" ({BuildQueryField(EquippedAbilityFields)}) VALUES ({BuildQueryInsert(EquippedAbilityFields)});";
        private readonly string SqlReplaceEquippedAbility = $"INSERT OR REPLACE INTO \"ddon_equipped_ability\" ({BuildQueryField(EquippedAbilityFields)}) VALUES ({BuildQueryInsert(EquippedAbilityFields)});";
        private static readonly string SqlUpdateEquippedAbility = $"UPDATE \"ddon_equipped_ability\" SET {BuildQueryUpdate(EquippedAbilityFields)} WHERE \"character_common_id\"=@old_character_common_id AND \"equipped_to_job\"=@old_equipped_to_job AND \"slot_no\"=@old_slot_no;";
        private static readonly string SqlSelectEquippedAbilities = $"SELECT {BuildQueryField(EquippedAbilityFields)} FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id ORDER BY equipped_to_job, slot_no;";
        private const string SqlDeleteEquippedAbility = "DELETE FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id AND \"equipped_to_job\"=@equipped_to_job AND \"slot_no\"=@slot_no;";
        private const string SqlDeleteEquippedAbilities = "DELETE FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id AND \"equipped_to_job\"=@equipped_to_job;";

        public bool InsertEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
        {
            return ExecuteNonQuery(SqlInsertEquippedAbility, command =>
            {
                AddParameter(command, commonId, equippedToJob, slotNo, ability);
            }) == 1;
        }
        
        public bool ReplaceEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
        {
            ExecuteNonQuery(SqlReplaceEquippedAbility, command =>
            {
                AddParameter(command, commonId, equippedToJob, slotNo, ability);
            });
            return true;
        }

        public bool ReplaceEquippedAbilities(uint commonId, JobId equippedToJob, List<Ability> abilities)
        {
            return ExecuteInTransaction(connection =>
            {
                // Remove previously equipped abilities
                ExecuteNonQuery(connection, SqlDeleteEquippedAbilities, command =>
                {
                    AddParameter(command, "@character_common_id", commonId);
                    AddParameter(command, "@equipped_to_job", (byte) equippedToJob);
                });

                // Insert new ones
                for(byte i = 0; i < abilities.Count; i++)
                {
                    Ability ability = abilities[i];
                    byte slotNo = (byte)(i+1);
                    ExecuteNonQuery(connection, SqlInsertEquippedAbility, command =>
                    {
                        AddParameter(command, commonId, equippedToJob, slotNo, ability);
                    });
                }
            });
        }

        public bool UpdateEquippedAbility(uint commonId, JobId oldEquippedToJob, byte oldSlotNo, JobId equippedToJob, byte slotNo, Ability updatedability)
        {
            return ExecuteNonQuery(SqlDeleteEquippedAbility, command =>
            {
                AddParameter(command, commonId, equippedToJob, slotNo, updatedability);
                AddParameter(command, "@old_character_common_id", commonId);
                AddParameter(command, "@old_equipped_to_job", (byte) oldEquippedToJob);
                AddParameter(command, "@old_slot_no", oldSlotNo);
            }) == 1;
        }

        public bool DeleteEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo)
        {
            return ExecuteNonQuery(SqlDeleteEquippedAbility, command =>
            {
                AddParameter(command, "@character_common_id", commonId);
                AddParameter(command, "@equipped_to_job", (byte) equippedToJob);
                AddParameter(command, "@slot_no", slotNo);
            }) == 1;
        }

        public bool DeleteEquippedAbilities(uint commonId, JobId equippedToJob)
        {
            return ExecuteNonQuery(SqlDeleteEquippedAbilities, command =>
            {
                AddParameter(command, "@character_common_id", commonId);
                AddParameter(command, "@equipped_to_job", (byte) equippedToJob);
            }) == 1;
        }

        private void AddParameter(TCom command, uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "equipped_to_job", (byte) equippedToJob);
            AddParameter(command, "job", (byte) ability.Job);
            AddParameter(command, "slot_no", slotNo);
            AddParameter(command, "ability_id", ability.AbilityId);
            AddParameter(command, "ability_lv", ability.AbilityLv);
        }
    }
}
