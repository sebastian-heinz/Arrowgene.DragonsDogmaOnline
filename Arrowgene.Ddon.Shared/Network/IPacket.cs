using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Network
{
    public interface IPacket
    {
        PacketId Id { get; set; }
        byte[] Data { get; set; }
        PacketSource Source { get; set; }
        uint Count { get; set; }
        string PrintHeader();
        string PrintData();
        IBuffer AsBuffer();
        public byte[] GetHeaderBytes();
        public string PrintHeaderBytes();
    }
}
