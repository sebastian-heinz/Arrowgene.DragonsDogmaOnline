using System.Data.Common;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertToken = "INSERT INTO `ddo_token` (`account_id`, `token`, `created`) VALUES (@account_id, @token, @created);";
        private const string SqlUpdateToken = "UPDATE `ddo_token` SET `token`=@token, `created`=@created WHERE `account_id` = @account_id;";
        private const string SqlSelectToken = "SELECT `account_id`, `token`, `created` FROM `ddo_token` WHERE `account_id` = @account_id;";
        private const string SqlDeleteToken = "DELETE FROM `ddo_token` WHERE `account_id`=@account_id;";
        
        public bool SetToken(int accountId, GameToken token)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateToken, command =>
            {
                AddParameter(command, "@account_id", accountId);
                AddParameter(command, "@token", token.Token);
                AddParameter(command, "@created", token.Created);
            });
            if (rowsAffected > NoRowsAffected)
            {
                return true;
            }

            rowsAffected = ExecuteNonQuery(SqlInsertToken, command =>
            {
                AddParameter(command, "@account_id", accountId);
                AddParameter(command, "@token", token.Token);
                AddParameter(command, "@created", token.Created);
            });

            return rowsAffected > NoRowsAffected;
        }
        
        public GameToken SelectToken(int accountId)
        {
            GameToken token = null;
            ExecuteReader(SqlSelectToken, command =>
            {
                AddParameter(command, "@account_id", accountId);
            }, reader =>
            {
                token = new GameToken();
                token.AccountId = GetInt32(reader, "account_id");
                token.Token = GetString(reader, "token");
                token.Created = GetDateTime(reader, "created");
            });
            return token;
        }
        
        public bool DeleteToken(int accountId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteToken, command =>
            {
                AddParameter(command, "@account_id", accountId);
            });
            return rowsAffected > NoRowsAffected;
        }
    }
}
