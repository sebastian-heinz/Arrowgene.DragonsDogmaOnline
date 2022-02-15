using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Server.Network
{
    /// <summary>
    /// For typechecking to differentiate Packet from generic PacketStructure
    /// </summary>
    public interface IStructurePacket
    {
        string PrintStructure();
        PacketId Id { get; set; }
        byte[] Data { get; set; }
        PacketSource Source { get; set; }
        uint Count { get; set; }
        string PrintHeader();
        string PrintData();
    }
}
