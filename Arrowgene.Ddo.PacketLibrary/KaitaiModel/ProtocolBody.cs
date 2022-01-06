// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;

namespace Arrowgene.Ddo.PacketLibrary.KaitaiModel
{

    /// <summary>
    /// Protocol body represents particular payload on transport level (OSI
    /// layer 4).
    /// 
    /// Typically this payload in encapsulated into network level (OSI layer
    /// 3) packet, which includes &quot;protocol number&quot; field that would be used
    /// to decide what's inside the payload and how to parse it. Thanks to
    /// IANA's standardization effort, multiple network level use the same
    /// IDs for these payloads named &quot;protocol numbers&quot;.
    /// 
    /// This is effectively a &quot;router&quot; type: it expects to get protocol
    /// number as a parameter, and then invokes relevant type parser based
    /// on that parameter.
    /// </summary>
    /// <remarks>
    /// Reference: <a href="http://www.iana.org/assignments/protocol-numbers/protocol-numbers.xhtml">Source</a>
    /// </remarks>
    public partial class ProtocolBody : KaitaiStruct
    {

        public enum ProtocolEnum
        {
            Hopopt = 0,
            Icmp = 1,
            Igmp = 2,
            Ggp = 3,
            Ipv4 = 4,
            St = 5,
            Tcp = 6,
            Cbt = 7,
            Egp = 8,
            Igp = 9,
            BbnRccMon = 10,
            NvpIi = 11,
            Pup = 12,
            Argus = 13,
            Emcon = 14,
            Xnet = 15,
            Chaos = 16,
            Udp = 17,
            Mux = 18,
            DcnMeas = 19,
            Hmp = 20,
            Prm = 21,
            XnsIdp = 22,
            Trunk1 = 23,
            Trunk2 = 24,
            Leaf1 = 25,
            Leaf2 = 26,
            Rdp = 27,
            Irtp = 28,
            IsoTp4 = 29,
            Netblt = 30,
            MfeNsp = 31,
            MeritInp = 32,
            Dccp = 33,
            X3pc = 34,
            Idpr = 35,
            Xtp = 36,
            Ddp = 37,
            IdprCmtp = 38,
            TpPlusPlus = 39,
            Il = 40,
            Ipv6 = 41,
            Sdrp = 42,
            Ipv6Route = 43,
            Ipv6Frag = 44,
            Idrp = 45,
            Rsvp = 46,
            Gre = 47,
            Dsr = 48,
            Bna = 49,
            Esp = 50,
            Ah = 51,
            INlsp = 52,
            Swipe = 53,
            Narp = 54,
            Mobile = 55,
            Tlsp = 56,
            Skip = 57,
            Ipv6Icmp = 58,
            Ipv6Nonxt = 59,
            Ipv6Opts = 60,
            AnyHostInternalProtocol = 61,
            Cftp = 62,
            AnyLocalNetwork = 63,
            SatExpak = 64,
            Kryptolan = 65,
            Rvd = 66,
            Ippc = 67,
            AnyDistributedFileSystem = 68,
            SatMon = 69,
            Visa = 70,
            Ipcv = 71,
            Cpnx = 72,
            Cphb = 73,
            Wsn = 74,
            Pvp = 75,
            BrSatMon = 76,
            SunNd = 77,
            WbMon = 78,
            WbExpak = 79,
            IsoIp = 80,
            Vmtp = 81,
            SecureVmtp = 82,
            Vines = 83,
            TtpOrIptm = 84,
            NsfnetIgp = 85,
            Dgp = 86,
            Tcf = 87,
            Eigrp = 88,
            Ospfigp = 89,
            SpriteRpc = 90,
            Larp = 91,
            Mtp = 92,
            Ax25 = 93,
            Ipip = 94,
            Micp = 95,
            SccSp = 96,
            Etherip = 97,
            Encap = 98,
            AnyPrivateEncryptionScheme = 99,
            Gmtp = 100,
            Ifmp = 101,
            Pnni = 102,
            Pim = 103,
            Aris = 104,
            Scps = 105,
            Qnx = 106,
            AN = 107,
            Ipcomp = 108,
            Snp = 109,
            CompaqPeer = 110,
            IpxInIp = 111,
            Vrrp = 112,
            Pgm = 113,
            Any0Hop = 114,
            L2tp = 115,
            Ddx = 116,
            Iatp = 117,
            Stp = 118,
            Srp = 119,
            Uti = 120,
            Smp = 121,
            Sm = 122,
            Ptp = 123,
            IsisOverIpv4 = 124,
            Fire = 125,
            Crtp = 126,
            Crudp = 127,
            Sscopmce = 128,
            Iplt = 129,
            Sps = 130,
            Pipe = 131,
            Sctp = 132,
            Fc = 133,
            RsvpE2eIgnore = 134,
            MobilityHeader = 135,
            Udplite = 136,
            MplsInIp = 137,
            Manet = 138,
            Hip = 139,
            Shim6 = 140,
            Wesp = 141,
            Rohc = 142,
            Reserved255 = 255,
        }
        public ProtocolBody(byte p_protocolNum, KaitaiStream p__io, KaitaiStruct p__parent = null, ProtocolBody p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _protocolNum = p_protocolNum;
            f_protocol = false;
            _read();
        }
        private void _read()
        {
            switch (Protocol) {
            case ProtocolEnum.Ipv6Nonxt: {
                _body = new NoNextHeader(m_io, this, m_root);
                break;
            }
            case ProtocolEnum.Ipv4: {
                _body = new Ipv4Packet(m_io);
                break;
            }
            case ProtocolEnum.Udp: {
                _body = new UdpDatagram(m_io);
                break;
            }
            case ProtocolEnum.Icmp: {
                _body = new IcmpPacket(m_io);
                break;
            }
            case ProtocolEnum.Hopopt: {
                _body = new OptionHopByHop(m_io, this, m_root);
                break;
            }
            case ProtocolEnum.Ipv6: {
                _body = new Ipv6Packet(m_io);
                break;
            }
            case ProtocolEnum.Tcp: {
                _body = new TcpSegment(m_io);
                break;
            }
            }
        }

