using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] PawnCraftProgressFields = {
            "craft_character_id", "craft_lead_pawn_id", "craft_support_pawn_id1", "craft_support_pawn_id2", "craft_support_pawn_id3", "recipe_id", "exp", "npc_action_id",
            "item_id", "unk0", "remain_time", "exp_bonus", "create_count"
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

        public bool ReplacePawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, uint craft_support_pawn_id1, uint craft_support_pawn_id2, uint craft_support_pawn_id3,
            uint recipe_id, uint exp, int npc_action_id, uint item_id, ushort unk0, uint remain_time, bool exp_bonus, uint create_count)
        {
            using TCon connection = OpenNewConnection();
            return ReplacePawnCraftProgress(connection, craftCharacterId, craftLeadPawnId, craft_support_pawn_id1, craft_support_pawn_id2, craft_support_pawn_id3, recipe_id, exp,
                npc_action_id, item_id, unk0, remain_time, exp_bonus, create_count);
        }

        public bool ReplacePawnCraftProgress(TCon connection, uint craftCharacterId, uint craftLeadPawnId, uint craft_support_pawn_id1, uint craft_support_pawn_id2,
            uint craft_support_pawn_id3, uint recipe_id, uint exp, int npc_action_id, uint item_id, ushort unk0, uint remain_time, bool exp_bonus, uint create_count)
        {
            Logger.Debug("Inserting pawn craft progress.");
            if (!InsertIfNotExistsPawnCraftProgress(connection, craftCharacterId, craftLeadPawnId, craft_support_pawn_id1, craft_support_pawn_id2, craft_support_pawn_id3,
                    recipe_id, exp, npc_action_id, item_id, unk0, remain_time, exp_bonus, create_count))
            {
                Logger.Debug("Pawn craft progress already exists, replacing.");
                return UpdatePawnCraftProgress(connection, craftCharacterId, craftLeadPawnId, craft_support_pawn_id1, craft_support_pawn_id2, craft_support_pawn_id3, recipe_id,
                    exp, npc_action_id, item_id, unk0, remain_time, exp_bonus, create_count);
            }

            return true;
        }

        public bool InsertPawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, uint craft_support_pawn_id1, uint craft_support_pawn_id2, uint craft_support_pawn_id3,
            uint recipe_id, uint exp, int npc_action_id, uint item_id, ushort unk0, uint remain_time, bool exp_bonus, uint create_count)
        {
            using TCon connection = OpenNewConnection();
            return InsertPawnCraftProgress(connection, craftCharacterId, craftLeadPawnId, craft_support_pawn_id1, craft_support_pawn_id2, craft_support_pawn_id3, recipe_id, exp,
                npc_action_id, item_id, unk0, remain_time, exp_bonus, create_count);
        }

        public bool InsertPawnCraftProgress(TCon connection, uint craftCharacterId, uint craftLeadPawnId, uint craft_support_pawn_id1, uint craft_support_pawn_id2,
            uint craft_support_pawn_id3, uint recipe_id, uint exp, int npc_action_id, uint item_id, ushort unk0, uint remain_time, bool exp_bonus, uint create_count)
        {
            return ExecuteNonQuery(connection, SqlInsertPawnCraftProgress, command =>
            {
                AddParameter(command, "@craft_character_id", craftCharacterId);
                AddParameter(command, "@craft_lead_pawn_id", craftLeadPawnId);
                AddParameter(command, "@craft_support_pawn_id1", craft_support_pawn_id1);
                AddParameter(command, "@craft_support_pawn_id2", craft_support_pawn_id2);
                AddParameter(command, "@craft_support_pawn_id3", craft_support_pawn_id3);
                AddParameter(command, "@recipe_id", recipe_id);
                AddParameter(command, "@exp", exp);
                AddParameter(command, "@npc_action_id", npc_action_id);
                AddParameter(command, "@item_id", item_id);
                AddParameter(command, "@unk0", unk0);
                AddParameter(command, "@remain_time", remain_time);
                AddParameter(command, "@exp_bonus", exp_bonus);
                AddParameter(command, "@create_count", create_count);
            }) == 1;
        }

        public bool InsertIfNotExistsPawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, uint craft_support_pawn_id1, uint craft_support_pawn_id2,
            uint craft_support_pawn_id3, uint recipe_id, uint exp, int npc_action_id, uint item_id, ushort unk0, uint remain_time, bool exp_bonus, uint create_count)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsPawnCraftProgress(connection, craftCharacterId, craftLeadPawnId, craft_support_pawn_id1, craft_support_pawn_id2, craft_support_pawn_id3,
                recipe_id, exp, npc_action_id, item_id, unk0, remain_time, exp_bonus, create_count);
        }

        public bool InsertIfNotExistsPawnCraftProgress(TCon connection, uint craftCharacterId, uint craftLeadPawnId, uint craft_support_pawn_id1, uint craft_support_pawn_id2,
            uint craft_support_pawn_id3, uint recipe_id, uint exp, int npc_action_id, uint item_id, ushort unk0, uint remain_time, bool exp_bonus, uint create_count)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsPawnCraftProgress, command =>
            {
                AddParameter(command, "@craft_character_id", craftCharacterId);
                AddParameter(command, "@craft_lead_pawn_id", craftLeadPawnId);
                AddParameter(command, "@craft_support_pawn_id1", craft_support_pawn_id1);
                AddParameter(command, "@craft_support_pawn_id2", craft_support_pawn_id2);
                AddParameter(command, "@craft_support_pawn_id3", craft_support_pawn_id3);
                AddParameter(command, "@recipe_id", recipe_id);
                AddParameter(command, "@exp", exp);
                AddParameter(command, "@npc_action_id", npc_action_id);
                AddParameter(command, "@item_id", item_id);
                AddParameter(command, "@unk0", unk0);
                AddParameter(command, "@remain_time", remain_time);
                AddParameter(command, "@exp_bonus", exp_bonus);
                AddParameter(command, "@create_count", create_count);
            }) == 1;
        }

        public bool UpdatePawnCraftProgress(uint craftCharacterId, uint craftLeadPawnId, uint craft_support_pawn_id1, uint craft_support_pawn_id2, uint craft_support_pawn_id3,
            uint recipe_id, uint exp, int npc_action_id, uint item_id, ushort unk0, uint remain_time, bool exp_bonus, uint create_count)
        {
            using TCon connection = OpenNewConnection();
            return UpdatePawnCraftProgress(connection, craftCharacterId, craftLeadPawnId, craft_support_pawn_id1, craft_support_pawn_id2, craft_support_pawn_id3, recipe_id, exp,
                npc_action_id, item_id, unk0, remain_time, exp_bonus, create_count);
        }

        public bool UpdatePawnCraftProgress(TCon connection, uint craftCharacterId, uint craftLeadPawnId, uint craft_support_pawn_id1, uint craft_support_pawn_id2,
            uint craft_support_pawn_id3, uint recipe_id, uint exp, int npc_action_id, uint item_id, ushort unk0, uint remain_time, bool exp_bonus, uint create_count)
        {
            return ExecuteNonQuery(connection, SqlUpdatePawnCraftProgress, command =>
            {
                AddParameter(command, "@craft_character_id", craftCharacterId);
                AddParameter(command, "@craft_lead_pawn_id", craftLeadPawnId);
                AddParameter(command, "@craft_support_pawn_id1", craft_support_pawn_id1);
                AddParameter(command, "@craft_support_pawn_id2", craft_support_pawn_id2);
                AddParameter(command, "@craft_support_pawn_id3", craft_support_pawn_id3);
                AddParameter(command, "@recipe_id", recipe_id);
                AddParameter(command, "@exp", exp);
                AddParameter(command, "@npc_action_id", npc_action_id);
                AddParameter(command, "@item_id", item_id);
                AddParameter(command, "@unk0", unk0);
                AddParameter(command, "@remain_time", remain_time);
                AddParameter(command, "@exp_bonus", exp_bonus);
                AddParameter(command, "@create_count", create_count);
            }) == 1;
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
                CreateCount = GetUInt32(reader, "create_count")
            };

            return craftProgress;
        }
    }
}
