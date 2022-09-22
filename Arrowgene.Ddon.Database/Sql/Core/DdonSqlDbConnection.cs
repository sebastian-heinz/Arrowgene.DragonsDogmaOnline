using System.Collections.Generic;
using System.Data.Common;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core
{
    public abstract partial class DdonSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertConnection =
            "INSERT INTO `ddon_connection` (`account_id`, `connection_type`, `created`) VALUES (@account_id, @connection_type, @created);";

        private const string SqlSelectConnection =
            "SELECT `account_id`, `connection_type`, `created` FROM `ddon_connection` WHERE `account_id` = @account_id;";

        private const string SqlDeleteConnections = "DELETE FROM `ddon_connection` WHERE `account_id`=@account_id;";

        private const string SqlDeleteConnection =
            "DELETE FROM `ddon_connection` WHERE `account_id`=@account_id AND `connection_type`=@connection_type ;";

        public bool InsertConnection(Connection connection)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertConnection, command =>
            {
                AddParameter(command, "@account_id", connection.AccountId);
                AddParameterEnumInt32(command, "@connection_type", connection.ConnectionType);
                AddParameter(command, "@created", connection.Created);
            });

            return rowsAffected > NoRowsAffected;
        }

        public List<Connection> SelectConnections(int accountId)
        {
            List<Connection> connections = new List<Connection>();
            ExecuteReader(SqlSelectConnection, command => { AddParameter(command, "@account_id", accountId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        Connection connection = new Connection();
                        connection.AccountId = GetInt32(reader, "account_id");
                        connection.ConnectionType = GetEnumInt32<ConnectionType>(reader, "connection_type");
                        connection.Created = GetDateTime(reader, "created");
                        connections.Add(connection);
                    }
                });
            return connections;
        }

        public bool DeleteConnection(int accountId, ConnectionType connectionType)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteConnection, command =>
            {
                AddParameter(command, "@account_id", accountId);
                AddParameterEnumInt32(command, "@connection_type", connectionType);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteConnections(int accountId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteConnections,
                command => { AddParameter(command, "@account_id", accountId); });
            return rowsAffected > NoRowsAffected;
        }
    }
}
