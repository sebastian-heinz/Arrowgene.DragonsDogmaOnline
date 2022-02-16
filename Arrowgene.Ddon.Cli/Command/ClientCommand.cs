using System.IO;
using Arrowgene.Ddon.Client;
using Arrowgene.Ddon.Shared;

namespace Arrowgene.Ddon.Cli.Command
{
    public class ClientCommand : ICommand
    {
        public string Key => "client";

        public string Description => "Experiments with client resources";


        public CommandResultType Run(CommandParameter parameter)
        {
            string directoryPath = Path.Combine(Util.RelativeExecutingDirectory(), "Files/Client");
            ClientResourceRepository repo = new ClientResourceRepository(directoryPath);
            repo.Load();
            return CommandResultType.Continue;
        }

        public void Shutdown()
        {
        }
    }
}
