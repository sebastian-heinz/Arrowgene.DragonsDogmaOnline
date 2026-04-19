using System.Collections.Generic;
using Arrowgene.Ddon.Database.Model;

namespace Arrowgene.Ddon.Database.Sql.Core;

public partial class DdonSqlDb : SqlDb
{
    private static readonly string[] ConnectionFields =
    [
        "server_id", "account_id", "type", "created", "character_id"
    ];

    private readonly string SqlInsertConnection =
        $"INSERT INTO \"ddon_connection\" ({BuildQueryField(ConnectionFields)}) VALUES ({BuildQueryInsert(ConnectionFields)});";

    private readonly string SqlSelectConnections =
        $"SELECT {BuildQueryField(ConnectionFields)} FROM \"ddon_connection\";";

    private readonly string SqlSelectConnectionsByAccountId =
        $"SELECT {BuildQueryField(ConnectionFields)} FROM \"ddon_connection\" WHERE \"account_id\" = @account_id;";

    private readonly string SqlDeleteConnectionsByAccountId =
        "DELETE FROM \"ddon_connection\" WHERE \"account_id\"=@account_id;";

    private readonly string SqlDeleteConnectionsByServerId =
        "DELETE FROM \"ddon_connection\" WHERE \"server_id\"=@server_id;";

    private readonly string SqlDeleteConnection =
        "DELETE FROM \"ddon_connection\" WHERE \"server_id\"=@server_id AND \"account_id\"=@account_id;";

    public override bool InsertConnection(Connection connection)
    {
        int rowsAffected = ExecuteNonQuery(SqlInsertConnection, command =>
        {
            AddParameter(command, "@server_id", connection.ServerId);
            AddParameter(command, "@account_id", connection.AccountId);
            AddParameterEnumInt32(command, "@type", connection.Type);
            AddParameter(command, "@created", connection.Created);
            AddParameter(command, "@character_id", connection.CharacterId);
        });

        return rowsAffected > NoRowsAffected;
    }

    public override List<Connection> SelectConnectionsByAccountId(int accountId)
    {
        List<Connection> connections = new();
        ExecuteReader(SqlSelectConnectionsByAccountId,
            command => { AddParameter(command, "@account_id", accountId); },
            reader =>
            {
                while (reader.Read())
                {
                    Connection connection = new();
                    connection.ServerId = GetInt32(reader, "server_id");
                    connection.AccountId = GetInt32(reader, "account_id");
                    connection.Type = GetEnumInt32<ConnectionType>(reader, "type");
                    connection.Created = GetDateTime(reader, "created");
                    if (!reader.IsDBNull(reader.GetOrdinal("character_id")))
                    {
                        connection.CharacterId = GetUInt32(reader, "character_id");
                    }
                    connections.Add(connection);
                }
            });
        return connections;
    }

    public override List<Connection> SelectConnections()
    {
        List<Connection> connections = new();
        ExecuteReader(SqlSelectConnections,
            reader =>
            {
                while (reader.Read())
                {
                    Connection connection = new();
                    connection.ServerId = GetInt32(reader, "server_id");
                    connection.AccountId = GetInt32(reader, "account_id");
                    connection.Type = GetEnumInt32<ConnectionType>(reader, "type");
                    connection.Created = GetDateTime(reader, "created");
                    if (!reader.IsDBNull(reader.GetOrdinal("character_id")))
                    {
                        connection.CharacterId = GetUInt32(reader, "character_id");
                    }
                    connections.Add(connection);
                }
            });
        return connections;
    }

    public override bool DeleteConnection(int serverId, int accountId)
    {
        int rowsAffected = ExecuteNonQuery(SqlDeleteConnection, command =>
        {
            AddParameter(command, "@server_id", serverId);
            AddParameter(command, "@account_id", accountId);
        });
        return rowsAffected > NoRowsAffected;
    }

    public override bool DeleteConnectionsByAccountId(int accountId)
    {
        int rowsAffected = ExecuteNonQuery(SqlDeleteConnectionsByAccountId,
            command => { AddParameter(command, "@account_id", accountId); });
        return rowsAffected > NoRowsAffected;
    }

    public override bool DeleteConnectionsByServerId(int serverId)
    {
        int rowsAffected = ExecuteNonQuery(SqlDeleteConnectionsByServerId,
            command => { AddParameter(command, "@server_id", serverId); });
        return rowsAffected > NoRowsAffected;
    }
}
