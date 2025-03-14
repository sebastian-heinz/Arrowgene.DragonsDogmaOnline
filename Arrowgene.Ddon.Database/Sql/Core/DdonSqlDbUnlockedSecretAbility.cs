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
        protected static readonly string[] UnlockedSecretAbilityFields = new string[]
        {
            "character_common_id", "ability_id"
        };

        private readonly string SqlInsertUnlockedSecretAbility = $"INSERT INTO \"ddon_unlocked_secret_ability\" ({BuildQueryField(UnlockedSecretAbilityFields)}) VALUES ({BuildQueryInsert(UnlockedSecretAbilityFields)});";
        private readonly string SqlInsertIfNotExistsUnlockedSecretAbility = $"INSERT INTO \"ddon_unlocked_secret_ability\" ({BuildQueryField(UnlockedSecretAbilityFields)}) SELECT " +
                                                                         $"{BuildQueryInsert(UnlockedSecretAbilityFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_unlocked_secret_ability\" WHERE " +
                                                                         $"\"character_common_id\" = @character_common_id AND \"ability_id\" = @ability_id);";
        private readonly string SqlSelectAllUnlockedSecretAbility = $"SELECT {BuildQueryField(UnlockedSecretAbilityFields)} FROM \"ddon_unlocked_secret_ability\" WHERE \"character_common_id\" = @character_common_id;";


        public bool InsertSecretAbilityUnlock(uint commonId, SecretAbility secretAbility, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe<bool>(connectionIn, (connection) =>
            {
                return ExecuteNonQuery(connection, SqlInsertIfNotExistsUnlockedSecretAbility, command =>
                {
                    AddParameter(command, "character_common_id", commonId);
                    AddParameter(command, "ability_id", (uint)secretAbility);
                }) == 1;
            });
        }

        public List<SecretAbility> SelectAllUnlockedSecretAbilities(uint commonId)
        {
            using TCon connection = OpenNewConnection();
            return SelectAllUnlockedSecretAbilities(connection, commonId);
        }

        public List<SecretAbility> SelectAllUnlockedSecretAbilities(TCon conn, uint commonId)
        {
            List<SecretAbility> Results = new List<SecretAbility>();

            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectAllUnlockedSecretAbility,
                    command => {
                        AddParameter(command, "@character_common_id", commonId);
                    }, reader => {
                        while (reader.Read())
                        {
                            Results.Add(ReadUnlockedSecretAbility(reader));
                        }
                    });
            });

            return Results;
        }

        private void AddParameter(TCom command, uint commonId, SecretAbility secretAbility)
        {
            AddParameter(command, "character_common_id", commonId);
            AddParameter(command, "ability_id", (uint) secretAbility);
        }

        private SecretAbility ReadUnlockedSecretAbility(TReader reader)
        {
            return (SecretAbility) GetUInt32(reader, "ability_id");
        }
    }
}
