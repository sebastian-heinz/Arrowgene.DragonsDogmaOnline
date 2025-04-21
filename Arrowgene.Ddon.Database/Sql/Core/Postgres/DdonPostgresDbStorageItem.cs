using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql;

public partial class DdonPostgresDb
{
    private static readonly string SqlUpsertStorageItem =
        $"""
         INSERT INTO "ddon_storage_item" ({BuildQueryField(StorageItemFields)})
                        VALUES ({BuildQueryInsert(StorageItemFields)})
                        ON CONFLICT ("character_id", "storage_type", "slot_no")
                        DO UPDATE SET {BuildQueryUpdateWithPrefix("EXCLUDED.", StorageItemNonUniqueFields)};
         """;

    public override bool ReplaceStorageItem(
        uint characterId,
        StorageType storageType,
        ushort slotNo,
        uint itemNum,
        Item item,
        DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpsertStorageItem, cmd =>
            {
                AddParameter(cmd, "item_uid", item.UId);
                AddParameter(cmd, "character_id", characterId);
                AddParameter(cmd, "storage_type", (byte)storageType);
                AddParameter(cmd, "slot_no", slotNo);
                AddParameter(cmd, "item_id", item.ItemId);
                AddParameter(cmd, "item_num", itemNum);
                AddParameter(cmd, "safety", item.SafetySetting);
                AddParameter(cmd, "color", item.Color);
                AddParameter(cmd, "plus_value", item.PlusValue);
                AddParameter(cmd, "equip_points", item.EquipPoints);
            }) == 1;
        });
    }
}
