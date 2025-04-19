using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private static readonly string[] LearnedAbilityFields = new[]
    {
        "character_common_id", "job", "ability_id", "ability_lv"
    };

    private static readonly string SqlSelectLearnedAbilities =
        $"SELECT {BuildQueryField(LearnedAbilityFields)} FROM \"ddon_learned_ability\" WHERE \"character_common_id\"=@character_common_id;";

    private readonly string SqlInsertLearnedAbility =
        $"INSERT INTO \"ddon_learned_ability\" ({BuildQueryField(LearnedAbilityFields)}) VALUES ({BuildQueryInsert(LearnedAbilityFields)});";

    private readonly string SqlUpdateLearnedAbility =
        $"UPDATE \"ddon_learned_ability\" SET {BuildQueryUpdate(LearnedAbilityFields)} WHERE \"character_common_id\"=@character_common_id AND \"job\"=@job AND \"ability_id\"=@ability_id;";

    public override bool InsertLearnedAbility(uint commonId, Ability ability, DbConnection? connectionIn = null)
    {
        ExecuteQuerySafe(connectionIn, connection => { ExecuteNonQuery(connection, SqlInsertLearnedAbility, command => { AddParameter(command, commonId, ability); }); });

        return true;
    }

    public override bool UpdateLearnedAbility(uint commonId, Ability ability, DbConnection? connectionIn = null)
    {
        ExecuteQuerySafe(connectionIn, connection => { ExecuteNonQuery(connection, SqlUpdateLearnedAbility, command => { AddParameter(command, commonId, ability); }); });
        return true;
    }

    private Ability ReadLearnedAbility(DbDataReader reader)
    {
        Ability ability = new();
        ability.Job = (JobId)GetByte(reader, "job");
        ability.AbilityId = GetUInt32(reader, "ability_id");
        ability.AbilityLv = GetByte(reader, "ability_lv");
        return ability;
    }

    private void AddParameter(DbCommand command, uint commonId, Ability ability)
    {
        AddParameter(command, "character_common_id", commonId);
        AddParameter(command, "job", (byte)ability.Job);
        AddParameter(command, "ability_id", ability.AbilityId);
        AddParameter(command, "ability_lv", ability.AbilityLv);
    }
}
