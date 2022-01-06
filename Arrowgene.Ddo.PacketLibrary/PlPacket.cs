using System;

namespace Arrowgene.Ddo.PacketLibrary
{
    public class PlPacket
    {
        private uint _tsSec;
        
        public TcpFlag Flag { get; set; }
        public byte[] Data { get; set; }
        public ushort SrcPort { get; set; }
        public ushort DstPort { get; set; }
        public uint InitialTsSec  { get; set; }
        public uint InitialTsUsec  { get; set; }
        public uint TsUsec  { get; set; }
        public uint PacketNum  { get; set; }

        public string RelativeTs => $"{TsSec - InitialTsSec}.{TsUsec - InitialTsUsec}";
        
        public uint TsSec
        {
            get { return _tsSec;}
            set
            {
                _tsSec = value;
                DateTime = DateTimeOffset.FromUnixTimeSeconds(value);
            }
        }
        
        public DateTimeOffset DateTime  { get; set; }
    }
}
