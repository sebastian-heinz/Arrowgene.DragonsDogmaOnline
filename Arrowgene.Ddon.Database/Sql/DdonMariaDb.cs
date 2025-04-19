using System.Data.Common;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;
using MySqlConnector;

namespace Arrowgene.Ddon.Database.Sql;

public class DdonMariaDb : DdonSqlDb
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonMariaDb));

    private readonly string _connectionString;

    public DdonMariaDb(string host, string user, string password, string database, bool wipeOnStartup)
    {
        _connectionString = BuildConnectionString(host, user, password, database);
        if (wipeOnStartup) Logger.Info("WipeOnStartup is currently not supported.");
    }

    public override bool CreateDatabase()
    {
        if (_connectionString == null)
        {
            Logger.Error("Failed to build connection string");
            return false;
        }

        ReusableConnection = new MySqlConnection(_connectionString);
        return true;
    }

    public override void Stop()
    {
        Logger.Info("Stopping database connection.");
    }

    private string BuildConnectionString(string host, string user, string password, string database)
    {
        MySqlConnectionStringBuilder builder = new()
        {
            Server = host,
            UserID = user,
            Password = password,
            Database = database,
            IgnoreCommandTransaction = true,
            Pooling = true
        };
        string connectionString = builder.ToString();
        Logger.Info($"Connection String: {connectionString}");
        return connectionString;
    }

    public override MySqlConnection OpenNewConnection()
    {
        MySqlConnection connection = new(_connectionString);
        connection.Open();
        return connection;
    }

    protected override DbCommand Command(string query, DbConnection connection)
    {
        return new MySqlCommand(query, (MySqlConnection)connection);
    }

    /// <summary>
    ///     Always returns the first generated ID in a multi-statement environment. Ideally should be used on a per-connection
    ///     basis.
    ///     https://dev.mysql.com/doc/refman/8.0/en/information-functions.html#function_last-insert-id
    /// </summary>
    protected override long AutoIncrement(MySqlConnection connection, MySqlCommand command)
    {
        return command.LastInsertedId;
    }
}
