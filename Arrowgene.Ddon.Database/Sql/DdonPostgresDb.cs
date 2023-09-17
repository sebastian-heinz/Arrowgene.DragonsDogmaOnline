using System;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;
using Npgsql;

namespace Arrowgene.Ddon.Database.Sql
{
    /// <summary>
    /// PostgreSQL Ddon database.
    /// </summary>
    public class DdonPostgresDb : DdonSqlDb<NpgsqlConnection, NpgsqlCommand>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonPostgresDb));

        private readonly DatabaseSetting _settings;
        private string _connectionString;
        private NpgsqlDataSource _dataSource;

        public DdonPostgresDb(DatabaseSetting settings)
        {
            _settings = settings;
            Logger.Info($"Database Path: {settings.SqLiteFolder}");
        }

        public bool CreateDatabase()
        {
            _connectionString = BuildConnectionString(_settings);
            if (_connectionString == null)
            {
                Logger.Error($"Failed to build connection string");
                return false;
            }

            if (_dataSource == null)
            {
                _dataSource = NpgsqlDataSource.Create(_connectionString);
            }

            if (_settings.WipeOnStartup)
            {
                try
                {
                    NpgsqlCommand command = _dataSource.CreateCommand("DROP SCHEMA public CASCADE;");
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return true;
        }

        private string BuildConnectionString(DatabaseSetting settings)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.Host = settings.Host;
            builder.Username = settings.User;
            builder.Password = settings.Password;
            builder.Database = settings.Database;
            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override NpgsqlConnection Connection()
        {
            return _dataSource.OpenConnection();
        }

        protected override NpgsqlCommand Command(string query, NpgsqlConnection connection)
        {
            return new NpgsqlCommand(query, connection);
        }

        protected override long AutoIncrement(NpgsqlConnection connection, NpgsqlCommand command)
        {
            return 0;
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }
    }
}
