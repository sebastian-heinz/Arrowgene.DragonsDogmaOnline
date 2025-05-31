using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Database.Sql;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database;

public static class DdonDatabaseBuilder
{
    private const string DefaultSchemaFile = "Script/schema_sqlite.sql";

    public const uint Version = 44;
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonDatabaseBuilder));

    public static IDatabase Build(DatabaseSetting settings)
    {
        Enum.TryParse(settings.Type, true, out DatabaseType dbType);
        IDatabase database = dbType switch
        {
            DatabaseType.SQLite => BuildSqLite(settings),
            DatabaseType.SQLiteInMemory => BuildSqLite(settings, true),
            DatabaseType.PostgreSQL => BuildPostgres(settings),
            _ => throw new ArgumentOutOfRangeException($"Unknown database type '{settings.Type}' encountered!")
        };

        database.CreateMeta(new DatabaseMeta
        {
            DatabaseVersion = Version
        });
        Logger.Info($"Database of type '${dbType.ToString()}' has been created.");
        Logger.Info($"Database path: {settings.DatabaseFolder}");

        return database;
    }

    private static string AdaptSQLiteSchemaToPostgreSQL(string schema)
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

    private static string AdaptSQLiteSchemaTo(DatabaseType databaseType, string schema)
    {
        switch (databaseType)
        {
            case DatabaseType.SQLite:
                return schema;
            case DatabaseType.PostgreSQL:
                return AdaptSQLiteSchemaToPostgreSQL(schema);
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
        return AdaptSQLiteSchemaTo(setting.Type, schema);
    }

    private static string BuildSqLitePath(string databaseFolder)
    {
        return Path.Combine(databaseFolder, "db.sqlite");
    }

    public static DdonSqLiteDb BuildSqLite(DatabaseSetting settings, bool inMemory = false)
    {
        string sqLitePath = BuildSqLitePath(settings.DatabaseFolder);
        DdonSqLiteDb db = inMemory ?
            new DdonSqLiteInMemoryDb(sqLitePath, settings.WipeOnStartup, settings.BufferSize, settings.EnableTracing, settings.EnablePooling) 
            : new DdonSqLiteDb(sqLitePath, settings.WipeOnStartup, settings.BufferSize, settings.EnableTracing, settings.EnablePooling);
        if (db.CreateDatabase())
        {
            string schemaFilePath = Path.Combine(settings.DatabaseFolder, DefaultSchemaFile);
            string schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);

            db.Execute(schema);

            Logger.Info($"Created new v{Version} database");
        }

        return db;
    }

    private static DdonPostgresDb BuildPostgres(DatabaseSetting settings)
    {
        DdonPostgresDb db = new(settings.Host, settings.User, settings.Password, settings.Database, settings.WipeOnStartup, settings.BufferSize, settings.NoResetOnClose, settings.EnablePooling, settings.EnableTracing, settings.MaxAutoPrepare);
        if (db.CreateDatabase())
        {
            string schemaFilePath = Path.Combine(settings.DatabaseFolder, DefaultSchemaFile);
            string schema = File.ReadAllText(schemaFilePath, Encoding.UTF8);
            schema = AdaptSQLiteSchemaToPostgreSQL(schema);

            db.Execute(schema);
        }

        return db;
    }
}
