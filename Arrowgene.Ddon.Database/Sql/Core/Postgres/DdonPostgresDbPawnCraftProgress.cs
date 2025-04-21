using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql;

public partial class DdonPostgresDb
{
    private readonly string SqlUpsertPawnCraftProgress =
        $"""
         INSERT INTO "ddon_equipment_limit_break" ({BuildQueryField(PawnCraftProgressFields)}) 
                        VALUES ({BuildQueryInsert(PawnCraftProgressFields)}) 
                        ON CONFLICT ("craft_character_id", "craft_lead_pawn_id") 
                        DO UPDATE SET {BuildQueryUpdateWithPrefix("EXCLUDED.", PawnCraftProgressNonKeyFields)};
         """;

    /// <summary>
    ///     Insert or update in one round‑trip using Postgres ON CONFLICT.
    ///     Returns true if exactly one row was inserted or updated.
    /// </summary>
    public override bool ReplacePawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlUpsertPawnCraftProgress, command => { AddAllNonKeyParameters(command, craftProgress); }) == 1; });
    }
}
