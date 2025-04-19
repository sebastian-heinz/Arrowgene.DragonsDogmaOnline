using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private const string SqlDeleteCharacterStamp = "DELETE FROM \"ddon_stamp_bonus\" WHERE \"character_id\"=@character_id;";

    private static readonly string[] CDataStampFields = new[]
    {
        "character_id", "last_stamp_time", "consecutive_stamp", "total_stamp"
    };

    private static readonly string SqlUpdateCharacterStamp = $"UPDATE \"ddon_stamp_bonus\" SET {BuildQueryUpdate(CDataStampFields)} WHERE \"character_id\" = @character_id;";
    private static readonly string SqlSelectCharacterStamp = $"SELECT {BuildQueryField(CDataStampFields)} FROM \"ddon_stamp_bonus\" WHERE \"character_id\" = @character_id;";

    private readonly string SqlInsertCharacterStamp = $"INSERT INTO \"ddon_stamp_bonus\" ({BuildQueryField(CDataStampFields)}) VALUES ({BuildQueryInsert(CDataStampFields)});";

    public override bool InsertCharacterStampData(uint id, CharacterStampBonus stampData)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertCharacterStampData(connection, id, stampData);
    }

    public bool InsertCharacterStampData(DbConnection connection, uint id, CharacterStampBonus stampData)
    {
        return ExecuteNonQuery(connection, SqlInsertCharacterStamp, command => { AddParameter(command, id, stampData); }) == 1;
    }

    public override bool UpdateCharacterStampData(uint id, CharacterStampBonus stampData)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateCharacterStampData(connection, id, stampData);
    }

    public bool UpdateCharacterStampData(DbConnection connection, uint id, CharacterStampBonus stampData)
    {
        return ExecuteNonQuery(connection, SqlUpdateCharacterStamp, command => { AddParameter(command, id, stampData); }) == 1;
    }

    private void AddParameter(DbCommand command, uint id, CharacterStampBonus stampData)
    {
        AddParameter(command, "character_id", id);
        AddParameter(command, "last_stamp_time", stampData.LastStamp);
        AddParameter(command, "consecutive_stamp", stampData.ConsecutiveStamp);
        AddParameter(command, "total_stamp", stampData.TotalStamp);
    }

    private CharacterStampBonus ReadCharacterStampData(DbDataReader reader)
    {
        CharacterStampBonus stampData = new();
        stampData.LastStamp = GetDateTime(reader, "last_stamp_time");
        stampData.ConsecutiveStamp = GetUInt16(reader, "consecutive_stamp");
        stampData.TotalStamp = GetUInt16(reader, "total_stamp");

        return stampData;
    }
}
