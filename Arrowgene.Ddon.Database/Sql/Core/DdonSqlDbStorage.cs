using System.Data.Common;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private static readonly string[] StorageFields = new string[]
        {
            "character_id", "storage_type", "slot_max"
        };

        private static readonly string SqlInsertStorage = $"INSERT INTO `ddon_storage` ({BuildQueryField(StorageFields)}) VALUES ({BuildQueryInsert(StorageFields)});";
        private static readonly string SqlReplaceStorage = $"INSERT OR REPLACE INTO `ddon_storage` ({BuildQueryField(StorageFields)}) VALUES ({BuildQueryInsert(StorageFields)});";
        private static readonly string SqlUpdateStorage = $"UPDATE `ddon_storage` SET {BuildQueryUpdate(StorageFields)} WHERE `storage_type` = @storage_type AND `character_id` = @character_id;";
        private static readonly string SqlSelectStorage = $"SELECT {BuildQueryField(StorageFields)} FROM `ddon_storage` WHERE `storage_type` = @storage_type AND `character_id` = @character_id;";
        private static readonly string SqlSelectAllStoragesByCharacter = $"SELECT {BuildQueryField(StorageFields)} FROM `ddon_storage` WHERE `character_id` = @character_id;";
        private static readonly string SqlDeleteStorage = "DELETE FROM `ddon_storage` WHERE `storage_type`=@storage_type AND `character_id` = @character_id;";

        public bool InsertStorage(uint characterId, CDataCharacterItemSlotInfo storage)
        {
            return ExecuteNonQuery(SqlInsertStorage, command =>
            {
                AddParameter(command, characterId, storage);
            }) == 1;
        }

        public bool UpdateStorage(uint characterId, CDataCharacterItemSlotInfo storage)
        {
            return ExecuteNonQuery(SqlUpdateStorage, command =>
            {
                AddParameter(command, characterId, storage);
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


        private CDataCharacterItemSlotInfo ReadStorage(DbDataReader reader)
        {
            CDataCharacterItemSlotInfo storage = new CDataCharacterItemSlotInfo();
            storage.StorageType = (StorageType) GetByte(reader, "storage_type");
            storage.SlotMax = GetUInt16(reader, "slot_max");
            return storage;
        }

        private void AddParameter(TCom command, uint characterId, CDataCharacterItemSlotInfo storage)
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "storage_type", (byte) storage.StorageType);
            AddParameter(command, "slot_max", storage.SlotMax);
        }
    }
}