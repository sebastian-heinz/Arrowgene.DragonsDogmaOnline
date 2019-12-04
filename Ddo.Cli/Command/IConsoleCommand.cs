using Ddo.Cli.Argument;

namespace Ddo.Cli.Command
{
    public interface IConsoleCommand
    {
        CommandResultType Handle(ConsoleParameter parameter);        
        void Shutdown();
        string Key { get; }
        string Description { get; }
    }
}
