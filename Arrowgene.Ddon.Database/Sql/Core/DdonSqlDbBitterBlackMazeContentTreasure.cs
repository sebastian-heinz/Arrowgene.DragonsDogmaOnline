using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Shared.Model.BattleContent;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    protected static readonly string[] BitterBlackMazeContentTreasure =
    [
        "character_id", "stage_id", "group_id", "index"
    ];

    private readonly string SqlDeleteBBMContentTreasure = "DELETE FROM \"ddon_bbm_content_treasure\" WHERE \"character_id\"=@character_id;";

    private readonly string SqlInsertBBMContentTreasure =
        $"INSERT INTO \"ddon_bbm_content_treasure\" ({BuildQueryField(BitterBlackMazeContentTreasure)}) VALUES ({BuildQueryInsert(BitterBlackMazeContentTreasure)}) ON CONFLICT DO NOTHING;";

    private readonly string SqlSelectBBMContentTreasure =
        $"SELECT {BuildQueryField(BitterBlackMazeContentTreasure)} FROM \"ddon_bbm_content_treasure\" WHERE \"character_id\"=@character_id;";

    public override bool InsertBBMContentTreasure(uint characterId, uint stageId, uint groupId, uint index, DbConnection? connectionIn = null)
    {
        bool isTransaction = connectionIn is not null;
        DbConnection connection = connectionIn ?? OpenNewConnection();
        try
        {
            return ExecuteNonQuery(connection, SqlInsertBBMContentTreasure, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "stage_id", stageId);
                AddParameter(command, "group_id", groupId);
                AddParameter(command, "index", index);
            }) == 1;
        }
        finally
        {
            if (!isTransaction) connection.Dispose();
        }
    }

    public override bool RemoveBBMContentTreasure(uint characterId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlDeleteBBMContentTreasure, command => { AddParameter(command, "character_id", characterId); }) == 1;
        });
    }

    public override List<BitterblackMazeTreasure> SelectBBMContentTreasure(uint characterId, DbConnection? connectionIn = null)
    {
        List<BitterblackMazeTreasure> results = [];
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectBBMContentTreasure, command => { AddParameter(command, "character_id", characterId); }, reader =>
            {
                while (reader.Read())
                {
                    results.Add(new()
                    {
                        LayoutId = new(
                            GetUInt32(reader, "stage_id"),
                            0,
                            GetUInt32(reader, "group_id")
                        ),
                        Index = GetUInt32(reader, "index")
                    });
                }
            });
        });
        return results;
    }
}
