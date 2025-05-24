using System.Data.Common;
using System.Linq;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeletePawnCraftProgress =
        "DELETE FROM \"ddon_pawn_craft_progress\" WHERE \"craft_character_id\" = @craft_character_id AND \"craft_lead_pawn_id\" = @craft_lead_pawn_id;";

    protected static readonly string[] PawnCraftProgressFields =
    [
        "craft_character_id", "craft_lead_pawn_id", "craft_support_pawn_id1", "craft_support_pawn_id2", "craft_support_pawn_id3", "recipe_id", "exp", "npc_action_id",
        "item_id", "unk0", "remain_time", "exp_bonus", "create_count", "plus_value", "great_success", "bonus_exp", "additional_quantity"
    ];

    protected static readonly string[] PawnCraftProgressKeyFields =
    [
        "craft_character_id", "craft_lead_pawn_id"
    ];

    protected static readonly string[] PawnCraftProgressNonKeyFields = PawnCraftProgressFields.Except(PawnCraftProgressKeyFields).ToArray();

    private static readonly string SqlUpdatePawnCraftProgress =
        $"UPDATE \"ddon_pawn_craft_progress\" SET {BuildQueryUpdate(PawnCraftProgressFields)} WHERE \"craft_character_id\" = @craft_character_id AND \"craft_lead_pawn_id\" = @craft_lead_pawn_id;";

    private static readonly string SqlSelectPawnCraftProgress =
        $"SELECT {BuildQueryField(PawnCraftProgressFields)} FROM \"ddon_pawn_craft_progress\" WHERE \"craft_character_id\" = @craft_character_id AND \"craft_lead_pawn_id\" = @craft_lead_pawn_id;";

    private readonly string SqlInsertPawnCraftProgress =
        $"INSERT INTO \"ddon_pawn_craft_progress\" ({BuildQueryField(PawnCraftProgressFields)}) VALUES ({BuildQueryInsert(PawnCraftProgressFields)});";

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

    public override bool InsertPawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlInsertPawnCraftProgress, command => { AddAllParameters(command, craftProgress); }) == 1; });
    }

    public override bool UpdatePawnCraftProgress(CraftProgress craftProgress, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlUpdatePawnCraftProgress, command => { AddAllParameters(command, craftProgress); }) == 1; });
    }

    public override bool DeletePawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlDeletePawnCraftProgress, command =>
            {
                AddParameter(command, "@craft_character_id", craftCharacterId);
                AddParameter(command, "@craft_lead_pawn_id", craftLeadPawnId);
            }) == 1;
        });
    }

    public override CraftProgress? SelectPawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            CraftProgress? craftProgress = null;
            ExecuteReader(connection, SqlSelectPawnCraftProgress,
                command =>
                {
                    AddParameter(command, "@craft_character_id", craftCharacterId);
                    AddParameter(command, "@craft_lead_pawn_id", craftLeadPawnId);
                }, reader =>
                {
                    if (reader.Read()) craftProgress = ReadAllCraftProgressData(reader);
                });
            return craftProgress;
        });
    }

    private CraftProgress ReadAllCraftProgressData(DbDataReader reader)
    {
        CraftProgress craftProgress = new()
        {
            CraftCharacterId = GetUInt32(reader, "craft_character_id"),
            CraftLeadPawnId = GetUInt32(reader, "craft_lead_pawn_id"),
            CraftSupportPawnId1 = GetUInt32(reader, "craft_support_pawn_id1"),
            CraftSupportPawnId2 = GetUInt32(reader, "craft_support_pawn_id2"),
            CraftSupportPawnId3 = GetUInt32(reader, "craft_support_pawn_id3"),

            RecipeId = GetUInt32(reader, "recipe_id"),
            Exp = GetUInt32(reader, "exp"),
            NpcActionId = (NpcActionType)GetInt32(reader, "npc_action_id"),
            ItemId = GetUInt32(reader, "item_id"),
            AdditionalStatusId = GetUInt16(reader, "unk0"),
            RemainTime = GetUInt32(reader, "remain_time"),
            ExpBonus = GetBoolean(reader, "exp_bonus"),
            CreateCount = GetUInt32(reader, "create_count"),

            PlusValue = GetUInt32(reader, "plus_value"),
            GreatSuccess = GetBoolean(reader, "great_success"),
            BonusExp = GetUInt32(reader, "bonus_exp"),
            AdditionalQuantity = GetUInt32(reader, "additional_quantity")
        };

        return craftProgress;
    }

    protected void AddAllKeyParameters(DbCommand command, CraftProgress craftProgress)
    {
        AddParameter(command, "@craft_character_id", craftProgress.CraftCharacterId);
        AddParameter(command, "@craft_lead_pawn_id", craftProgress.CraftLeadPawnId);
    }

    protected void AddAllNonKeyParameters(DbCommand command, CraftProgress craftProgress)
    {
        AddParameter(command, "@craft_support_pawn_id1", craftProgress.CraftSupportPawnId1);
        AddParameter(command, "@craft_support_pawn_id2", craftProgress.CraftSupportPawnId2);
        AddParameter(command, "@craft_support_pawn_id3", craftProgress.CraftSupportPawnId3);

        AddParameter(command, "@recipe_id", craftProgress.RecipeId);
        AddParameter(command, "@exp", craftProgress.Exp);
        AddParameter(command, "@npc_action_id", (int)craftProgress.NpcActionId);
        AddParameter(command, "@item_id", craftProgress.ItemId);
        AddParameter(command, "@unk0", craftProgress.AdditionalStatusId);
        AddParameter(command, "@remain_time", craftProgress.RemainTime);
        AddParameter(command, "@exp_bonus", craftProgress.ExpBonus);
        AddParameter(command, "@create_count", craftProgress.CreateCount);

        AddParameter(command, "@plus_value", craftProgress.PlusValue);
        AddParameter(command, "@great_success", craftProgress.GreatSuccess);
        AddParameter(command, "@bonus_exp", craftProgress.BonusExp);
        AddParameter(command, "@additional_quantity", craftProgress.AdditionalQuantity);
    }

    protected void AddAllParameters(DbCommand command, CraftProgress craftProgress)
    {
        AddAllKeyParameters(command, craftProgress);
        AddAllNonKeyParameters(command, craftProgress);
    }
}
