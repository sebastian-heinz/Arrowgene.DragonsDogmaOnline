namespace Ddo.Server.Packet
{
    public interface IPacketDeserializer<T>
    {
        T Deserialize(DdoPacket packet);
    }
}
