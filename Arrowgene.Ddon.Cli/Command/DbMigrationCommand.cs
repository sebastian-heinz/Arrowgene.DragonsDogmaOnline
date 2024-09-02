using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Sql.Core.Migration;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;
using System.Linq;
using System.IO;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Cli.Command
{
    public class DbMigrationCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ServerCommand));

        public string Key => "dbmigration";

        public string Description => $"Commandline tool to update the database\n\n" +
                                     $"usage:\n" +
                                     $"{Key}\n";

        public CommandResultType Run(CommandParameter parameter)
        {
           
            string settingPath = Path.Combine(Util.ExecutingDirectory(), "Files/Arrowgene.Ddon.config.json");
            Setting settings = Setting.LoadFromFile(settingPath);
            if (settings == null)
            {
                return CommandResultType.Exit;
            }

            DatabaseSetting dbSettings = settings.DatabaseSetting;
            if (dbSettings == null)
            {
                return CommandResultType.Exit;
            }

            IDatabase database = DdonDatabaseBuilder.Build(dbSettings);
            if (database == null)
            {
                return CommandResultType.Exit;
            }

            // Instance all implementations of IMigrationStrategy located in the Migration namespace
            // supplying the adequate arguments to the constructor
            DatabaseMigrator migrator = new DatabaseMigrator(typeof(IMigrationStrategy).Assembly.GetTypes()
                .Where(type => type != typeof(IMigrationStrategy) && typeof(IMigrationStrategy).IsAssignableFrom(type) && type.Namespace == typeof(IMigrationStrategy).Namespace)
                .Select(type => InstanceMigrationStrategy(type, dbSettings))
                .ToList());

            // TODO: Warn that migration is destructive
            bool result = database.MigrateDatabase(migrator, DdonDatabaseBuilder.Version);

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

        private IMigrationStrategy InstanceMigrationStrategy(Type type, DatabaseSetting databaseSetting)
        {
            foreach(ConstructorInfo constructorInfo in type.GetConstructors())
            {
                List<object> parameters = new List<object>();
                foreach (var constructorParam in constructorInfo.GetParameters())
                {
                    if(constructorParam.ParameterType == typeof(DatabaseSetting))
                    {
                        parameters.Add(databaseSetting);
                    }
                    else
                    {
                        break;
                    }
                }

                if(parameters.Count == constructorInfo.GetParameters().Length)
                {
                    return (IMigrationStrategy) constructorInfo.Invoke(parameters.ToArray());
                }
            }

            throw new MissingMethodException("No suitable constructor found in "+type.Name);
        }
    }
}
