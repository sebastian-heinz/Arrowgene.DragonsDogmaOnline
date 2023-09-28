using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] EquippedAbilityFields = new string[]
        {
            "character_common_id", "equipped_to_job", "job", "slot_no", "ability_id"
        };

        private readonly string SqlInsertEquippedAbility = $"INSERT INTO \"ddon_equipped_ability\" ({BuildQueryField(EquippedAbilityFields)}) VALUES ({BuildQueryInsert(EquippedAbilityFields)});";
        private readonly string SqlInsertIfNotExistsEquippedAbility = $"INSERT INTO \"ddon_equipped_ability\" ({BuildQueryField(EquippedAbilityFields)}) SELECT {BuildQueryInsert(EquippedAbilityFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id AND \"equipped_to_job\"=@equipped_to_job AND \"slot_no\"=@slot_no);";
        private static readonly string SqlUpdateEquippedAbility = $"UPDATE \"ddon_equipped_ability\" SET {BuildQueryUpdate(EquippedAbilityFields)} WHERE \"character_common_id\"=@old_character_common_id AND \"equipped_to_job\"=@old_equipped_to_job AND \"slot_no\"=@old_slot_no;";
        private static readonly string SqlSelectEquippedAbilities = $"SELECT {BuildQueryField(EquippedAbilityFields)} FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id ORDER BY equipped_to_job, slot_no;";
        private const string SqlDeleteEquippedAbility = "DELETE FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id AND \"equipped_to_job\"=@equipped_to_job AND \"slot_no\"=@slot_no;";
        private const string SqlDeleteEquippedAbilities = "DELETE FROM \"ddon_equipped_ability\" WHERE \"character_common_id\"=@character_common_id AND \"equipped_to_job\"=@equipped_to_job;";

        public bool InsertIfNotExistsEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsEquippedAbility(connection, commonId, equippedToJob, slotNo, ability);
        }
        
        public bool InsertIfNotExistsEquippedAbility(TCon connection, uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsEquippedAbility, command =>
            {
                AddParameter(command, commonId, equippedToJob, slotNo, ability);
            }) == 1;
        }
        
        public bool InsertEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
        {
            using TCon connection = OpenNewConnection();
            return InsertEquippedAbility(connection, commonId, equippedToJob, slotNo, ability);
        }
        
        public bool InsertEquippedAbility(TCon connection, uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
        {
            return ExecuteNonQuery(connection, SqlInsertEquippedAbility, command =>
            {
                AddParameter(command, commonId, equippedToJob, slotNo, ability);
            }) == 1;
        }
        
        public bool ReplaceEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceEquippedAbility(connection, commonId, equippedToJob, slotNo, ability);
        }        
        
        public bool ReplaceEquippedAbility(TCon connection, uint commonId, JobId equippedToJob, byte slotNo, Ability ability)
        {
            Logger.Debug("Inserting equipped ability.");
            if (!InsertIfNotExistsEquippedAbility(connection, commonId, equippedToJob, slotNo, ability))
            {
                Logger.Debug("Equipped ability already exists, replacing.");
                return UpdateEquippedAbility(connection, commonId, equippedToJob, slotNo, equippedToJob, slotNo, ability);
            }
            return true;
        }
        
        public bool ReplaceEquippedAbilities(uint commonId, JobId equippedToJob, List<Ability> abilities)
        {
            return ExecuteInTransaction(connection =>
            {
                // Remove previously equipped abilities
                DeleteEquippedAbilities(connection, commonId, equippedToJob);
                // Insert new ones
                for(byte i = 0; i < abilities.Count; i++)
                {
                    Ability ability = abilities[i];
                    byte slotNo = (byte)(i+1);
                    InsertEquippedAbility(connection, commonId, equippedToJob, slotNo, ability);
                }
            });
        }

        public bool UpdateEquippedAbility(uint commonId, JobId oldEquippedToJob, byte oldSlotNo, JobId equippedToJob, byte slotNo, Ability updatedAbility)
        {
            using TCon connection = OpenNewConnection();
            return UpdateEquippedAbility(connection, commonId, oldEquippedToJob, oldSlotNo, equippedToJob, slotNo, updatedAbility);
        }
        
        public bool UpdateEquippedAbility(TCon connection, uint commonId, JobId oldEquippedToJob, byte oldSlotNo, JobId equippedToJob, byte slotNo, Ability updatedAbility)
        {
            return ExecuteNonQuery(connection, SqlUpdateEquippedAbility, command =>
            {
                AddParameter(command, commonId, equippedToJob, slotNo, updatedAbility);
                AddParameter(command, "@old_character_common_id", commonId);
                AddParameter(command, "@old_equipped_to_job", (byte) oldEquippedToJob);
                AddParameter(command, "@old_slot_no", oldSlotNo);
            }) == 1;
        }

        public bool DeleteEquippedAbility(uint commonId, JobId equippedToJob, byte slotNo)
        {
            using TCon connection = OpenNewConnection();
            return DeleteEquippedAbility(connection, commonId, equippedToJob, slotNo);
        }
        
        public bool DeleteEquippedAbility(TCon connection, uint commonId, JobId equippedToJob, byte slotNo)
        {
            return ExecuteNonQuery(connection, SqlDeleteEquippedAbility, command =>
            {
                AddParameter(command, "@character_common_id", commonId);
                AddParameter(command, "@equipped_to_job", (byte) equippedToJob);
                AddParameter(command, "@slot_no", slotNo);
            }) == 1;
        }

        public bool DeleteEquippedAbilities(uint commonId, JobId equippedToJob)
        {
            using TCon connection = OpenNewConnection();
            return DeleteEquippedAbilities(connection, commonId, equippedToJob);
        }
        
        public bool DeleteEquippedAbilities(TCon connection, uint commonId, JobId equippedToJob)
        {
            return ExecuteNonQuery(connection, SqlDeleteEquippedAbilities, command =>
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
