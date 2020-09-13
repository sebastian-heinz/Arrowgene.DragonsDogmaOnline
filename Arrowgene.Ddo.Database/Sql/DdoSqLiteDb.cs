using System;
using System.Data.SQLite;
using System.IO;
using Arrowgene.Ddo.Database.Sql.Core;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.Database.Sql
{
    /// <summary>
    /// SQLite Ddo database.
    /// </summary>
    public class DdoSqLiteDb : DdoSqlDb<SQLiteConnection, SQLiteCommand>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdoSqLiteDb));

        
        public const string MemoryDatabasePath = ":memory:";
        public const int Version = 1;

        private const string SelectAutoIncrement = "SELECT last_insert_rowid()";

        private readonly string _databasePath;
        private string _connectionString;

        public DdoSqLiteDb(string databasePath)
        {
            _databasePath = databasePath;
            Logger.Info($"Database Path: {_databasePath}");
        }

        public bool CreateDatabase()
        {
            if (_databasePath != MemoryDatabasePath && !File.Exists(_databasePath))
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
            if (_connectionString == null)
            {
                _connectionString = BuildConnectionString(_databasePath);
            }

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
            // long autoIncrement = NoAutoIncrement;
            // ExecuteReader(SelectAutoIncrement, reader =>
            // {
            //     if (reader.Read())
            //     {
            //         autoIncrement = reader.GetInt32(0);
            //     }
            // });
            // return autoIncrement;
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }
    }
}
