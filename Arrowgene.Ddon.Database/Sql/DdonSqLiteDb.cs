using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database.Sql;

public class DdonSqLiteDb : DdonSqlDb
{
    protected const SQLiteTraceFlags TraceLevel = SQLiteTraceFlags.SQLITE_TRACE_ALL;
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonSqLiteDb));
    private readonly string _databasePath;
    protected readonly bool EnableTracing;
    protected readonly bool EnablePooling;
    /// <summary>
    /// Check out https://sqlite.org/pragma.html#pragma_cache_size.
    /// </summary>
    protected readonly uint CacheSize;
    private string _connectionString;

    public DdonSqLiteDb(string databasePath, bool wipeOnStartup, uint cacheSize, bool enableTracing = false, bool enablePooling = true)
    {
        _databasePath = databasePath;
        EnableTracing = enableTracing;
        EnablePooling = enablePooling;
        CacheSize = cacheSize;
        if (wipeOnStartup)
            try
            {
                File.Delete(_databasePath);
                Logger.Info("Database has been wiped.");
            }
            catch (Exception)
            {
                Logger.Error("Failed to wipe database.");
            }
    }

    public override bool CreateDatabase()
    {
        _connectionString = BuildConnectionString(_databasePath, CacheSize, EnablePooling);
        if (_connectionString == null)
        {
            Logger.Error("Failed to build connection string");
            return false;
        }

        ReusableConnection = new SQLiteConnection(_connectionString);
        if (EnableTracing)
        {
            ((SQLiteConnection)ReusableConnection).TraceFlags = TraceLevel;
            ((SQLiteConnection)ReusableConnection).Trace2 += TraceSqLiteEvent;
        }

        if (!File.Exists(_databasePath))
        {
            FileStream fs = File.Create(_databasePath);
            fs.Close();
            fs.Dispose();
            return true;
        }

        return false;
    }

    public override void Stop()
    {
        Logger.Info("Stopping database connection.");
        ReusableConnection.Close();
        ReusableConnection.Dispose();
    }

    private string BuildConnectionString(string source, uint cacheSize, bool enablePooling)
    {
        SQLiteConnectionStringBuilder builder = new()
        {
            DataSource = source,
            Version = 3,
            ForeignKeys = true,
            Pooling = enablePooling,
            CacheSize = -(int)cacheSize,
            // Set ADO.NET conformance flag https://system.data.sqlite.org/index.html/info/e36e05e299
            Flags = SQLiteConnectionFlags.Default | SQLiteConnectionFlags.StrictConformance
        };

        string connectionString = builder.ToString();
        Logger.Info($"Connection String: {connectionString}");
        return connectionString;
    }

    public override DbConnection OpenNewConnection()
    {
        SQLiteConnection openNewConnection = new SQLiteConnection(_connectionString).OpenAndReturn();
        if (EnableTracing)
        {
            openNewConnection.TraceFlags = TraceLevel;
            openNewConnection.Trace2 += TraceSqLiteEvent;
        }

        return openNewConnection;
    }

    protected override DbCommand Command(string query, DbConnection connection)
    {
        return new SQLiteCommand(query, (SQLiteConnection)connection);
    }

    /// <summary>
    ///     Thread Safe on Connection basis.
    ///     http://www.sqlite.org/c3ref/last_insert_rowid.html
    /// </summary>
    protected override long AutoIncrement(DbConnection connection, DbCommand command)
    {
        return ((SQLiteConnection)connection).LastInsertRowId;
    }

    protected void TraceSqLiteEvent(object sender, TraceEventArgs e)
    {
        Logger.Debug($"statement={e.Statement};preparedStatement={e.PreparedStatement};elapsed={e.Elapsed}");
    }
}
