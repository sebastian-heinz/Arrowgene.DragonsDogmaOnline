using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] BitterBlackMazeCharacterMapFields =
    [
        "character_id", "bbm_character_id"
    ];

    private readonly string SqlInsertBBMCharacterId =
        $"INSERT INTO \"ddon_bbm_character_map\" ({BuildQueryField(BitterBlackMazeCharacterMapFields)}) VALUES ({BuildQueryInsert(BitterBlackMazeCharacterMapFields)});";

    private readonly string SqlSelectBBMCharacterId =
        $"SELECT {BuildQueryField(BitterBlackMazeCharacterMapFields)} FROM ddon_bbm_character_map WHERE \"character_id\"=@character_id;";

    private readonly string SqlSelectBBMNormalCharacterId =
        $"SELECT {BuildQueryField(BitterBlackMazeCharacterMapFields)} FROM ddon_bbm_character_map WHERE \"bbm_character_id\"=@bbm_character_id;";

    public override bool InsertBBMCharacterId(uint characterId, uint bbmCharacterId)
    {
        using DbConnection connection = OpenNewConnection();
        return InsertBBMCharacterId(connection, characterId, bbmCharacterId);
    }

    public bool InsertBBMCharacterId(DbConnection connection, uint characterId, uint bbmCharacterId)
    {
        return ExecuteNonQuery(connection, SqlInsertBBMCharacterId, command =>
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "bbm_character_id", bbmCharacterId);
        }) == 1;
    }

    public override uint SelectBBMCharacterId(uint characterId, DbConnection? connectionIn = null)
    {
        uint bbmCharacterId = 0;
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            ExecuteReader(connection, SqlSelectBBMCharacterId, command => { AddParameter(command, "character_id", characterId); }, reader =>
            {
                if (reader.Read()) bbmCharacterId = GetUInt32(reader, "bbm_character_id");
            });
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }

        return bbmCharacterId;
    }

    public override uint SelectBBMNormalCharacterId(uint bbmCharacterId)
    {
        using DbConnection connection = OpenNewConnection();
        return SelectBBMNormalCharacterId(connection, bbmCharacterId);
    }

    public uint SelectBBMNormalCharacterId(DbConnection connection, uint bbmCharacterId)
    {
        uint characterId = 0;
        ExecuteInTransaction(connection =>
        {
            ExecuteReader(connection, SqlSelectBBMNormalCharacterId, command => { AddParameter(command, "bbm_character_id", bbmCharacterId); }, reader =>
            {
                if (reader.Read()) characterId = GetUInt32(reader, "character_id");
            });
        });

        return characterId;
    }
}
