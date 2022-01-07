namespace Arrowgene.Ddon.Cli
{
    public interface ICommand
    {
        string Key { get; }
        string Description { get; }
        CommandResultType Run(CommandParameter parameter);
        void Shutdown();
    }
}
