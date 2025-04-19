using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_recycle_equipment */
    protected static readonly string[] RecycleEquipmentFields = new[]
    {
        "character_id", "num_attempts"
    };

    private readonly string SqlInsertRecycleEquipmentRecord =
        $"INSERT INTO \"ddon_recycle_equipment\" ({BuildQueryField(RecycleEquipmentFields)}) VALUES ({BuildQueryInsert(RecycleEquipmentFields)});";

    private readonly string SqlSelectRecyleEquipmentRecord =
        $"SELECT {BuildQueryField(RecycleEquipmentFields)} FROM \"ddon_recycle_equipment\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlUpdateRecycleEquipmentRecord = "UPDATE \"ddon_recycle_equipment\" SET \"num_attempts\"=@num_attempts WHERE \"character_id\"=@character_id;";
    private readonly string SqlUpdateRecycleEquipmentRecords = "UPDATE ddon_recycle_equipment SET \"num_attempts\"=0;";

    public override bool InsertRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertRecycleEquipmentRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "num_attempts", numAttempts);
            }) == 1;
        });
    }

    public override bool UpdateRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateRecycleEquipmentRecord, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "num_attempts", numAttempts);
            }) == 1;
        });
    }

    public override bool HasRecycleEquipmentRecord(uint characterId, DbConnection? connectionIn = null)
    {
        bool foundRecord = false;
        ExecuteQuerySafe(connectionIn,
            connection =>
            {
                ExecuteReader(connection, SqlSelectRecyleEquipmentRecord, command => { AddParameter(command, "character_id", characterId); },
                    reader => { foundRecord = reader.Read(); });
            });
        return foundRecord;
    }

    public override bool UpsertRecycleEquipmentRecord(uint characterId, byte numAttempts, DbConnection? connectionIn = null)
    {
        return HasRecycleEquipmentRecord(characterId, connectionIn)
            ? UpdateRecycleEquipmentRecord(characterId, numAttempts, connectionIn)
            : InsertRecycleEquipmentRecord(characterId, numAttempts, connectionIn);
    }

    public override byte GetRecycleEquipmentAttempts(uint characterId, DbConnection? connectionIn = null)
    {
        byte attempts = 0;
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectRecyleEquipmentRecord, command => { AddParameter(command, "character_id", characterId); }, reader =>
            {
                if (reader.Read()) attempts = GetByte(reader, "num_attempts");
            });
        });
        return attempts;
    }

    public override void ResetRecyleEquipmentRecords(DbConnection? connectionIn = null)
    {
        ExecuteQuerySafe(connectionIn, connection => { ExecuteNonQuery(connection, SqlUpdateRecycleEquipmentRecords, command => { }); });
    }
}
