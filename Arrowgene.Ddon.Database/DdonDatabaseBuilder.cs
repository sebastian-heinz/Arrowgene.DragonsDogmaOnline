using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Database.Sql;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Logging;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Arrowgene.Ddon.Database
{
    public static class DdonDatabaseBuilder
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonDatabaseBuilder));
        private const string DefaultSchemaFile = "Script/schema_sqlite.sql";

        public const uint Version = 29;

        public static IDatabase Build(DatabaseSetting settings)
        {
            Enum.TryParse(settings.Type, true, out DatabaseType dbType);
            IDatabase database = dbType switch
            {
                DatabaseType.SQLite => BuildSqLite(settings.DatabaseFolder, settings.WipeOnStartup),
                DatabaseType.PostgreSQL => BuildPostgres(settings.DatabaseFolder, settings.Host, settings.User, settings.Password, settings.Database, settings.WipeOnStartup),
                DatabaseType.MariaDb => BuildMariaDB(settings.DatabaseFolder, settings.Host, settings.User, settings.Password, settings.Database, settings.WipeOnStartup),
                _ => throw new ArgumentOutOfRangeException($"Unknown database type '{settings.Type}' encountered!")
            };

            database.CreateMeta(new DatabaseMeta()
            {
                DatabaseVersion = Version
            });

            if (database == null)
            {
                Logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }
            else
            {
                Logger.Info($"Database of type '${dbType.ToString()}' has been created.");
                Logger.Info($"Database path: {settings.DatabaseFolder}");
            }

            return database;
        }

        public static string AdaptSQLiteSchemaToPostgreSQL(string schema)
        {
            schema = Regex.Replace(schema, @"TINYINT", "SMALLINT", RegexOptions.IgnoreCase);
            schema = Regex.Replace(schema, @"(\s)DATETIME(\s|,)", "$1TIMESTAMP WITH TIME ZONE$2", RegexOptions.IgnoreCase);
            schema = Regex.Replace(schema, @"(\s)INTEGER PRIMARY KEY AUTOINCREMENT(\s|,)", "$1SERIAL PRIMARY KEY$2", RegexOptions.IgnoreCase);
            schema = Regex.Replace(schema, @"(\s)BLOB(\s|,)", "$1BYTEA$2", RegexOptions.IgnoreCase);
            // Dangerous and requires super-user privileges, but rough one-line equivalent
            schema = Regex.Replace(schema, @"PRAGMA(\s*)foreign_keys=(\s*)(OFF|0)(\s*);", "SET session_replication_role='replica';", RegexOptions.IgnoreCase);
            schema = Regex.Replace(schema, @"PRAGMA(\s*)foreign_keys=(\s*)(ON|1)(\s*);", "SET session_replication_role='origin';", RegexOptions.IgnoreCase);
            return schema;
        }

        public static string AdaptSQLiteSchemaToMariaDB(string schema)
        {
            schema = Regex.Replace(schema, @"(\s)AUTOINCREMENT(\s|,)", "$1AUTO_INCREMENT$2", RegexOptions.IgnoreCase);
            schema = Regex.Replace(schema, @"PRAGMA(\s*)foreign_keys=(\s*)(OFF|0)(\s*);", "SET FOREIGN_KEY_CHECKS=0;", RegexOptions.IgnoreCase);
            schema = Regex.Replace(schema, @"PRAGMA(\s*)foreign_keys=(\s*)(ON|1)(\s*);", "SET FOREIGN_KEY_CHECKS=1;", RegexOptions.IgnoreCase);
            return schema;
        }

        public static string AdaptSQLiteSchemaTo(DatabaseType databaseType, string schema)
        {
            switch (databaseType)
            {
                case DatabaseType.SQLite:
                    return schema;
                case DatabaseType.PostgreSQL:
                    return AdaptSQLiteSchemaToPostgreSQL(schema);
                case DatabaseType.MariaDb:
                    return AdaptSQLiteSchemaToMariaDB(schema);
                default:
                    throw new NotImplementedException();
            }
        }

        public static string AdaptSQLiteSchemaTo(string databaseTypeString, string schema)
        {
            DatabaseType databaseType = (DatabaseType)Enum.Parse(typeof(DatabaseType), databaseTypeString, true);
            return AdaptSQLiteSchemaTo(databaseType, schema);
        }

        public static string GetAdaptedSchema(DatabaseSetting setting, string schemaPath)
        {
            string schemaFilePath = Path.Combine(setting.DatabaseFolder, schemaPath);
            string schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);
            return DdonDatabaseBuilder.AdaptSQLiteSchemaTo(setting.Type, schema);
        }

        public static string BuildSqLitePath(string databaseFolder)
        {
            return Path.Combine(databaseFolder, $"db.sqlite");
        }

        public static DdonSqLiteDb BuildSqLite(string databaseFolder, bool wipeOnStartup)
        {
            string sqLitePath = BuildSqLitePath(databaseFolder);
            DdonSqLiteDb db = new DdonSqLiteDb(sqLitePath, wipeOnStartup);
            if (db.CreateDatabase())
            {
                string schemaFilePath = Path.Combine(databaseFolder, DefaultSchemaFile);
                String schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);
                
                db.Execute(schema);

                Logger.Info($"Created new v{Version} database");
            }

            return db;
        }

        public static DdonPostgresDb BuildPostgres(string databaseFolder, string host, string user, string password, string database, bool wipeOnStartup)
        {
            DdonPostgresDb db = new DdonPostgresDb(host, user, password, database, wipeOnStartup);
            if (db.CreateDatabase())
            {
                string schemaFilePath = Path.Combine(databaseFolder, DefaultSchemaFile);
                String schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);
                schema = AdaptSQLiteSchemaToPostgreSQL(schema);
                
                db.Execute(schema);
            }

            return db;
        }

        public static DdonMariaDb BuildMariaDB(string databaseFolder, string host, string user, string password, string database, bool wipeOnStartup)
        {
            DdonMariaDb db = new DdonMariaDb(host, user, password, database, wipeOnStartup);
            if (db.CreateDatabase())
            {
                string schemaFilePath = Path.Combine(databaseFolder, DefaultSchemaFile);
                String schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);
                schema = AdaptSQLiteSchemaToMariaDB(schema);
                db.Execute(schema);
            }

            return db;
        }
    }
}
