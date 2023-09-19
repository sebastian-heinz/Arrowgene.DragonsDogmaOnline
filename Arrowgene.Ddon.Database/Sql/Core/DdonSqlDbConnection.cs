using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom, TReader> : SqlDb<TCon, TCom, TReader>
        where TCon : DbConnection
        where TCom : DbCommand
        where TReader : DbDataReader
    {
        private const string SqlInsertConnection =
            "INSERT INTO \"ddon_connection\" (\"server_id\", \"account_id\", \"type\", \"created\") VALUES (@server_id, @account_id, @type, @created);";

        private const string SqlSelectConnectionsByAccountId =
            "SELECT \"server_id\", \"account_id\", \"type\", \"created\" FROM \"ddon_connection\" WHERE \"account_id\" = @account_id;";

        private const string SqlDeleteConnectionsByAccountId =
            "DELETE FROM \"ddon_connection\" WHERE \"account_id\"=@account_id;";
        
        private const string SqlDeleteConnectionsByServerId =
            "DELETE FROM \"ddon_connection\" WHERE \"server_id\"=@server_id;";

        private const string SqlDeleteConnection =
            "DELETE FROM \"ddon_connection\" WHERE \"server_id\"=@server_id AND \"account_id\"=@account_id;";

        public bool InsertConnection(Connection connection)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertConnection, command =>
            {
                AddParameter(command, "@server_id", connection.ServerId);
                AddParameter(command, "@account_id", connection.AccountId);
                AddParameterEnumInt32(command, "@type", connection.Type);
                AddParameter(command, "@created", connection.Created);
            });

            return rowsAffected > NoRowsAffected;
        }

        public List<Connection> SelectConnectionsByAccountId(int accountId)
        {
            List<Connection> connections = new List<Connection>();
            ExecuteReader(SqlSelectConnectionsByAccountId,
                command => { AddParameter(command, "@account_id", accountId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        Connection connection = new Connection();
                        connection.ServerId = GetInt32(reader, "server_id");
                        connection.AccountId = GetInt32(reader, "account_id");
                        connection.Type = GetEnumInt32<ConnectionType>(reader, "type");
                        connection.Created = GetDateTime(reader, "created");
                        connections.Add(connection);
                    }
                });
            return connections;
        }

        public bool DeleteConnection(int serverId, int accountId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteConnection, command =>
            {
                AddParameter(command, "@server_id", serverId);
                AddParameter(command, "@account_id", accountId);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteConnectionsByAccountId(int accountId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteConnectionsByAccountId,
                command => { AddParameter(command, "@account_id", accountId); });
            return rowsAffected > NoRowsAffected;
        }
        
        public bool DeleteConnectionsByServerId(int serverId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteConnectionsByServerId,
                command => { AddParameter(command, "@server_id", serverId); });
            return rowsAffected > NoRowsAffected;
        }
    }
}
