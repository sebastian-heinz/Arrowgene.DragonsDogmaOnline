using System.Data.SQLite;
using System.IO;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database.Sql;

/// <summary>
///     WARNING: Usage in production scenarios is discouraged, check caveats.
///     This database approach will make use of a file-based SQLite DB as a starting point to ingest all data into memory,
///     and afterward operate purely in-memory. Once the application performs a graceful shutdown it will backup the
///     database to disk.
///     A graceful shutdown can be triggered either by exiting ("e" key) console mode or by triggering SIGTERM/SIGINT
///     (ctrl+c) in service ("--service" flag) mode.
///     <see href="https://sqlite.org/inmemorydb.html">Check out the SQLite documentation.</see>
///     Caveats:
///     - In case of hard process crashes all progress while the DB resided in memory will be lost.
///     - While this approach avoids disk I/O, careful it requires a keep-alive connection as the DB is deleted once no
///     connections are present.
/// </summary>
public class DdonSqLiteInMemoryDb : DdonSqLiteDb
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonSqLiteInMemoryDb));
    private readonly string _fileDatabasePath;
    private SQLiteConnection _backupFileConnection;
    private SQLiteConnection _fileConnection;
    private bool _isRunning;
    private SQLiteConnection _keepAliveMemoryConnection;
    private string _memoryConnectionString;

    public DdonSqLiteInMemoryDb(string databasePath, bool wipeOnStartup, uint cacheSize, bool enableTracing = false, bool enablePooling = true) : base(databasePath, wipeOnStartup,
        cacheSize, enableTracing, enablePooling)
    {
        _fileDatabasePath = databasePath;

        _fileConnection = null;
        _backupFileConnection = null;
        _keepAliveMemoryConnection = null;
    }

    public override bool CreateDatabase()
    {
        bool databaseExists = File.Exists(_fileDatabasePath);
        if (!databaseExists)
            using (File.Create(_fileDatabasePath))
            {
                ;
            }

        // Establish file-based connection setup
        string fileConnectionString = BuildFileConnectionString(_fileDatabasePath, CacheSize, EnablePooling);
        string backupFileConnectionString = BuildFileConnectionString(_fileDatabasePath + ".backup", CacheSize, EnablePooling);
        if (fileConnectionString == null)
        {
            Logger.Error($"Failed to build connection string for {_fileDatabasePath}");
            return false;
        }

        _fileConnection = new SQLiteConnection(fileConnectionString);
        _backupFileConnection = new SQLiteConnection(backupFileConnectionString);

        // Establish memory-based connection setup
        _memoryConnectionString = BuildMemoryConnectionString(CacheSize, EnablePooling);
        _keepAliveMemoryConnection = new SQLiteConnection(_memoryConnectionString);
        _keepAliveMemoryConnection.Open();
        ReusableConnection = new SQLiteConnection(_memoryConnectionString);

        if (EnableTracing)
        {
            _fileConnection.TraceFlags = TraceLevel;
            _fileConnection.Trace2 += TraceSqLiteEvent;
            _backupFileConnection.TraceFlags = TraceLevel;
            _backupFileConnection.Trace2 += TraceSqLiteEvent;
            (ReusableConnection as SQLiteConnection).TraceFlags = TraceLevel;
            (ReusableConnection as SQLiteConnection).Trace2 += TraceSqLiteEvent;
        }

        // Load the file-based DB contents into memory
        LoadToMemory();

        _isRunning = true;
        // If database had to be freshly created ensure that initial schema creation will run.
        return !databaseExists;
    }

    public override void Stop()
    {
        if (_isRunning)
        {
            Logger.Info("Stopping database connection.");

            CreateBackup();
        }

        _isRunning = false;
    }

    private void LoadToMemory()
    {
        ReusableConnection.Open();
        _fileConnection.Open();

        _fileConnection.BackupDatabase(ReusableConnection as SQLiteConnection, ReusableConnection.Database, _fileConnection.Database, -1, null, 5000);

        _fileConnection.Close();
        ReusableConnection.Close();
    }

    private void CreateBackup()
    {
        _backupFileConnection.Open();
        _fileConnection.Open();
        ReusableConnection.Open();

        // Dump the in-memory DB to file to not lose progress by overwriting the original file.
        _fileConnection.BackupDatabase(_backupFileConnection, _backupFileConnection.Database, _fileConnection.Database, -1, null, 5000);
        (ReusableConnection as SQLiteConnection).BackupDatabase(_fileConnection, _fileConnection.Database, ReusableConnection.Database, -1, null, 5000);

        _backupFileConnection.Close();
        _fileConnection.Close();
        ReusableConnection.Close();
    }

    private static string BuildFileConnectionString(string source, uint cacheSize, bool enablePooling)
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

    private static string BuildMemoryConnectionString(uint cacheSize, bool enablePooling)
    {
        SQLiteConnectionStringBuilder builder = new()
        {
            DataSource = "memory.db",
            Version = 3,
            ForeignKeys = true,
            Pooling = enablePooling,
            CacheSize = -(int)cacheSize,
            // Set ADO.NET conformance flag https://system.data.sqlite.org/index.html/info/e36e05e299
            Flags = SQLiteConnectionFlags.Default | SQLiteConnectionFlags.StrictConformance
        };
        string connectionString = builder.ConnectionString + ";mode=memory;cache=shared";
        Logger.Info($"Connection String: {connectionString}");
        return connectionString;
    }

    public override SQLiteConnection OpenNewConnection()
    {
        SQLiteConnection openNewConnection = new SQLiteConnection(_memoryConnectionString).OpenAndReturn();
        if (EnableTracing)
        {
            openNewConnection.TraceFlags = TraceLevel;
            openNewConnection.Trace2 += TraceSqLiteEvent;
        }

        return openNewConnection;
    }
}
