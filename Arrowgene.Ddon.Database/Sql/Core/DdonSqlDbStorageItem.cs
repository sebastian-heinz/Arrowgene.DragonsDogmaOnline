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

        protected virtual string SqlReplaceStorageItem { get; } =
            $"INSERT OR REPLACE INTO \"ddon_storage_item\" ({BuildQueryField(StorageItemFields)}) VALUES ({BuildQueryInsert(StorageItemFields)});";

        private static readonly string SqlSelectStorageItemsByUId = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"item_uid\"=@item_uid;";
        private static readonly string SqlSelectStorageItemsByCharacter = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id;";
        private static readonly string SqlSelectStorageItemsByCharacterAndStorageType = $"SELECT {BuildQueryField(StorageItemFields)} FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type;";
        private static readonly string SqlDeleteStorageItem = "DELETE FROM \"ddon_storage_item\" WHERE \"character_id\"=@character_id AND \"storage_type\"=@storage_type AND \"slot_no\"=@slot_no;";

        public bool InsertStorageItem(TCon conn, uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
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
            return this.InsertStorageItem(null, characterId, storageType, slotNo, itemUId, itemNum);
        }

        public bool ReplaceStorageItem(TCon conn, uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            return ExecuteNonQuery(conn, SqlReplaceStorageItem, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
                AddParameter(command, "slot_no", slotNo);
                AddParameter(command, "item_uid", itemUId);
                AddParameter(command, "item_num", itemNum);
            }) == 1;
        }

        public bool ReplaceStorageItem(uint characterId, StorageType storageType, ushort slotNo, string itemUId, uint itemNum)
        {
            return this.ReplaceStorageItem(null, characterId, storageType, slotNo, itemUId, itemNum);
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
    }
}
