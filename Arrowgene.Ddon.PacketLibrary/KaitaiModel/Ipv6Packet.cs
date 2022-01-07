// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;

namespace Arrowgene.Ddon.PacketLibrary.KaitaiModel
{
    public partial class Ipv6Packet : KaitaiStruct
    {
        public static Ipv6Packet FromFile(string fileName)
        {
            return new Ipv6Packet(new KaitaiStream(fileName));
        }

        public Ipv6Packet(KaitaiStream p__io, KaitaiStruct p__parent = null, Ipv6Packet p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _version = m_io.ReadBitsIntBe(4);
            _trafficClass = m_io.ReadBitsIntBe(8);
            _flowLabel = m_io.ReadBitsIntBe(20);
            m_io.AlignToByte();
            _payloadLength = m_io.ReadU2be();
            _nextHeaderType = m_io.ReadU1();
            _hopLimit = m_io.ReadU1();
            _srcIpv6Addr = m_io.ReadBytes(16);
            _dstIpv6Addr = m_io.ReadBytes(16);
            _nextHeader = new ProtocolBody(NextHeaderType, m_io);
            _rest = m_io.ReadBytesFull();
        }
        private ulong _version;
        private ulong _trafficClass;
        private ulong _flowLabel;
        private ushort _payloadLength;
        private byte _nextHeaderType;
        private byte _hopLimit;
        private byte[] _srcIpv6Addr;
        private byte[] _dstIpv6Addr;
        private ProtocolBody _nextHeader;
        private byte[] _rest;
        private Ipv6Packet m_root;
        private KaitaiStruct m_parent;
        public ulong Version { get { return _version; } }
        public ulong TrafficClass { get { return _trafficClass; } }
        public ulong FlowLabel { get { return _flowLabel; } }
        public ushort PayloadLength { get { return _payloadLength; } }
        public byte NextHeaderType { get { return _nextHeaderType; } }
        public byte HopLimit { get { return _hopLimit; } }
        public byte[] SrcIpv6Addr { get { return _srcIpv6Addr; } }
        public byte[] DstIpv6Addr { get { return _dstIpv6Addr; } }
        public ProtocolBody NextHeader { get { return _nextHeader; } }
        public byte[] Rest { get { return _rest; } }
        public Ipv6Packet M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
