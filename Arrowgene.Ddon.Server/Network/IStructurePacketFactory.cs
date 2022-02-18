namespace Arrowgene.Ddon.Server.Network
{
    public interface IStructurePacketFactory
    {
        StructurePacket Create(Packet packet);
    }
}
