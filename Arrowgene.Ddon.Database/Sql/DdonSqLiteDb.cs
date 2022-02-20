using System;
using System.Data.SQLite;
using System.IO;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database.Sql
{
    /// <summary>
    /// SQLite Ddon database.
    /// </summary>
    public class DdonSqLiteDb : DdonSqlDb<SQLiteConnection, SQLiteCommand>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonSqLiteDb));


        public const string MemoryDatabasePath = ":memory:";
        public const int Version = 1;

        private readonly string _databasePath;
        private string _connectionString;
        private SQLiteConnection _memoryConnection;

        public DdonSqLiteDb(string databasePath)
        {
            _memoryConnection = null;
            _databasePath = databasePath;
            Logger.Info($"Database Path: {_databasePath}");
        }

        public bool CreateDatabase()
        {
            _connectionString = BuildConnectionString(_databasePath);
            if (_connectionString == null)
            {
                Logger.Error($"Failed to build connection string");
                return false;
            }

            if (_databasePath == MemoryDatabasePath)
            {
                throw new NotSupportedException("Connections are utilized via `using`, disposing the connection. In Memory DB only available for lifetime of connection");
                _memoryConnection = new SQLiteConnection(_connectionString);
                _memoryConnection.Open();
                return true;
            }

            if (!File.Exists(_databasePath))
            {
                FileStream fs = File.Create(_databasePath);
                fs.Close();
                fs.Dispose();
                Logger.Info($"Created new v{Version} database");
                return true;
            }

            return false;
        }

        private string BuildConnectionString(string source)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = source;
            builder.Version = 3;
            builder.ForeignKeys = true;
            // Set ADO.NET conformance flag https://system.data.sqlite.org/index.html/info/e36e05e299
            builder.Flags = builder.Flags & SQLiteConnectionFlags.StrictConformance;

            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override SQLiteConnection Connection()
        {
            SQLiteConnection connection = new SQLiteConnection(_connectionString);
            return connection.OpenAndReturn();
        }

        protected override SQLiteCommand Command(string query, SQLiteConnection connection)
        {
            return new SQLiteCommand(query, connection);
        }

        /// <summary>
        /// Thread Safe on Connection basis.
        /// http://www.sqlite.org/c3ref/last_insert_rowid.html
        /// </summary>
        protected override long AutoIncrement(SQLiteConnection connection, SQLiteCommand command)
        {
            return connection.LastInsertRowId;
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }
    }
}