        /// <summary>
        /// Dummy type for IPv6 &quot;no next header&quot; type, which signifies end of headers chain.
        /// </summary>
        public partial class NoNextHeader : KaitaiStruct
        {
            public static NoNextHeader FromFile(string fileName)
            {
                return new NoNextHeader(new KaitaiStream(fileName));
            }

            public NoNextHeader(KaitaiStream p__io, ProtocolBody p__parent = null, ProtocolBody p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
            }
            private ProtocolBody m_root;
            private ProtocolBody m_parent;
            public ProtocolBody M_Root { get { return m_root; } }
            public ProtocolBody M_Parent { get { return m_parent; } }
        }
        public partial class OptionHopByHop : KaitaiStruct
        {
            public static OptionHopByHop FromFile(string fileName)
            {
                return new OptionHopByHop(new KaitaiStream(fileName));
            }

            public OptionHopByHop(KaitaiStream p__io, ProtocolBody p__parent = null, ProtocolBody p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _nextHeaderType = m_io.ReadU1();
                _hdrExtLen = m_io.ReadU1();
                _body = m_io.ReadBytes((HdrExtLen - 1));
                _nextHeader = new ProtocolBody(NextHeaderType, m_io);
            }
            private byte _nextHeaderType;
            private byte _hdrExtLen;
            private byte[] _body;
            private ProtocolBody _nextHeader;
            private ProtocolBody m_root;
            private ProtocolBody m_parent;
            public byte NextHeaderType { get { return _nextHeaderType; } }
            public byte HdrExtLen { get { return _hdrExtLen; } }
            public byte[] Body { get { return _body; } }
            public ProtocolBody NextHeader { get { return _nextHeader; } }
            public ProtocolBody M_Root { get { return m_root; } }
            public ProtocolBody M_Parent { get { return m_parent; } }
        }
        private bool f_protocol;
        private ProtocolEnum _protocol;
        public ProtocolEnum Protocol
        {
            get
            {
                if (f_protocol)
                    return _protocol;
                _protocol = (ProtocolEnum) (((ProtocolEnum) ProtocolNum));
                f_protocol = true;
                return _protocol;
            }
        }
        private KaitaiStruct _body;
        private byte _protocolNum;
        private ProtocolBody m_root;
        private KaitaiStruct m_parent;
        public KaitaiStruct Body { get { return _body; } }

        /// <summary>
        /// Protocol number as an integer.
        /// </summary>
        public byte ProtocolNum { get { return _protocolNum; } }
        public ProtocolBody M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
