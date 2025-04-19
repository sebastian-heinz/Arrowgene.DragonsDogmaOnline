using System;
using System.Data;
using System.Data.Common;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;
using Npgsql;

namespace Arrowgene.Ddon.Database.Sql;

public partial class DdonPostgresDb : DdonSqlDb
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonPostgresDb));

    private readonly string _connectionString;
    private NpgsqlDataSource _dataSource;

    public DdonPostgresDb(string host, string user, string password, string database, bool wipeOnStartup, uint bufferSize, bool resetOnClose)
    {
        _connectionString = BuildConnectionString(host, user, password, database, bufferSize, resetOnClose);
        if (wipeOnStartup) Logger.Info("WipeOnStartup is currently not supported.");
    }

    public override bool CreateDatabase()
    {
        if (_connectionString == null)
        {
            Logger.Error("Failed to build connection string");
            return false;
        }

        if (_dataSource == null)
        {
            NpgsqlDataSourceBuilder dataSourceBuilder = new(_connectionString);
            dataSourceBuilder.EnableParameterLogging();
            _dataSource = dataSourceBuilder.Build();
        }

        ReusableConnection = _dataSource.OpenConnection();

        // check to see if account table exists, if it does then dont run the global schema file
        string sql =
            "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'account') AS table_existence;";

        bool tableExists = false;
        ExecuteReader(sql,
            command => { }, reader =>
            {
                if (reader.Read()) tableExists = GetBoolean(reader, "table_existence");
            });

        return !tableExists;
    }

    public override void Stop()
    {
        Logger.Info("Stopping database connection.");
    }

    private string BuildConnectionString(string host, string user, string password, string database, uint bufferSize, bool resetOnClose)
    {
        NpgsqlConnectionStringBuilder builder = new()
        {
            Host = host,
            Username = user,
            Password = password,
            Database = database,
            MaxAutoPrepare = 200,
            MinPoolSize = 1,
            ConnectionLifetime = 0,
            ReadBufferSize = (int)bufferSize,
            WriteBufferSize = (int)bufferSize,
            NoResetOnClose = resetOnClose,
            SocketReceiveBufferSize = (int)bufferSize,
            SocketSendBufferSize = (int)bufferSize,
            Pooling = true,
            IncludeErrorDetail = true
        };
        string connectionString = builder.ToString();
        Logger.Info($"Connection String: {connectionString}");
        return connectionString;
    }

    public override NpgsqlConnection OpenNewConnection()
    {
        return _dataSource.OpenConnection();
    }

    protected override DbCommand Command(string query, DbConnection connection)
    {
        return new NpgsqlCommand(query, (NpgsqlConnection)connection);
    }

    /// <summary>
    ///     Safe within the same connection session (transaction?), but unsafe if triggers are involved.
    ///     https://stackoverflow.com/questions/2944297/postgresql-function-for-last-inserted-id
    /// </summary>
    protected override long AutoIncrement(DbConnection connection, DbCommand command)
    {
        using NpgsqlCommand lastIdCommand = new("SELECT LASTVAL();", connection as NpgsqlConnection);
        return (long)lastIdCommand.ExecuteScalar();
    }

    public override DateTime GetDateTime(DbDataReader reader, string column)
    {
        return DateTime.SpecifyKind(reader.GetDateTime(reader.GetOrdinal(column)), DateTimeKind.Utc);
    }

    public override DateTime? GetDateTimeNullable(DbDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal)) return null;

        return DateTime.SpecifyKind(reader.GetDateTime(ordinal), DateTimeKind.Utc);
    }

    public override ushort GetUInt16(DbDataReader reader, string column)
    {
        return unchecked((ushort)reader.GetInt32(reader.GetOrdinal(column)));
    }

    public override byte GetByte(DbDataReader reader, string column)
    {
        return unchecked((byte)reader.GetInt32(reader.GetOrdinal(column)));
    }

    #region: Parameter

    protected override DbParameter Parameter(DbCommand command, string name, object? value, DbType type)
    {
        throw new Exception("Do not use the generic parameter mapping for PSQL as it introduces a performance overhead due to autoboxing.");
    }

    public override void AddParameter(DbCommand command, string name, object? value, DbType type)
    {
        throw new Exception("Do not use the generic parameter mapping for PSQL as it introduces a performance overhead due to autoboxing.");
    }

    private static void AddTypedParameter<T>(DbCommand command, string name, T value)
    {
        command.Parameters.Add(new NpgsqlParameter<T>(name, value));
    }

    public override void AddParameter(DbCommand command, string name, string value)
    {
        AddTypedParameter(command, name, value);
    }

    public override void AddParameter(DbCommand command, string name, int value)
    {
        AddTypedParameter(command, name, value);
    }

    public override void AddParameter(DbCommand command, string name, float value)
    {
        AddTypedParameter(command, name, value);
    }

    public override void AddParameter(DbCommand command, string name, long value)
    {
        AddTypedParameter(command, name, value);
    }

    public override void AddParameter(DbCommand command, string name, ulong value)
    {
        AddTypedParameter(command, name, (long)value);
    }

    public override void AddParameter(DbCommand command, string name, byte value)
    {
        AddTypedParameter(command, name, value);
    }

    public override void AddParameter(DbCommand command, string name, ushort value)
    {
        AddTypedParameter(command, name, (int)value);
    }

    public override void AddParameter(DbCommand command, string name, uint value)
    {
        AddTypedParameter(command, name, (int)value);
    }

    public override void AddParameterEnumInt32<T>(DbCommand command, string name, T value)
    {
        AddTypedParameter(command, name, (int)(object)value);
    }

    public override void AddParameter(DbCommand command, string name, DateTime? value)
    {
        if (value.HasValue)
            AddTypedParameter(command, name, DateTime.SpecifyKind(value.Value, DateTimeKind.Utc));
        else
            command.Parameters.Add(new NpgsqlParameter(name, null));
    }

    public override void AddParameter(DbCommand command, string name, DateTime value)
    {
        AddTypedParameter(command, name, DateTime.SpecifyKind(value, DateTimeKind.Utc));
    }

    public override void AddParameter(DbCommand command, string name, byte[] value)
    {
        AddTypedParameter(command, name, value);
    }

    public override void AddParameter(DbCommand command, string name, bool value)
    {
        AddTypedParameter(command, name, value);
    }

    #endregion
}
