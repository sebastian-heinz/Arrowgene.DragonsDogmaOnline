using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] PawnCraftProgressFields =
        {
            "craft_character_id", "craft_lead_pawn_id", "craft_support_pawn_id1", "craft_support_pawn_id2", "craft_support_pawn_id3", "recipe_id", "exp", "npc_action_id",
            "item_id", "unk0", "remain_time", "exp_bonus", "create_count", "plus_value", "great_success", "bonus_exp", "additional_quantity"
        };

        private readonly string SqlInsertPawnCraftProgress =
            $"INSERT INTO \"ddon_pawn_craft_progress\" ({BuildQueryField(PawnCraftProgressFields)}) VALUES ({BuildQueryInsert(PawnCraftProgressFields)});";

        protected virtual string SqlInsertIfNotExistsPawnCraftProgress { get; } =
            $"INSERT INTO \"ddon_pawn_craft_progress\" ({BuildQueryField(PawnCraftProgressFields)}) SELECT {BuildQueryInsert(PawnCraftProgressFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_pawn_craft_progress\" WHERE \"craft_character_id\" = @craft_character_id AND \"craft_lead_pawn_id\" = @craft_lead_pawn_id);";

        private static readonly string SqlUpdatePawnCraftProgress =
            $"UPDATE \"ddon_pawn_craft_progress\" SET {BuildQueryUpdate(PawnCraftProgressFields)} WHERE \"craft_character_id\" = @craft_character_id AND \"craft_lead_pawn_id\" = @craft_lead_pawn_id;";

        private static readonly string SqlSelectPawnCraftProgress =
            $"SELECT {BuildQueryField(PawnCraftProgressFields)} FROM \"ddon_pawn_craft_progress\" WHERE \"craft_character_id\" = @craft_character_id AND \"craft_lead_pawn_id\" = @craft_lead_pawn_id;";

        private const string SqlDeletePawnCraftProgress =
            "DELETE FROM \"ddon_pawn_craft_progress\" WHERE \"craft_character_id\" = @craft_character_id AND \"craft_lead_pawn_id\" = @craft_lead_pawn_id;";

        public bool ReplacePawnCraftProgress(CraftProgress craftProgress)
        {
            using TCon connection = OpenNewConnection();
            return ReplacePawnCraftProgress(connection, craftProgress);
        }

        public bool ReplacePawnCraftProgress(TCon connection, CraftProgress craftProgress)
        {
            Logger.Debug("Inserting pawn craft progress.");
            if (!InsertIfNotExistsPawnCraftProgress(connection, craftProgress))
            {
                Logger.Debug("Pawn craft progress already exists, replacing.");
                return UpdatePawnCraftProgress(connection, craftProgress);
            }

            return true;
        }

        public bool InsertPawnCraftProgress(CraftProgress craftProgress)
        {
            using TCon connection = OpenNewConnection();
            return InsertPawnCraftProgress(connection, craftProgress);
        }

        public bool InsertPawnCraftProgress(TCon connection, CraftProgress craftProgress)
        {
            return ExecuteNonQuery(connection, SqlInsertPawnCraftProgress, command => { AddAllParameters(command, craftProgress); }) == 1;
        }

        public bool InsertIfNotExistsPawnCraftProgress(CraftProgress craftProgress)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsPawnCraftProgress(connection, craftProgress);
        }

        public bool InsertIfNotExistsPawnCraftProgress(TCon connection, CraftProgress craftProgress)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsPawnCraftProgress, command => { AddAllParameters(command, craftProgress); }) == 1;
        }

        public bool UpdatePawnCraftProgress(CraftProgress craftProgress)
        {
            using TCon connection = OpenNewConnection();
            return UpdatePawnCraftProgress(connection, craftProgress);
        }

        public bool UpdatePawnCraftProgress(TCon connection, CraftProgress craftProgress)
        {
            return ExecuteNonQuery(connection, SqlUpdatePawnCraftProgress, command => { AddAllParameters(command, craftProgress); }) == 1;
        }

        public bool DeletePawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId)
        {
            using TCon connection = OpenNewConnection();
            return DeletePawnCraftProgress(connection, craftCharacterId, craftLeadPawnId);
        }

        public bool DeletePawnCraftProgress(TCon connection, uint craftCharacterId, uint craftLeadPawnId)
        {
            return ExecuteNonQuery(connection, SqlDeletePawnCraftProgress, command =>
            {
                AddParameter(command, "@craft_character_id", craftCharacterId);
                AddParameter(command, "@craft_lead_pawn_id", craftLeadPawnId);
            }) == 1;
        }

        public CraftProgress SelectPawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId)
        {
            using TCon connection = OpenNewConnection();
            return SelectPawnCraftProgress(connection, craftCharacterId, craftLeadPawnId);
        }

        public CraftProgress SelectPawnCraftProgress(TCon connection, uint craftCharacterId, uint craftLeadPawnId)
        {
            CraftProgress craftProgress = null;
            ExecuteReader(connection, SqlSelectPawnCraftProgress,
                command =>
                {
                    AddParameter(command, "@craft_character_id", craftCharacterId);
                    AddParameter(command, "@craft_lead_pawn_id", craftLeadPawnId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        craftProgress = ReadAllCraftProgressData(reader);
                    }
                });
            return craftProgress;
        }

        private CraftProgress ReadAllCraftProgressData(TReader reader)
        {
            CraftProgress craftProgress = new CraftProgress
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
                Unk0 = GetUInt16(reader, "unk0"),
                RemainTime = GetUInt32(reader, "remain_time"),
                ExpBonus = GetBoolean(reader, "exp_bonus"),
                CreateCount = GetUInt32(reader, "create_count"),

                PlusValue = GetUInt32(reader, "plus_value"),
                GreatSuccess = GetBoolean(reader, "great_success"),
                BonusExp = GetUInt32(reader, "bonus_exp"),
                AdditionalQuantity = GetUInt32(reader, "additional_quantity"),
            };

            return craftProgress;
        }

        private void AddAllParameters(DbCommand command, CraftProgress craftProgress)
        {
            AddParameter(command, "@craft_character_id", craftProgress.CraftCharacterId);
            AddParameter(command, "@craft_lead_pawn_id", craftProgress.CraftLeadPawnId);
            AddParameter(command, "@craft_support_pawn_id1", craftProgress.CraftSupportPawnId1);
            AddParameter(command, "@craft_support_pawn_id2", craftProgress.CraftSupportPawnId2);
            AddParameter(command, "@craft_support_pawn_id3", craftProgress.CraftSupportPawnId3);

            AddParameter(command, "@recipe_id", craftProgress.RecipeId);
            AddParameter(command, "@exp", craftProgress.Exp);
            AddParameter(command, "@npc_action_id", (int)craftProgress.NpcActionId);
            AddParameter(command, "@item_id", craftProgress.ItemId);
            AddParameter(command, "@unk0", craftProgress.Unk0);
            AddParameter(command, "@remain_time", craftProgress.RemainTime);
            AddParameter(command, "@exp_bonus", craftProgress.ExpBonus);
            AddParameter(command, "@create_count", craftProgress.CreateCount);

            AddParameter(command, "@plus_value", craftProgress.PlusValue);
            AddParameter(command, "@great_success", craftProgress.GreatSuccess);
            AddParameter(command, "@bonus_exp", craftProgress.BonusExp);
            AddParameter(command, "@additional_quantity", craftProgress.AdditionalQuantity);
        }
    }
}
