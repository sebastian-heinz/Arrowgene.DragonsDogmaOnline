using System.Data.Common;
using System.Linq;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private static readonly string[] GameTokenFields = ["account_id", "character_id", "token", "created"];
    private static readonly string[] GameTokenKeyFields = ["account_id"];
    private static readonly string[] GameTokenNonKeyFields = GameTokenFields.Except(GameTokenKeyFields).ToArray();
    private static readonly string SqlSelectToken = $"SELECT {BuildQueryField(GameTokenFields)} FROM \"ddon_game_token\" WHERE \"token\" = @token;";
    private static readonly string SqlInsertToken = $"INSERT INTO \"ddon_game_token\" ({BuildQueryField(GameTokenFields)}) VALUES ({BuildQueryInsert(GameTokenFields)});";
    private static readonly string SqlUpdateToken = $"UPDATE \"ddon_game_token\" SET {BuildQueryUpdate(GameTokenFields)} WHERE \"account_id\" = @account_id;";
    private const string SqlDeleteTokenByAccountId = "DELETE FROM \"ddon_game_token\" WHERE \"account_id\"=@account_id;";
    private const string SqlDeleteToken = "DELETE FROM \"ddon_game_token\" WHERE \"token\"=@token;";
    private readonly string SqlUpsertToken =
        $"""
         INSERT INTO "ddon_game_token" ({BuildQueryField(GameTokenFields)}) VALUES ({BuildQueryInsert(GameTokenFields)}) 
         ON CONFLICT ("account_id") DO UPDATE SET {BuildQueryUpdateWithPrefix("EXCLUDED.", GameTokenNonKeyFields)};
         """;

    public override bool ReplaceToken(GameToken token, DbConnection? connectionIn = null)
    {
        return ExecuteQuerySafe(connectionIn,
            connection => { return ExecuteNonQuery(connection, SqlUpsertToken, command =>
            {
                AddParameter(command, "@account_id", token.AccountId);
                AddParameter(command, "@character_id", token.CharacterId);
                AddParameter(command, "@token", token.Token);
                AddParameter(command, "@created", token.Created);
            }) == 1; });
    }

    public override GameToken SelectToken(string tokenStr, DbConnection? connectionIn = null)
    {
        GameToken token = null;
        ExecuteReader(SqlSelectToken, command => { AddParameter(command, "@token", tokenStr); }, reader =>
        {
            if (reader.Read())
            {
                token = new GameToken
                {
                    AccountId = GetInt32(reader, "account_id"),
                    CharacterId = GetUInt32(reader, "character_id"),
                    Token = GetString(reader, "token"),
                    Created = GetDateTime(reader, "created")
                };
            }
        });
        return token;
    }

    public override bool DeleteTokenByAccountId(int accountId, DbConnection? connectionIn = null)
    {
        int rowsAffected = ExecuteNonQuery(SqlDeleteTokenByAccountId, command => { AddParameter(command, "@account_id", accountId); });
        return rowsAffected > NoRowsAffected;
    }
}
