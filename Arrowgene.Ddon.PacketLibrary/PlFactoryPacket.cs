using System;

namespace Arrowgene.Ddon.PacketLibrary
{
    public class PlFactoryPacket
    {
        private uint _tsSec;
        public byte[] Data { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public TcpFlag Flag { get; set; }
        public ushort SrcPort { get; set; }
        public ushort DstPort { get; set; }
        public uint InitialTsSec { get; set; }
        public uint InitialTsUsec { get; set; }
        public uint TsUsec { get; set; }
        public uint PacketNum { get; set; }

        public string RelativeTs => GetRelativeTs();

        public string GetDirection()
        {
            if (SrcPort == 52000 || SrcPort == 52100)
            {
                return "S2C";
            }

            if (DstPort == 52000 || SrcPort == DstPort)
            {
                return "C2S";
            }

            return "UNK";
        }

        public string GetRelativeTs()
        {
            uint s;
            uint us;
            if (InitialTsUsec <= TsUsec)
            {
                s = TsSec - InitialTsSec;
                us = TsUsec - InitialTsUsec;
            }
            else
            {
                s = TsSec - InitialTsSec - 1;
                us = (1000000 - InitialTsUsec) + TsUsec;
            }

            return $"{s}.{us}";
        }

        public uint TsSec
        {
            get { return _tsSec; }
            set
            {
                _tsSec = value;
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(value);
            }
        }
    }
}
