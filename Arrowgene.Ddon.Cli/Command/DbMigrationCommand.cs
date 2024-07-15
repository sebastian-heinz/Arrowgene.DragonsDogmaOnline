using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;
using System;
using System.IO;

namespace Arrowgene.Ddon.Cli.Command
{
    public class DbMigrationCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ServerCommand));

        public string Key => "dbmigration";

        public string Description => $"Commandline tool to update the database\n\n" +
                                     $"usage:\n" +
                                     $"{Key} <previous version>\n" +
                                     $"The previous version argument is required only when using sqlite as the database engine.";

        public CommandResultType Run(CommandParameter parameter)
        {
           
            string settingPath = Path.Combine(Util.ExecutingDirectory(), "Files/Arrowgene.Ddon.config.json");
            Setting settings = Setting.Load(settingPath);
            if (settings == null)
            {
                return CommandResultType.Exit;
            }

            DatabaseSetting dbSettings = settings.DatabaseSetting;
            if (dbSettings == null)
            {
                return CommandResultType.Exit;
            }

            Enum.TryParse(dbSettings.Type, true, out DatabaseType dbType);
            if (dbType == DatabaseType.SQLite)
            {
                if (parameter.Arguments.Count < 1)
                {
                    Logger.Error("The previous version argument is required when using sqlite as the database engine.");
                    return CommandResultType.Exit;
                }

                if(!uint.TryParse(parameter.Arguments[0], out uint previousVersion))
                {
                    Logger.Error($"Failed to parse the version value '{parameter.Arguments[1]}'.");
                    return CommandResultType.Exit;
                }

                string oldSqlitePath = DdonDatabaseBuilder.BuildSqLitePath(dbSettings.DatabaseFolder, previousVersion);
                if (!File.Exists(oldSqlitePath))
                {
                    Logger.Error($"The previous version database file '{oldSqlitePath}' does not exist.");
                    return CommandResultType.Exit;
                }

                string newSqlitePath = DdonDatabaseBuilder.BuildSqLitePath(dbSettings.DatabaseFolder, DdonDatabaseBuilder.Version);
                if (File.Exists(newSqlitePath))
                {
                    Logger.Error($"Cannot migrate the database, the file '{newSqlitePath}' already exists.");
                    return CommandResultType.Exit;
                }

                File.Copy(oldSqlitePath, newSqlitePath);
            }

            IDatabase database = DdonDatabaseBuilder.Build(dbSettings);
            if (database == null)
            {
                return CommandResultType.Exit;
            }

            // TODO: Warn that migration is destructive
            bool result = database.MigrateDatabase(DdonDatabaseBuilder.Version);

            // TODO: Better logging
            if(result)
            {
                Logger.Info($"Successfully migrated the database to version '{DdonDatabaseBuilder.Version}'.");
            }
            else
            {
                Logger.Error("Failed to migrate the database.");
            }

            return CommandResultType.Exit;
        }

        public void Shutdown()
        {
        }
    }
}
