using System;
using System.Data;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;
using Npgsql;

namespace Arrowgene.Ddon.Database.Sql
{
    public class DdonPostgresDb : DdonSqlDb<NpgsqlConnection, NpgsqlCommand, NpgsqlDataReader>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonPostgresDb));

        private string _connectionString;
        private NpgsqlDataSource _dataSource;

        public DdonPostgresDb(string host, string user, string password, string database, bool wipeOnStartup)
        {
            _connectionString = BuildConnectionString(host, user, password, database);
            if (wipeOnStartup)
            {
                Logger.Info($"WipeOnStartup is currently not supported.");
            }
        }

        public bool CreateDatabase()
        {
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

            ReusableConnection = _dataSource.OpenConnection();
            return true;
        }

        private string BuildConnectionString(string host, string user, string password, string database)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Username = user,
                Password = password,
                Database = database,
                Pooling = true
            };
            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override NpgsqlConnection OpenNewConnection()
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
            using NpgsqlCommand lastIdCommand = new NpgsqlCommand("SELECT LASTVAL();", connection);
            return (long)lastIdCommand.ExecuteScalar();
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }

        protected override void AddParameter(NpgsqlCommand command, string name, DateTime? value)
        {
            if (value.HasValue)
            {
                AddParameter(command, name, DateTime.SpecifyKind(value.Value, DateTimeKind.Utc), DbType.DateTime);
            }
            else
            {
                AddParameter(command, name, value, DbType.DateTime);
            }
        }

        protected override void AddParameter(NpgsqlCommand command, string name, DateTime value)
        {
            AddParameter(command, name, DateTime.SpecifyKind(value, DateTimeKind.Utc), DbType.DateTime);
        }

        protected override DateTime GetDateTime(NpgsqlDataReader reader, string column)
        {
            return DateTime.SpecifyKind(reader.GetDateTime(reader.GetOrdinal(column)), DateTimeKind.Utc);
        }

        protected override DateTime? GetDateTimeNullable(NpgsqlDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return null;
            }

            return DateTime.SpecifyKind(reader.GetDateTime(ordinal), DateTimeKind.Utc);
        }
    }
}
