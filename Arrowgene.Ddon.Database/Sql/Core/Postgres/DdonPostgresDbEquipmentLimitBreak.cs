using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Database.Sql;

public partial class DdonPostgresDb
{
    private readonly string SqlUpsertEquipmentLimitBreakRecord =
        $"""
         INSERT INTO "ddon_equipment_limit_break" ({BuildQueryField(EquipmentLimitBreakFields)}) 
                        VALUES ({BuildQueryInsert(EquipmentLimitBreakFields)}) 
                        ON CONFLICT ("character_id","item_uid") 
                        DO UPDATE SET {BuildQueryUpdateWithPrefix("EXCLUDED.", EquipmentLimitBreakNonKeyFields)};
         """;

    /// <summary>
    ///     Insert or update in one round‑trip using Postgres ON CONFLICT.
    ///     Returns true if exactly one row was inserted or updated.
    /// </summary>
    public override bool UpsertEquipmentLimitBreakRecord(uint characterId, string itemUID, CDataAddStatusParam statusParam, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpsertEquipmentLimitBreakRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "item_uid", itemUID);
                AddParameter(command, "effect_1", statusParam.AdditionalStatus1);
                AddParameter(command, "effect_2", statusParam.AdditionalStatus2);
                AddParameter(command, "is_effect1_valid", statusParam.IsAddStat1);
                AddParameter(command, "is_effect2_valid", statusParam.IsAddStat2);
            }) == 1;
        });
    }
}
