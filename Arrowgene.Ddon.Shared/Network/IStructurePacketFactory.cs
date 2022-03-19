namespace Arrowgene.Ddon.Shared.Network
{
    public interface IStructurePacketFactory
    {
        public IStructurePacket Create(Packet packet);
    }
}
