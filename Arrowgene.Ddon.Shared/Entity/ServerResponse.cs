using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity
{
    public abstract class ServerResponse : IPacketStructure
    {
        public uint Error { get; set; }
        public uint Result { get; set; }
        public abstract PacketId Id { get; }
    }
}
