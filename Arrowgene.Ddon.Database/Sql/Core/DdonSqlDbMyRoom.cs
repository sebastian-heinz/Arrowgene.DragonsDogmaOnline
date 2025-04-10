using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        private static readonly string[] UnlockedItemFields = new string[]
        {
            "character_id", "category", "item_id"
        };

        private readonly string SqlSelectUnlockedItem = $"SELECT {BuildQueryField(UnlockedItemFields)} FROM \"ddon_unlocked_items\" WHERE \"character_id\" = @character_id;";
        private readonly string SqlInsertUnlockedItem = $"INSERT INTO \"ddon_unlocked_items\" ({BuildQueryField(UnlockedItemFields)}) VALUES ({BuildQueryInsert(UnlockedItemFields)}) ON CONFLICT DO NOTHING;";

        private static readonly string[] MyRoomCustomizationFields = new string[]
        {
            "character_id", "layout_id", "item_id"
        };

        private readonly string SqlSelectMyRoomCustomization = $"SELECT {BuildQueryField(MyRoomCustomizationFields)} FROM \"ddon_myroom_customization\" WHERE \"character_id\" = @character_id;";
        private readonly string SqlUpsertMyRoomCustomization = $"INSERT INTO \"ddon_myroom_customization\" ({BuildQueryField(MyRoomCustomizationFields)}) VALUES ({BuildQueryInsert(MyRoomCustomizationFields)}) ON CONFLICT DO UPDATE SET {BuildQueryUpdate(MyRoomCustomizationFields)};";
        private readonly string SqlDeleteMyRoomCustomization = $"DELETE FROM \"ddon_myroom_customization\" WHERE \"character_id\"=@character_id AND \"item_id\"=@item_id;";

        public HashSet<(UnlockableItemCategory Category, uint Id)> SelectUnlockedItems(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                var unlocks = new HashSet<(UnlockableItemCategory Category, uint Id)>();
                ExecuteReader(connection,
                    SqlSelectUnlockedItem,
                    command => { AddParameter(command, "@character_id", characterId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            var category = (UnlockableItemCategory)GetUInt32(reader, "category");
                            var itemId = GetUInt32(reader, "item_id");

                            unlocks.Add((category, itemId));
                        }
                    });
                return unlocks;
            });
        }

        public bool InsertUnlockedItem(uint characterId, UnlockableItemCategory category, uint itemId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(
                    connection,
                    SqlInsertUnlockedItem,
                    command =>
                    {
                        AddParameter(command, "character_id", characterId);
                        AddParameter(command, "category", (byte)category);
                        AddParameter(command, "item_id", itemId);
                    }
                    ) == 1;
            });
        }

        public Dictionary<ItemId, byte> SelectMyRoomCustomization(uint characterId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                Dictionary<ItemId, byte> customizations = new();
                ExecuteReader(connection,
                    SqlSelectMyRoomCustomization, 
                    command => { AddParameter(command, "@character_id", characterId); },
                    reader =>
                    {
                        while (reader.Read())
                        {
                            var layoutId = GetByte(reader, "layout_id");
                            var itemId = (ItemId)GetUInt32(reader, "item_id");

                            customizations[itemId] = layoutId;
                        }
                    });

                return customizations;
            });
        }

        public bool UpsertMyRoomCustomization(uint characterId, byte layoutId, uint itemId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(
                    connection,
                    SqlUpsertMyRoomCustomization,
                    command =>
                    {
                        AddParameter(command, "character_id", characterId);
                        AddParameter(command, "layout_id", layoutId);
                        AddParameter(command, "item_id", itemId);
                    }
                    ) == 1;
            });
        }

        public bool DeleteMyRoomCustomization(uint characterId, uint itemId, DbConnection? connectionIn = null)
        {
            return ExecuteQuerySafe(connectionIn, connection =>
            {
                return ExecuteNonQuery(
                    connection,
                    SqlDeleteMyRoomCustomization,
                    command =>
                    {
                        AddParameter(command, "character_id", characterId);
                        AddParameter(command, "item_id", itemId);
                    }
                    ) >= 1;
            });
        }
    }
}
