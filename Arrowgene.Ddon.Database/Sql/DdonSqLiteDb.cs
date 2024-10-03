﻿using System;
using System.Data.SQLite;
using System.IO;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database.Sql
{
    public class DdonSqLiteDb : DdonSqlDb<SQLiteConnection, SQLiteCommand, SQLiteDataReader>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonSqLiteDb));


        public const string MemoryDatabasePath = ":memory:";

        private readonly string _databasePath;
        private string _connectionString;
        private SQLiteConnection _memoryConnection;

        public DdonSqLiteDb(string databasePath, bool wipeOnStartup)
        {
            _memoryConnection = null;
            _databasePath = databasePath;
            if (wipeOnStartup)
            {
                try
                {
                    File.Delete(_databasePath);
                    Logger.Info($"Database has been wiped.");
                }
                catch (Exception)
                {
                    Logger.Error($"Failed to wipe database.");
                }
            }
        }

        public override bool CreateDatabase()
        {
            _connectionString = BuildConnectionString(_databasePath);
            if (_connectionString == null)
            {
                Logger.Error($"Failed to build connection string");
                return false;
            }

            ReusableConnection = new SQLiteConnection(_connectionString);

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
                return true;
            }

            return false;
        }

        private string BuildConnectionString(string source)
        {
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder
            {
                DataSource = source,
                Version = 3,
                ForeignKeys = true,
                Pooling = true,
                // Set ADO.NET conformance flag https://system.data.sqlite.org/index.html/info/e36e05e299
                Flags = SQLiteConnectionFlags.Default | SQLiteConnectionFlags.StrictConformance
            };

            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override SQLiteConnection OpenNewConnection()
        {
            return new SQLiteConnection(_connectionString).OpenAndReturn();
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
