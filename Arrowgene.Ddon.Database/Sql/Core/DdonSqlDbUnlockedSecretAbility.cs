using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] UnlockedSecretAbilityFields = new[]
    {
        "character_common_id", "ability_id"
    };

    private readonly string SqlInsertIfNotExistsUnlockedSecretAbility =
        $"INSERT INTO \"ddon_unlocked_secret_ability\" ({BuildQueryField(UnlockedSecretAbilityFields)}) SELECT " +
        $"{BuildQueryInsert(UnlockedSecretAbilityFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_unlocked_secret_ability\" WHERE " +
        $"\"character_common_id\" = @character_common_id AND \"ability_id\" = @ability_id);";

    private readonly string SqlInsertUnlockedSecretAbility =
        $"INSERT INTO \"ddon_unlocked_secret_ability\" ({BuildQueryField(UnlockedSecretAbilityFields)}) VALUES ({BuildQueryInsert(UnlockedSecretAbilityFields)});";

    private readonly string SqlSelectAllUnlockedSecretAbility =
        $"SELECT {BuildQueryField(UnlockedSecretAbilityFields)} FROM \"ddon_unlocked_secret_ability\" WHERE \"character_common_id\" = @character_common_id;";


    public override bool InsertSecretAbilityUnlock(uint commonId, AbilityId secretAbility, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsUnlockedSecretAbility, command =>
            {
                AddParameter(command, "character_common_id", commonId);
                AddParameter(command, "ability_id", (uint)secretAbility);
            }) == 1;
        });
    }

    public override List<AbilityId> SelectAllUnlockedSecretAbilities(uint commonId)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectAllUnlockedSecretAbilities(connection, commonId);
    }

    public List<AbilityId> SelectAllUnlockedSecretAbilities(DbConnection conn, uint commonId)
    {
        List<AbilityId> Results = new();

        ExecuteInTransaction(conn =>
        {
            ExecuteReader(conn, SqlSelectAllUnlockedSecretAbility,
                command => { AddParameter(command, "@character_common_id", commonId); }, reader =>
                {
                    while (reader.Read()) Results.Add(ReadUnlockedSecretAbility(reader));
                });
        });

        return Results;
    }

    private void AddParameter(DbCommand command, uint commonId, AbilityId secretAbility)
    {
        AddParameter(command, "character_common_id", commonId);
        AddParameter(command, "ability_id", (uint)secretAbility);
    }

    private AbilityId ReadUnlockedSecretAbility(DbDataReader reader)
    {
        return (AbilityId)GetUInt32(reader, "ability_id");
    }
}
