namespace Ddo.Cli.Argument
{
    public interface ISwitchProperty
    {
        string Key { get; }
        string Description { get; }
        string ValueDescription { get; }
        bool Assign(string value);
    }
}
