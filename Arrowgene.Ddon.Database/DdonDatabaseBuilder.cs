using System;
using System.IO;
using System.Text;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Database.Sql;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database
{
    public static class DdonDatabaseBuilder
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonDatabaseBuilder));

        public static IDatabase Build(DatabaseSetting settings)
        {
            IDatabase database = settings.DbType switch
            {
                DatabaseType.SQLite => BuildSqLite(settings.DatabaseFolder, settings.WipeOnStartup),
                DatabaseType.PostgreSQL => BuildPostgres(settings.DatabaseFolder, settings.Host, settings.User, settings.Password, settings.Database, settings.WipeOnStartup),
                DatabaseType.MariaDb => BuildMariaDB(settings.DatabaseFolder, settings.Host, settings.User, settings.Password, settings.Database, settings.WipeOnStartup),
                _ => throw new ArgumentOutOfRangeException($"Unknown database type '{settings.Type}' encountered!")
            };

            if (database == null)
            {
                Logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }
            else
            {
                Logger.Info($"Database of type '${settings.DbType.ToString()}' has been created.");
                Logger.Info($"Database path: {settings.DatabaseFolder}");
            }

            return database;
        }

        public static DdonSqLiteDb BuildSqLite(string databaseFolder, bool wipeOnStartup)
        {
            string sqLitePath = Path.Combine(databaseFolder, $"db.v{DdonSqLiteDb.Version}.sqlite");
            DdonSqLiteDb db = new DdonSqLiteDb(sqLitePath, wipeOnStartup);
            if (db.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(databaseFolder, "Script/schema_sqlite.sql"));
            }

            return db;
        }

        public static DdonPostgresDb BuildPostgres(string databaseFolder, string host, string user, string password, string database, bool wipeOnStartup)
        {
            DdonPostgresDb db = new DdonPostgresDb(host, user, password, database, wipeOnStartup);
            if (db.CreateDatabase())
            {
                string schemaFilePath = Path.Combine(databaseFolder, "Script/schema_sqlite.sql");
                String schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);
                schema = schema.Replace(" DATETIME ", " TIMESTAMP WITH TIME ZONE ");
                schema = schema.Replace(" INTEGER PRIMARY KEY AUTOINCREMENT ", " SERIAL PRIMARY KEY ");
                schema = schema.Replace(" BLOB ", " BYTEA ");
                File.WriteAllText(schemaFilePath, schema);
                
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(databaseFolder, "Script/schema_postgres.sql"));
            }

            return db;
        }

        public static DdonMariaDb BuildMariaDB(string databaseFolder, string host, string user, string password, string database, bool wipeOnStartup)
        {
            DdonMariaDb db = new DdonMariaDb(host, user, password, database, wipeOnStartup);
            if (db.CreateDatabase())
            {
                string schemaFilePath = Path.Combine(databaseFolder, "Script/schema_sqlite.sql");
                String schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);
                schema = schema.Replace(" AUTOINCREMENT ", " AUTO_INCREMENT ");
                File.WriteAllText(schemaFilePath, schema);
                
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(databaseFolder, "Script/schema_mariadb.sql"));
            }

            return db;
        }
    }
}
