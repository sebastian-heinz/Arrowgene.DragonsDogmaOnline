using System.IO;
using Arrowgene.Ddon.Client;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ClientCommand : ICommand
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(ClientCommand));

        public string Key => "client";

        public string Description => "Usage: `client \"E:\\Games\\Dragon's Dogma Online\\nativePC\\rom\"`";


        public CommandResultType Run(CommandParameter parameter)
        {
            if (parameter.Arguments.Count < 1)
            {
                Logger.Error($"To few arguments. {Description}");
                return CommandResultType.Exit;
            }

            string romDirectory = parameter.Arguments[0];
            if (!Directory.Exists(romDirectory))
            {
                Logger.Error("Rom Path Invalid");
                return CommandResultType.Exit;
            }

            ClientResourceRepository repo = new ClientResourceRepository();
            repo.Load(romDirectory);
            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
        }
    }
}
