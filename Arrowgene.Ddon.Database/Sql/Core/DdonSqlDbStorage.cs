using System;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] StorageFields = new[]
    {
        "character_id", "storage_type", "slot_max", "item_sort"
    };

    private static readonly string SqlInsertStorage = $"INSERT INTO \"ddon_storage\" ({BuildQueryField(StorageFields)}) VALUES ({BuildQueryInsert(StorageFields)});";

    private static readonly string SqlUpdateStorage =
        $"UPDATE \"ddon_storage\" SET {BuildQueryUpdate(StorageFields)} WHERE \"storage_type\" = @storage_type AND \"character_id\" = @character_id;";

    private static readonly string SqlSelectStorage =
        $"SELECT {BuildQueryField(StorageFields)} FROM \"ddon_storage\" WHERE \"storage_type\" = @storage_type AND \"character_id\" = @character_id;";

    private static readonly string SqlSelectAllStoragesByCharacter = $"SELECT {BuildQueryField(StorageFields)} FROM \"ddon_storage\" WHERE \"character_id\" = @character_id;";
    private static readonly string SqlDeleteStorage = "DELETE FROM \"ddon_storage\" WHERE \"storage_type\"=@storage_type AND \"character_id\" = @character_id;";

    private static readonly string SqlUpsertStorage =
        $@"INSERT INTO ""ddon_storage"" ({BuildQueryField(StorageFields)}) VALUES ({BuildQueryInsert(StorageFields)}) ON CONFLICT (character_id, storage_type) DO UPDATE SET slot_max   = EXCLUDED.slot_max, item_sort  = EXCLUDED.item_sort;";

    public override bool ReplaceStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, conn =>
            ExecuteNonQuery(conn, SqlUpsertStorage, cmd =>
                AddParameter(cmd, characterId, storageType, storage)
            ) == 1);
    }

    public override bool InsertStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlInsertStorage, command => { AddParameter(command, characterId, storageType, storage); }) == 1; });
    }

    public override bool UpdateStorage(uint characterId, StorageType storageType, Storage storage, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlUpdateStorage, command => { AddParameter(command, characterId, storageType, storage); }) == 1; });
    }

    public override bool DeleteStorage(uint characterId, StorageType storageType, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection =>
            {
                return ExecuteNonQuery(connection, SqlDeleteStorage, command =>
                {
                    AddParameter(command, "character_id", characterId);
                    AddParameter(command, "storage_type", (byte)storageType);
                }) == 1;
            });
    }

    private Tuple<StorageType, Storage> ReadStorage(DbDataReader reader)
    {
        StorageType storageType = (StorageType)GetByte(reader, "storage_type");
        ushort slotMax = GetUInt16(reader, "slot_max");
        byte[] sortData = GetBytes(reader, "item_sort", 1024);
        return new Tuple<StorageType, Storage>(storageType, new Storage(storageType, slotMax, sortData));
    }

    private void AddParameter(DbCommand command, uint characterId, StorageType storageType, Storage storage)
    {
        AddParameter(command, "character_id", characterId);
        AddParameter(command, "storage_type", (byte)storageType);
        AddParameter(command, "slot_max", storage.Items.Count);
        AddParameter(command, "item_sort", storage.SortData);
    }
}
