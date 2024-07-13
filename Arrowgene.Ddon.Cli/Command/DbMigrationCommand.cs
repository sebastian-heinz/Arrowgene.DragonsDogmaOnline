using Arrowgene.Ddon.Database;
using Arrowgene.Ddon.Database.Migrations;
using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.Shared;
using Arrowgene.Logging;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using YamlDotNet.Serialization;

namespace Arrowgene.Ddon.Cli.Command
{
    public class DbMigrationCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ServerCommand));

        public string Key => "dbmigration";

        public string Description => $"Commandline tool to update the database\n\n" +
                                     $"usage:\n" +
                                     $"{Key} up\n" +
                                     $"{Key} down <version>";

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

            IDatabase database = DdonDatabaseBuilder.Build(dbSettings);
            if (database == null)
            {
                return CommandResultType.Exit;
            }


            using (var serviceProvider = database.CreateServices())
            using (var scope = serviceProvider.CreateScope())
            {
                if (parameter.Arguments.Contains("up"))
                {
                    DdonDatabaseBuilder.UpdateDatabase(scope.ServiceProvider);
                }
                else if (parameter.Arguments.Contains("down"))
                {
                    if (parameter.Arguments.Count < 2)
                    {
                        Logger.Error($"Expected 2 arguments but only found '{parameter.Arguments.Count}'.");
                        return CommandResultType.Exit;
                    }

                    if (!long.TryParse(parameter.Arguments[1], out long version))
                    {
                        Logger.Error($"Failed to parse the version value '{parameter.Arguments[1]}'.");
                        return CommandResultType.Exit;
                    }

                    // TODO: Warn that downgrade is destructive
                    DdonDatabaseBuilder.DowngradeDatabase(scope.ServiceProvider, version);
                }
            }

            return CommandResultType.Exit;
        }

        public void Shutdown()
        {
        }
    }
}
