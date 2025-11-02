using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteCharacterStamp = "DELETE FROM \"ddon_stamp_bonus\" WHERE \"character_id\"=@character_id;";

    private static readonly string[] CDataStampFields = new[]
    {
        "character_id", "can_stamp", "consecutive_stamp", "total_stamp"
    };

    private static readonly string SqlUpdateCharacterStamp = $"UPDATE \"ddon_stamp_bonus\" SET {BuildQueryUpdate(CDataStampFields)} WHERE \"character_id\" = @character_id;";
    private static readonly string SqlSelectCharacterStamp = $"SELECT {BuildQueryField(CDataStampFields)} FROM \"ddon_stamp_bonus\" WHERE \"character_id\" = @character_id;";

    private static readonly string SqlInsertCharacterStamp = $"INSERT INTO \"ddon_stamp_bonus\" ({BuildQueryField(CDataStampFields)}) VALUES ({BuildQueryInsert(CDataStampFields)});";

    private static readonly string SqlResetAllStamps = @"
    UPDATE ddon_stamp_bonus
    SET
    consecutive_stamp = CASE
        WHEN can_stamp = TRUE THEN 0
        ELSE consecutive_stamp
    END,
    can_stamp = TRUE;";

    public override bool InsertCharacterStampData(uint id, CharacterStampBonus stampData, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertCharacterStamp, command => { AddParameter(command, id, stampData); }) == 1;
        });
    }

    public override bool UpdateCharacterStampData(uint id, CharacterStampBonus stampData, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlUpdateCharacterStamp, command => { AddParameter(command, id, stampData); }) == 1;
        });
    }

    public override int ResetCharacterStamps(DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlResetAllStamps, command => { });
        });
    }

    private void AddParameter(DbCommand command, uint id, CharacterStampBonus stampData)
    {
        AddParameter(command, "character_id", id);
        AddParameter(command, "can_stamp", stampData.CanStamp);
        AddParameter(command, "consecutive_stamp", stampData.ConsecutiveStamp);
        AddParameter(command, "total_stamp", stampData.TotalStamp);
    }

    private CharacterStampBonus ReadCharacterStampData(DbDataReader reader)
    {
        CharacterStampBonus stampData = new();
        stampData.CanStamp = GetBoolean(reader, "can_stamp");
        stampData.ConsecutiveStamp = GetUInt16(reader, "consecutive_stamp");
        stampData.TotalStamp = GetUInt16(reader, "total_stamp");

        return stampData;
    }
}
