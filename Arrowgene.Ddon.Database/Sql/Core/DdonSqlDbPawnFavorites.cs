using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    /* ddon_pawn_favorites */
    protected static readonly string[] PawnFavoriteFields = new[]
    {
        "character_id", "pawn_id"
    };

    private readonly string SqlInsertPawnFavorite =
        $"INSERT INTO \"ddon_pawn_favorites\" ({BuildQueryField(PawnFavoriteFields)}) VALUES ({BuildQueryInsert(PawnFavoriteFields)});";

    private readonly string SqlDeletePawnFavorite =
        "DELETE FROM \"ddon_pawn_favorites\" WHERE \"character_id\"=@character_id AND \"pawn_id\"=@pawn_id;";

    private readonly string SqlSelectAllPawnFavorites =
        $"SELECT {BuildQueryField(PawnFavoriteFields)} FROM \"ddon_pawn_favorites\" WHERE \"character_id\"=@character_id;";

    public override bool InsertPawnFavorite(uint characterId, uint pawnId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlInsertPawnFavorite, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "pawn_id", pawnId);
            }) == 1;
        });
    }

    public override bool DeletePawnFavorite(uint characterId, uint pawnId, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn, connection =>
        {
            return ExecuteNonQuery(connection, SqlDeletePawnFavorite, command =>
            {
                AddParameter(command, "character_id", characterId);
                AddParameter(command, "pawn_id", pawnId);
            }) == 1;
        });
    }

    public override HashSet<uint> GetPawnFavorites(uint characterId, DbConnection? connectionIn = null)
    {
        var results = new HashSet<uint>();
        ExecuteQuerySafe(connectionIn, connection =>
        {
            ExecuteReader(connection, SqlSelectAllPawnFavorites, command =>
            {
                AddParameter(command, "character_id", characterId);
            }, reader =>
            {
                while (reader.Read())
                    results.Add(GetUInt32(reader, "pawn_id"));
            });
        });
        return results;
    }
}
