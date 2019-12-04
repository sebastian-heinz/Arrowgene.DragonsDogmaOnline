namespace Ddo.Server.Packet
{
    public class PacketHeader
    {
        public PacketHeader(ushort id) : this(id, 0)
        {
        }

        public PacketHeader(ushort id, ushort dataSize)
        {
            Id = id;
            DataSize = dataSize;
        }

        public ushort Id { get; set; }
        public ushort DataSize { get; set; }


        public string ToLogText()
        {
            return $"[Id:0x{Id:X2}|DataSize:0x{DataSize:X2}]";
        }
    }
}
