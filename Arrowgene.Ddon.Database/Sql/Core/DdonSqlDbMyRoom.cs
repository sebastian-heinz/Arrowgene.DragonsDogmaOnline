using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private static readonly string[] UnlockedItemFields =
    [
        "character_id", "category", "item_id"
    ];

    private static readonly string[] MyRoomCustomizationKeyFields =
    [
        "character_id", "layout_id"
    ];
    
    private static readonly string[] MyRoomCustomizationNonKeyFields =
    [
        "item_id"
    ];

    private static readonly string[] MyRoomCustomizationFields = MyRoomCustomizationKeyFields.Union(MyRoomCustomizationNonKeyFields).ToArray();
        
    private readonly string SqlDeleteMyRoomCustomization = "DELETE FROM \"ddon_myroom_customization\" WHERE \"character_id\"=@character_id AND \"item_id\"=@item_id;";

    private readonly string SqlInsertUnlockedItem =
        $"INSERT INTO \"ddon_unlocked_items\" ({BuildQueryField(UnlockedItemFields)}) VALUES ({BuildQueryInsert(UnlockedItemFields)}) ON CONFLICT DO NOTHING;";

    private readonly string SqlSelectMyRoomCustomization =
        $"SELECT {BuildQueryField(MyRoomCustomizationFields)} FROM \"ddon_myroom_customization\" WHERE \"character_id\" = @character_id;";

    private readonly string SqlSelectUnlockedItem = $"SELECT {BuildQueryField(UnlockedItemFields)} FROM \"ddon_unlocked_items\" WHERE \"character_id\" = @character_id;";

    private readonly string SqlUpsertMyRoomCustomization =
        $"INSERT INTO \"ddon_myroom_customization\" ({BuildQueryField(MyRoomCustomizationFields)}) VALUES ({BuildQueryInsert(MyRoomCustomizationFields)}) ON CONFLICT ({BuildQueryField(MyRoomCustomizationKeyFields)}) DO UPDATE SET {BuildQueryUpdateWithPrefix("EXCLUDED.",MyRoomCustomizationNonKeyFields)};";
    
    public override HashSet<(UnlockableItemCategory Category, uint Id)> SelectUnlockedItems(uint characterId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            HashSet<(UnlockableItemCategory Category, uint Id)> unlocks = new();
            ExecuteReader(connection,
                SqlSelectUnlockedItem,
                command => { AddParameter(command, "@character_id", characterId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        UnlockableItemCategory category = (UnlockableItemCategory)GetUInt32(reader, "category");
                        uint itemId = GetUInt32(reader, "item_id");

                        unlocks.Add((category, itemId));
                    }
                });
            return unlocks;
        });
    }

    public override bool InsertUnlockedItem(uint characterId, UnlockableItemCategory category, uint itemId, DbConnection? connectionIn = null)
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

    public override Dictionary<ItemId, byte> SelectMyRoomCustomization(uint characterId, DbConnection? connectionIn = null)
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
                        byte layoutId = GetByte(reader, "layout_id");
                        ItemId itemId = (ItemId)GetUInt32(reader, "item_id");

                        customizations[itemId] = layoutId;
                    }
                });

            return customizations;
        });
    }

    public override bool UpsertMyRoomCustomization(uint characterId, byte layoutId, uint itemId, DbConnection? connectionIn = null)
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

    public override bool DeleteMyRoomCustomization(uint characterId, uint itemId, DbConnection? connectionIn = null)
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
