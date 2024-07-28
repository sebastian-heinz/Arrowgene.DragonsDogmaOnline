using System;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] StorageItemFields = new string[]
        {
            "item_uid", "character_id", "storage_type", "slot_no", "item_num"
        };

        private static readonly string SqlInsertStorageItem = $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) VALUES ({BuildQueryInsert(StorageItemFields)});";
        private static readonly string SqlInsertIfNotExistsStorageItem = $"INSERT INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) SELECT {BuildQueryInsert(StorageItemFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no);"; 
        private static readonly string SqlSelectStorageItemsByUId = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"item_uid\"=@item_uid;";
        private static readonly string SqlSelectStorageItemsByCharacter = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id;";
        private static readonly string SqlSelectStorageItemsByCharacterAndStorageType = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type;";
        private static readonly string SqlDeleteStorageItem = "DELETE FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no;";
        private static readonly string SqlUpdateStorageItem = $"UPDATE \"ddon_storage_item\" SET {BuildQueryUpdate(StorageItemFields)} WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no;";
        
        public bool InsertIfNotExistsStorageItem(TCon conn, uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            return ExecuteNonQuery(conn, SqlInsertIfNotExistsStorageItem, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "item_num", itemNum);
            }) == 1;
        }

        public bool InsertIfNotExistsStorageItem(uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsStorageItem(connection, characterId, storageType, slotNo, itemUId, itemNum);
        }
        
        public bool InsertStorageItem(DbConnection conn, uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            return ExecuteNonQuery(conn, SqlInsertStorageItem, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "item_num", itemNum);
            }) == 1;
        }

        public bool InsertStorageItem(uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            using TCon connection = OpenNewConnection();
            return InsertStorageItem(connection, characterId, storageType, slotNo, itemUId, itemNum);
        }

        public bool ReplaceStorageItem(TCon conn, uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            Logger.Debug("Inserting storage item.");
            if (!InsertIfNotExistsStorageItem(conn, characterId, storageType, slotNo, itemUId, itemNum))
            {
                Logger.Debug("Storage item already exists, replacing.");
                return UpdateStorageItem(conn, characterId, storageType, slotNo, itemUId, itemNum);
            }
            return true;
        }

        public bool ReplaceStorageItem(uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceStorageItem(connection, characterId, storageType, slotNo, itemUId, itemNum);
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

        public bool UpdateStorageItem(uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            using TCon connection = OpenNewConnection();
            return UpdateStorageItem(connection, characterId, storageType, slotNo, itemUId, itemNum);
        }        
        
        public bool UpdateStorageItem(TCon connection, uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            return ExecuteNonQuery(connection, SqlUpdateStorageItem, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "item_num", itemNum);
            }) == 1;
        }
    }
}
