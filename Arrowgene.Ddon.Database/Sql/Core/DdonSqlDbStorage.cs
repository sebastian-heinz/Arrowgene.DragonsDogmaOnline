using System;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        protected static readonly string[] StorageFields = new string[]
        {
            "character_id", "storage_type", "slot_max", "item_sort"
        };

        private static readonly string SqlInsertStorage = $"INSERT INTO \"ddon_storage\" ({BuildQueryField(StorageFields)}) VALUES ({BuildQueryInsert(StorageFields)});";
        private static readonly string SqlInsertIfNotExistsStorage = $"INSERT INTO \"ddon_storage\" ({BuildQueryField(StorageFields)}) SELECT {BuildQueryInsert(StorageFields)} WHERE NOT EXISTS (SELECT 1 FROM \"ddon_storage\" WHERE \"storage_type\"=@storage_type AND \"character_id\" = @character_id);";
        private static readonly string SqlUpdateStorage = $"UPDATE \"ddon_storage\" SET {BuildQueryUpdate(StorageFields)} WHERE \"storage_type\" = @storage_type AND \"character_id\" = @character_id;";
        private static readonly string SqlSelectStorage = $"SELECT {BuildQueryField(StorageFields)} FROM \"ddon_storage\" WHERE \"storage_type\" = @storage_type AND \"character_id\" = @character_id;";
        private static readonly string SqlSelectAllStoragesByCharacter = $"SELECT {BuildQueryField(StorageFields)} FROM \"ddon_storage\" WHERE \"character_id\" = @character_id;";
        private static readonly string SqlDeleteStorage = "DELETE FROM \"ddon_storage\" WHERE \"storage_type\"=@storage_type AND \"character_id\" = @character_id;";

        public bool ReplaceStorage(uint characterId, StorageType storageType, Storage storage)
        {
            using TCon connection = OpenNewConnection();
            return ReplaceStorage(connection, characterId, storageType, storage);
        }

        public bool ReplaceStorage(TCon connection, uint characterId, StorageType storageType, Storage storage)
        {
            Logger.Debug("Inserting storage.");
            if (!InsertIfNotExistsStorage(connection, characterId, storageType, storage))
            {
                Logger.Debug("Storage already exists, replacing.");
                return UpdateStorage(connection, characterId, storageType, storage);
            }
            return true;
        }
        
        public bool InsertIfNotExistsStorage(uint characterId, StorageType storageType, Storage storage)
        {
            using TCon connection = OpenNewConnection();
            return InsertIfNotExistsStorage(connection, characterId, storageType, storage);
        }
        
        public bool InsertIfNotExistsStorage(TCon connection, uint characterId, StorageType storageType, Storage storage)
        {
            return ExecuteNonQuery(connection, SqlInsertIfNotExistsStorage, command =>
            {
                AddParameter(command, characterId, storageType, storage);
            }) == 1;
        }
        
        public bool InsertStorage(uint characterId, StorageType storageType, Storage storage)
        {
            using TCon connection = OpenNewConnection();
            return InsertStorage(connection, characterId, storageType, storage);
        }
        
        public bool InsertStorage(TCon connection, uint characterId, StorageType storageType, Storage storage)
        {
            return ExecuteNonQuery(connection, SqlInsertStorage, command =>
            {
                AddParameter(command, characterId, storageType, storage);
            }) == 1;
        }

        public bool UpdateStorage(uint characterId, StorageType storageType, Storage storage)
        {
            using TCon connection = OpenNewConnection();
            return UpdateStorage(connection, characterId, storageType, storage);
        }

        public bool UpdateStorage(TCon connection, uint characterId, StorageType storageType, Storage storage)
        {
            return ExecuteNonQuery(connection, SqlUpdateStorage, command =>
            {
                AddParameter(command, characterId, storageType, storage);
            }) == 1;
        }

        public bool DeleteStorage(uint characterId, StorageType storageType)
        {
            return ExecuteNonQuery(SqlDeleteStorage, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "storage_type", (byte) storageType);
            }) == 1;
        }

        private Tuple<StorageType, Storage> ReadStorage(TReader reader)
        {
            StorageType storageType = (StorageType) GetByte(reader, "storage_type");
            ushort slotMax = GetUInt16(reader, "slot_max");
            byte[] sortData = GetBytes(reader, "item_sort", 1024);
            return new Tuple<StorageType, Storage>(storageType, new Storage(storageType, slotMax, sortData));
        }

        private void AddParameter(TCom command, uint characterId, StorageType storageType, Storage storage)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "storage_type", (byte) storageType);
            AddParameter(command, "slot_max", storage.Items.Count);
            AddParameter(command, "item_sort", storage.SortData);
        }
    }
}
