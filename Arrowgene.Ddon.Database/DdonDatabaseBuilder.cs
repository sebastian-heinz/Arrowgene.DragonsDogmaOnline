using System;
using System.IO;
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
            IDatabase database = null;
            switch (settings.Type)
            {
                case DatabaseType.SQLite:
                    string sqLitePath = Path.Combine(settings.SqLiteFolder, $"db.v{DdonSqLiteDb.Version}.sqlite");
                    database = BuildSqLite(settings.SqLiteFolder, sqLitePath, settings.WipeOnStartup);
                    break;
                case DatabaseType.PostgreSQL:
                    database = BuildPostgres(settings);
                    break;              
                case DatabaseType.MariaDB:
                    database = BuildMariaDB(settings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown database type '{settings.Type}' encountered!");
            }

            if (database == null)
            {
                Logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }
            else
            {
                Logger.Info($"Database of type '${database.GetType()}' has been created.");
            }

            return database;
        }

        public static DdonSqLiteDb BuildSqLite(string sqLiteFolder, string sqLitePath, bool deleteIfExists)
        {
            if (deleteIfExists)
            {
                try
                {
                    File.Delete(sqLitePath);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            DdonSqLiteDb db = new DdonSqLiteDb(sqLitePath);
            if (db.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/schema_sqlite.sql"));
            }

            return db;
        }

        public static DdonPostgresDb BuildPostgres(DatabaseSetting settings)
        {
            DdonPostgresDb db = new DdonPostgresDb(settings);
            if (db.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(settings.SqLiteFolder, "Script/schema_postgres.sql"));
            }

            return db;
        }
        
        public static DdonMariaDb BuildMariaDB(DatabaseSetting settings)
        {
            DdonMariaDb db = new DdonMariaDb(settings);
            if (db.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(settings.SqLiteFolder, "Script/schema_mariadb.sql"));
            }

            return db;
        }
    }
}
