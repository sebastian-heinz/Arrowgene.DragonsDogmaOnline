using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity
{
    /// <summary>
    /// PacketStructure is defined as the internal structure of a packet
    /// </summary>
    public interface IPacketStructure
    {
        public PacketId Id { get; }
    }
}
