#nullable enable
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] StorageItemFields = new[]
    {
        "item_uid", "character_id", "storage_type", "slot_no", "item_id", "item_num", "safety", "color", "plus_value", "equip_points"
    };

    private static readonly string SqlInsertStorageItem =
        $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) VALUES ({BuildQueryInsert(StorageItemFields)});";

    private static readonly string SqlInsertIfNotExistsStorageItem =
        $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) SELECT {BuildQueryInsert(StorageItemFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no);";

    private static readonly string SqlSelectStorageItemsByUId = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"item_uid\"=@item_uid;";

    private static readonly string SqlSelectStorageItemsByCharacter =
        $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id;";

    private static readonly string SqlSelectStorageItemsByCharacterAndStorageType =
        $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type;";

    private static readonly string SqlDeleteStorageItem =
        "DELETE FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no;";

    private static readonly string SqlUpdateStorageItem =
        $"UPDATE \"ddon_storage_item\" SET {BuildQueryUpdate(StorageItemFields)} WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no;";

    private static readonly string SqlUpdateEquipPoints =
        "UPDATE \"ddon_storage_item\" SET \"equip_points\" = @equip_points " +
        "WHERE \"item_uid\" = @item_uid;";

    private static readonly string SqlDeleteAllStorageItems = "DELETE FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id;";

    public override Item SelectStorageItemByUId(string uId, DbConnection connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            Item item = null;
            ExecuteReader(connection, SqlSelectStorageItemsByUId,
                command => { AddParameter(command, "@item_uid", uId); }, reader =>
                {
                    if (reader.Read())
                    {
                        item = new Item();
                        item.UId = GetString(reader, "item_uid");
                        item.ItemId = GetUInt32(reader, "item_id");
                        item.SafetySetting = GetByte(reader, "safety");
                        item.Color = GetByte(reader, "color");
                        item.PlusValue = GetByte(reader, "plus_value");
                        item.EquipPoints = GetUInt32(reader, "equip_points");
                    }
                });
            return item;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public bool InsertIfNotExistsStorageItem(DbConnection conn, uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
    {
        return ExecuteNonQuery(conn, SqlInsertIfNotExistsStorageItem, command =>
        {
            AddParameter(command, "item_uid", item.UId);
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "storage_type", (byte)storageType);
            AddParameter(command, "slot_no", slotNo);
            AddParameter(command, "item_id", item.ItemId);
            AddParameter(command, "item_num", itemNum);
            AddParameter(command, "safety", item.SafetySetting);
            AddParameter(command, "color", item.Color);
            AddParameter(command, "plus_value", item.PlusValue);
            AddParameter(command, "equip_points", item.EquipPoints);
        }) == 1;
    }

    public bool InsertIfNotExistsStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertIfNotExistsStorageItem(connection, characterId, storageType, slotNo, itemNum, item);
    }

    public override bool InsertStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlInsertStorageItem, command =>
            {
                AddParameter(command, "item_uid", item.UId);
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte)storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_id", item.ItemId);
                AddParameter(command, "item_num", itemNum);
                AddParameter(command, "safety", item.SafetySetting);
                AddParameter(command, "color", item.Color);
                AddParameter(command, "plus_value", item.PlusValue);
                AddParameter(command, "equip_points", item.EquipPoints);
            }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool ReplaceStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            Logger.Debug("Inserting storage item.");
            if (!InsertIfNotExistsStorageItem(connection, characterId, storageType, slotNo, itemNum, item))
            {
                Logger.Debug("Storage item already exists, replacing.");
                return UpdateStorageItem(characterId, storageType, slotNo, itemNum, item, connection);
            }

            return true;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool DeleteStorageItem(uint characterId, StorageType storageType, ushort slotNo, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlDeleteStorageItem, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte)storageType);
                AddParameter(command, "slot_no", slotNo);
            }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool UpdateStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlUpdateStorageItem, command =>
            {
                AddParameter(command, "item_uid", item.UId);
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte)storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_id", item.ItemId);
                AddParameter(command, "item_num", itemNum);
                AddParameter(command, "safety", item.SafetySetting);
                AddParameter(command, "color", item.Color);
                AddParameter(command, "plus_value", item.PlusValue);
                AddParameter(command, "equip_points", item.EquipPoints);
            }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override void DeleteAllStorageItems(DbConnection connection, uint characterId)
    {
        ExecuteNonQuery(connection, SqlDeleteAllStorageItems, command => { AddParameter(command, "character_id", characterId); });
    }

    public override bool UpdateItemEquipPoints(string uid, uint equipPoints, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateEquipPoints, command =>
            {
                AddParameter(command, "item_uid", uid);
                AddParameter(command, "equip_points", equipPoints);
            }) == 1;
        });
    }
}
