﻿using System;
using System.IO;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Database.Sql;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Database
{
    public class DdonDatabaseBuilder
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(DdonDatabaseBuilder));

        public DdonDatabaseBuilder()
        {
        }

        public IDatabase Build(DatabaseSetting settings)
        {
            IDatabase database = null;
            switch (settings.Type)
            {
                case DatabaseType.SQLite:
                    database = PrepareSqlLiteDb(settings.SqLiteFolder);
                    break;
            }

            if (database == null)
            {
                Logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }

            return database;
        }

        private DdonSqLiteDb PrepareSqlLiteDb(string sqLiteFolder)
        {
            string sqLitePath = Path.Combine(sqLiteFolder, $"db.v{DdonSqLiteDb.Version}.sqlite");
            DdonSqLiteDb db = new DdonSqLiteDb(sqLitePath);
            if (db.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/schema_sqlite.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_account.sql"));
            }

            return db;
        }
    }
}