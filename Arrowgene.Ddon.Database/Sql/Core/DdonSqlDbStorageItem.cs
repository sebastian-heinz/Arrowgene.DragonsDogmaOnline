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
            "item_uid", "character_id", "storage_type", "slot_no", "item_num", "item_id", "unk3", "color", "plus_value"
        };

        private static readonly string SqlInsertStorageItem = $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) VALUES ({BuildQueryInsert(StorageItemFields)});";
        private static readonly string SqlInsertIfNotExistsStorageItem = $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) SELECT {BuildQueryInsert(StorageItemFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no);"; 
        private static readonly string SqlSelectStorageItemByUId = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"item_uid\"=@item_uid;";
        private static readonly string SqlSelectStorageItemsByCharacter = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id;";
        private static readonly string SqlSelectStorageItemsByCharacterAndStorageType = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type;";
        private static readonly string SqlDeleteStorageItem = "DELETE FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no;";
        private static readonly string SqlUpdateStorageItem = $"UPDATE \"ddon_storage_item\" SET {BuildQueryUpdate(StorageItemFields)} WHERE \"character_id\"=@character_id AND \"item_uid\"=@item_uid;";

        public bool InsertIfNotExistsStorageItem(TCon conn, uint characterId, StorageType storageType, ushort slotNo, Item item, uint itemNum)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsStorageItem, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_uid", item.UId);
                AddParameter(command, "item_num", itemNum);
                AddParameter(command, "item_id", item.ItemId);
                AddParameter(command, "unk3", item.Unk3);
                AddParameter(command, "color", item.Color);
                AddParameter(command, "plus_value", item.PlusValue);
            }) == 1;
        }

        public bool InsertIfNotExistsStorageItem(uint characterId, StorageType storageType, ushort slotNo, Item item, uint itemNum)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsStorageItem(connection, characterId, storageType, slotNo, item, itemNum);
        }
        
        public bool InsertStorageItem(TCon conn, uint characterId, StorageType storageType, ushort slotNo, Item item, uint itemNum)
        {
            return ExecuteNonQuery(conn, SqlInsertStorageItem, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_uid", item.UId);
                AddParameter(command, "item_num", itemNum);
                AddParameter(command, "item_id", item.ItemId);
                AddParameter(command, "unk3", item.Unk3);
                AddParameter(command, "color", item.Color);
                AddParameter(command, "plus_value", item.PlusValue);
            }) == 1;
        }

        public bool InsertStorageItem(uint characterId, StorageType storageType, ushort slotNo, Item item, uint itemNum)
        {
            using TCon connection = OpenNewConnection();
            return InsertStorageItem(connection, characterId, storageType, slotNo, item, itemNum);
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
        
        public bool UpdateStorageItem(uint characterId, StorageType storageType, ushort slotNo, Item item, uint itemNum)
        {
            using TCon connection = OpenNewConnection();
            return UpdateStorageItem(connection, characterId, storageType, slotNo, item, itemNum);
        }        
        
        public bool UpdateStorageItem(TCon connection, uint characterId, StorageType storageType, ushort slotNo, Item item, uint itemNum)
        {
            return ExecuteNonQuery(connection, SqlUpdateStorageItem, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_uid", item.UId);
                AddParameter(command, "item_num", itemNum);
                AddParameter(command, "item_id", item.ItemId);
                AddParameter(command, "unk3", item.Unk3);
                AddParameter(command, "color", item.Color);
                AddParameter(command, "plus_value", item.PlusValue);
            }) == 1;
        }

        public Item SelectStorageItemByUId(string uid)
        {
            using TCon connection = OpenNewConnection();
            return SelectStorageItemByUId(connection, uid);
        }

        public Item SelectStorageItemByUId(TCon connection, string uid)
        {
            Item item = null;
            ExecuteInTransaction(conn =>
            {
                ExecuteReader(conn, SqlSelectStorageItemByUId,
                    command => {
                        AddParameter(command, "@item_uid", uid);
                    }, reader => {
                        if (reader.Read())
                        {
                            item = ReadStorageItem(reader);
                        }
                    });
            });
            return item;
        }

        private Item ReadStorageItem(TReader reader)
        {
            Item obj = new Item();
            obj.UId = GetString(reader, "item_uid");
            obj.ItemId = GetUInt32(reader, "item_id");
            obj.Unk3 = GetByte(reader, "unk3");
            obj.Color = GetByte(reader, "color");
            obj.PlusValue = GetByte(reader, "plus_value");
            return obj;
        }
    }
}
