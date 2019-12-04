namespace Ddo.Server.Packet
{
    public interface IPacketSerializer<T>
    {
        DdoPacket Serialize(T obj);
    }
}
