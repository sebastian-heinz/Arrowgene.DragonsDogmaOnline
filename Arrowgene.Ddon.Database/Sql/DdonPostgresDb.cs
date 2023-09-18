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
                var dataSourceBuilder = new NpgsqlDataSourceBuilder(_connectionString);
                dataSourceBuilder.EnableParameterLogging();
                _dataSource = dataSourceBuilder.Build();
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

        /// <summary>
        /// Safe within the same connection session (transaction?), but unsafe if triggers are involved.
        /// https://stackoverflow.com/questions/2944297/postgresql-function-for-last-inserted-id
        /// </summary>
        protected override long AutoIncrement(NpgsqlConnection connection, NpgsqlCommand command)
        {
            return (long)new NpgsqlCommand("SELECT LASTVAL();", connection).ExecuteScalar();
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }

        protected override string SqlInsertOrIgnoreItem =>
            $"INSERT INTO \"ddon_item\" ({BuildQueryField(ItemFields)}) VALUES ({BuildQueryInsert(ItemFields)}) ON CONFLICT DO NOTHING;";

        protected override string SqlReplaceCharacterJobData =>
            $"INSERT INTO \"ddon_character_job_data\" ({BuildQueryField(CDataCharacterJobDataFields)}) VALUES ({BuildQueryInsert(CDataCharacterJobDataFields)}) ON CONFLICT ON CONSTRAINT pk_character_job_data DO UPDATE SET {BuildQueryUpdateWithTempTable("excluded", CDataCharacterJobDataFields)};";
    }
}
