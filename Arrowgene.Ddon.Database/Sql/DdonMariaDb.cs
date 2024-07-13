using System;
using Arrowgene.Ddon.Database.Migrations;
using Arrowgene.Ddon.Database.Sql.Core;
using Arrowgene.Logging;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace Arrowgene.Ddon.Database.Sql
{
    public class DdonMariaDb : DdonSqlDb<MySqlConnection, MySqlCommand, MySqlDataReader>, IDatabase
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonMariaDb));

        private string _connectionString;

        public DdonMariaDb(string host, string user, string password, string database, bool wipeOnStartup)
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

            using (var serviceProvider = CreateServices())
            using (var scope = serviceProvider.CreateScope())
            {
                DdonDatabaseBuilder.UpdateDatabase(scope.ServiceProvider);
            }

            ReusableConnection = new MySqlConnection(_connectionString);
            return true;
        }

        private string BuildConnectionString(string host, string user, string password, string database)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
            {
                Server = host,
                UserID = user,
                Password = password,
                Database = database,
                IgnoreCommandTransaction = true,
                Pooling = true
            };
            string connectionString = builder.ToString();
            Logger.Info($"Connection String: {connectionString}");
            return connectionString;
        }

        public ServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add MariaDb support to FluentMigrator
                    .AddMySql5()
                    // Set the connection string
                    .WithGlobalConnectionString(_connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(DatabaseCreate).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        protected override MySqlConnection OpenNewConnection()
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
