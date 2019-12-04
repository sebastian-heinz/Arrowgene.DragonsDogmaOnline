namespace Ddo.Server.Packet
{
    public interface IHandler
    {
        ushort Id { get; }
        int ExpectedSize { get; }
    }
}
