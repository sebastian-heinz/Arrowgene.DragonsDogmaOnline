using System;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;
using MySqlConnector;

namespace Arrowgene.Ddon.Database.Sql
{
    /// <summary>
    /// PostgreSQL Ddon database.
    /// </summary>
    public class DdonMariaDb : DdonSqlDb<MySqlConnection, MySqlCommand>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonMariaDb));

        private readonly DatabaseSetting _settings;
        private string _connectionString;

        public DdonMariaDb(DatabaseSetting settings)
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

            if (_settings.WipeOnStartup)
            {
                MySqlConnection connection = Connection();
                try
                {
                    MySqlCommand command = Command("DROP SCHEMA public CASCADE;", connection);
                    command.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    connection.Close();
                }
            }

            return true;
        }

        private string BuildConnectionString(DatabaseSetting settings)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = settings.Host;
            builder.UserID = settings.User;
            builder.Password = settings.Password;
            builder.Database = settings.Database;
            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        protected override MySqlConnection Connection()
        {
            MySqlConnection connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }

        protected override MySqlCommand Command(string query, MySqlConnection connection)
        {
            return new MySqlCommand(query, connection);
        }

        /// <summary>
        /// Always returns the first generated ID in a multi-statement environment. Ideally should be used on a per-connection basis.
        /// https://dev.mysql.com/doc/refman/8.0/en/information-functions.html#function_last-insert-id
        /// </summary>
        protected override long AutoIncrement(MySqlConnection connection, MySqlCommand command)
        {
            return command.LastInsertedId;
        }

        public override int Upsert(string table, string[] columns, object[] values, string whereColumn,
            object whereValue,
            out long autoIncrement)
        {
            throw new NotImplementedException();
        }
    }
}
