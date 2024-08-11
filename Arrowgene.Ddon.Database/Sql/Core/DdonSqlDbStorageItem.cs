using System;
using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] StorageItemFields = new string[]
        {
            "item_uid", "character_id", "storage_type", "slot_no", "item_id", "item_num", "unk3", "color", "plus_value", "equip_points"
        };

        private static readonly string SqlInsertStorageItem = $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) VALUES ({BuildQueryInsert(StorageItemFields)});";
        private static readonly string SqlInsertIfNotExistsStorageItem = $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) SELECT {BuildQueryInsert(StorageItemFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no);"; 
        private static readonly string SqlSelectStorageItemsByUId = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"item_uid\"=@item_uid;";
        private static readonly string SqlSelectStorageItemsByCharacter = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id;";
        private static readonly string SqlSelectStorageItemsByCharacterAndStorageType = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type;";
        private static readonly string SqlDeleteStorageItem = "DELETE FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no;";
        private static readonly string SqlUpdateStorageItem = $"UPDATE \"ddon_storage_item\" SET {BuildQueryUpdate(StorageItemFields)} WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no;";
        private static readonly string SqlUpdateEquipPoints =
            "UPDATE \"ddon_storage_item\" SET \"equip_points\" = @equip_points " +
            "WHERE \"item_uid\" = @item_uid;";
        public Item SelectStorageItemByUId(string uId)
        {
            using TCon connection = OpenNewConnection();
            return SelectStorageItemByUId(connection, uId);
        }

        public Item SelectStorageItemByUId(TCon connection, string uId)
        {
            Item item = null;
            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectStorageItemsByUId,
                    command => {
                        AddParameter(command, "@item_uid", uId);
                    }, reader => {
                        if (reader.Read())
                        {
                            item = new Item();
                            item.UId = GetString(reader, "item_uid");
                            item.ItemId = GetUInt32(reader, "item_id");
                            item.Unk3 = GetByte(reader, "unk3");
                            item.Color = GetByte(reader, "color");
                            item.PlusValue = GetByte(reader, "plus_value");
                            item.EquipPoints = GetUInt32(reader, "equip_points");
                        }
                    });
            });

            return item;
        }

        public bool InsertIfNotExistsStorageItem(TCon conn, uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsStorageItem, command =>
            {
                AddParameter(command, "item_uid", item.UId);
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_id", item.ItemId);
                AddParameter(command, "item_num", itemNum);
                AddParameter(command, "unk3", item.Unk3);
                AddParameter(command, "color", item.Color);
                AddParameter(command, "plus_value", item.PlusValue);
                AddParameter(command, "equip_points", item.EquipPoints);
            }) == 1;
        }

        public bool InsertIfNotExistsStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsStorageItem(connection, characterId, storageType, slotNo, itemNum, item);
        }
        
        public bool InsertStorageItem(DbConnection conn, uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
        {
            return ExecuteNonQuery(conn, SqlInsertStorageItem, command =>
            {
                AddParameter(command, "item_uid", item.UId);
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_id", item.ItemId);
                AddParameter(command, "item_num", itemNum);
                AddParameter(command, "unk3", item.Unk3);
                AddParameter(command, "color", item.Color);
                AddParameter(command, "plus_value", item.PlusValue);
                AddParameter(command, "equip_points", item.EquipPoints);
            }) == 1;
        }

        public bool InsertStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
        {
            using TCon connection = OpenNewConnection();
            return InsertStorageItem(connection, characterId, storageType, slotNo, itemNum, item);
        }

        public bool ReplaceStorageItem(DbConnection conn, uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
        {
            Logger.Debug("Inserting storage item.");
            if (!InsertIfNotExistsStorageItem((TCon)conn, characterId, storageType, slotNo, itemNum, item))
            {
                Logger.Debug("Storage item already exists, replacing.");
                return UpdateStorageItem(conn, characterId, storageType, slotNo, itemNum, item);
            }
            return true;
        }

        public bool ReplaceStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceStorageItem(connection, characterId, storageType, slotNo, itemNum, item);
        }

        public bool DeleteStorageItem(uint characterId, StorageType storageType, ushort slotNo)
        {
            return ExecuteNonQuery(SqlDeleteStorageItem, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
            }) == 1;
        }

        public bool UpdateStorageItem(uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
        {
            using TCon connection = OpenNewConnection();
            return UpdateStorageItem(connection, characterId, storageType, slotNo, itemNum, item);
        }        
        
        public bool UpdateStorageItem(DbConnection connection, uint characterId, StorageType storageType, ushort slotNo, uint itemNum, Item item)
        {
            return ExecuteNonQuery(connection, SqlUpdateStorageItem, command =>
            {
                AddParameter(command, "item_uid", item.UId);
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_id", item.ItemId);
                AddParameter(command, "item_num", itemNum);
                AddParameter(command, "unk3", item.Unk3);
                AddParameter(command, "color", item.Color);
                AddParameter(command, "plus_value", item.PlusValue);
                AddParameter(command, "equip_points", item.EquipPoints);
            }) == 1;
        }
        
        public bool UpdateItemEquipPoints(string uid, uint equipPoints)
        {
            using (TCon connection = OpenNewConnection())
            {
                return ExecuteNonQuery(connection, SqlUpdateEquipPoints, command =>
                {
                    AddParameter(command, "item_uid", uid);
                    AddParameter(command, "equip_points", equipPoints);
                }) == 1;
            }
        }

    }
}
