using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model.BattleContent;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] BitterBlackMazeContentTreasure =
    [
        "character_id", "content_id", "amount"
    ];

    private readonly string SqlDeleteBBMContentTreasure = "DELETE FROM \"ddon_bbm_content_treasure\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlInsertBBMContentTreasure =
        $"INSERT INTO \"ddon_bbm_content_treasure\" ({BuildQueryField(BitterBlackMazeContentTreasure)}) VALUES ({BuildQueryInsert(BitterBlackMazeContentTreasure)});";

    private readonly string SqlSelectBBMContentTreasure =
        $"SELECT {BuildQueryField(BitterBlackMazeContentTreasure)} FROM \"ddon_bbm_content_treasure\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlUpdateBBMContentTreasure =
        $"UPDATE \"ddon_bbm_content_treasure\" SET {BuildQueryUpdate(BitterBlackMazeContentTreasure)} WHERE \"character_id\"=@character_id and \"content_id\"=@content_id;";

    public override bool InsertBBMContentTreasure(uint characterId, BitterblackMazeTreasure treasure, DbConnection? connectionIn = null)
    {
        return InsertBBMContentTreasure(characterId, treasure.ContentId, treasure.Amount, connectionIn);
    }

    public override bool InsertBBMContentTreasure(uint characterId, uint contentId, uint amount, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlInsertBBMContentTreasure, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "content_id", contentId);
                AddParameter(command, "amount", amount);
            }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool UpdateBBMContentTreasure(uint characterId, BitterblackMazeTreasure treasure)
    {
        return UpdateBBMContentTreasure(characterId, treasure.ContentId, treasure.Amount);
    }

    public override bool UpdateBBMContentTreasure(uint characterId, uint contentId, uint amount)
    {
        using DbConnection connection = OpenNewConnection();
        return UpdateBBMContentTreasure(connection, characterId, contentId, amount);
    }

    public bool UpdateBBMContentTreasure(DbConnection connection, uint characterId, uint contentId, uint amount)
    {
        return ExecuteNonQuery(connection, SqlUpdateBBMContentTreasure, command =>
        {
            AddParameter(command, "character_id", characterId);
            AddParameter(command, "content_id", contentId);
            AddParameter(command, "amount", amount);
        }) == 1;
    }

    public override bool RemoveBBMContentTreasure(uint characterId)
    {
        using DbConnection connection = OpenNewConnection();
        return RemoveBBMContentTreasure(connection, characterId);
    }

    public bool RemoveBBMContentTreasure(DbConnection connection, uint characterId)
    {
        return ExecuteNonQuery(connection, SqlDeleteBBMContentTreasure, command => { AddParameter(command, "character_id", characterId); }) == 1;
    }

    public override List<BitterblackMazeTreasure> SelectBBMContentTreasure(uint characterId, DbConnection? connectionIn = null)
    {
        List<BitterblackMazeTreasure> results = new();
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectBBMContentTreasure, command => { AddParameter(command, "character_id", characterId); }, reader =>
            {
                if (reader.Read())
                {
                    BitterblackMazeTreasure result = new();
                    result.ContentId = GetUInt32(reader, "content_id");
                    result.Amount = GetUInt32(reader, "amount");
                    results.Add(result);
                }
            });
        });
        return results;
    }
}
