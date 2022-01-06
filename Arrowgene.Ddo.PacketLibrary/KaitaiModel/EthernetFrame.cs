// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;

namespace Arrowgene.Ddo.PacketLibrary.KaitaiModel
{

    /// <summary>
    /// Ethernet frame is a OSI data link layer (layer 2) protocol data unit
    /// for Ethernet networks. In practice, many other networks and/or
    /// in-file dumps adopted the same format for encapsulation purposes.
    /// </summary>
    /// <remarks>
    /// Reference: <a href="https://ieeexplore.ieee.org/document/7428776">Source</a>
    /// </remarks>
    public partial class EthernetFrame : KaitaiStruct
    {
        public static EthernetFrame FromFile(string fileName)
        {
            return new EthernetFrame(new KaitaiStream(fileName));
        }


        public enum EtherTypeEnum
        {
            Ipv4 = 2048,
            X75Internet = 2049,
            NbsInternet = 2050,
            EcmaInternet = 2051,
            Chaosnet = 2052,
            X25Level3 = 2053,
            Arp = 2054,
            Ieee8021qTpid = 33024,
            Ipv6 = 34525,
        }
        public EthernetFrame(KaitaiStream p__io, KaitaiStruct p__parent = null, EthernetFrame p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            f_etherType = false;
            _read();
        }
        private void _read()
        {
            _dstMac = m_io.ReadBytes(6);
            _srcMac = m_io.ReadBytes(6);
            _etherType1 = ((EtherTypeEnum) m_io.ReadU2be());
            if (EtherType1 == EtherTypeEnum.Ieee8021qTpid) {
                _tci = new TagControlInfo(m_io, this, m_root);
            }
            if (EtherType1 == EtherTypeEnum.Ieee8021qTpid) {
                _etherType2 = ((EtherTypeEnum) m_io.ReadU2be());
            }
            switch (EtherType) {
            case EtherTypeEnum.Ipv4: {
                __raw_body = m_io.ReadBytesFull();
                var io___raw_body = new KaitaiStream(__raw_body);
                _body = new Ipv4Packet(io___raw_body);
                break;
            }
            case EtherTypeEnum.Ipv6: {
                __raw_body = m_io.ReadBytesFull();
                var io___raw_body = new KaitaiStream(__raw_body);
                _body = new Ipv6Packet(io___raw_body);
                break;
            }
            default: {
                _body = m_io.ReadBytesFull();
                break;
            }
            }
        }

        /// <summary>
        /// Tag Control Information (TCI) is an extension of IEEE 802.1Q to
        /// support VLANs on normal IEEE 802.3 Ethernet network.
        /// </summary>
        public partial class TagControlInfo : KaitaiStruct
        {
            public static TagControlInfo FromFile(string fileName)
            {
                return new TagControlInfo(new KaitaiStream(fileName));
            }

            public TagControlInfo(KaitaiStream p__io, EthernetFrame p__parent = null, EthernetFrame p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _priority = m_io.ReadBitsIntBe(3);
                _dropEligible = m_io.ReadBitsIntBe(1) != 0;
                _vlanId = m_io.ReadBitsIntBe(12);
            }
            private ulong _priority;
            private bool _dropEligible;
            private ulong _vlanId;
            private EthernetFrame m_root;
            private EthernetFrame m_parent;

            /// <summary>
            /// Priority Code Point (PCP) is used to specify priority for
            /// different kinds of traffic.
            /// </summary>
            public ulong Priority { get { return _priority; } }

            /// <summary>
            /// Drop Eligible Indicator (DEI) specifies if frame is eligible
            /// to dropping while congestion is detected for certain classes
            /// of traffic.
            /// </summary>
            public bool DropEligible { get { return _dropEligible; } }

            /// <summary>
            /// VLAN Identifier (VID) specifies which VLAN this frame
            /// belongs to.
            /// </summary>
            public ulong VlanId { get { return _vlanId; } }
            public EthernetFrame M_Root { get { return m_root; } }
            public EthernetFrame M_Parent { get { return m_parent; } }
        }
        private bool f_etherType;
        private EtherTypeEnum _etherType;

        /// <summary>
        /// Ether type can be specied in several places in the frame. If
        /// first location bears special marker (0x8100), then it is not the
        /// real ether frame yet, an additional payload (`tci`) is expected
        /// and real ether type is upcoming next.
        /// </summary>
        public EtherTypeEnum EtherType
        {
            get
            {
                if (f_etherType)
                    return _etherType;
                _etherType = (EtherTypeEnum) ((EtherType1 == EtherTypeEnum.Ieee8021qTpid ? EtherType2 : EtherType1));
                f_etherType = true;
                return _etherType;
            }
        }
        private byte[] _dstMac;
        private byte[] _srcMac;
        private EtherTypeEnum _etherType1;
        private TagControlInfo _tci;
        private EtherTypeEnum _etherType2;
        private object _body;
        private EthernetFrame m_root;
        private KaitaiStruct m_parent;
        private byte[] __raw_body;

        /// <summary>
        /// Destination MAC address
        /// </summary>
        public byte[] DstMac { get { return _dstMac; } }

        /// <summary>
        /// Source MAC address
        /// </summary>
        public byte[] SrcMac { get { return _srcMac; } }

        /// <summary>
        /// Either ether type or TPID if it is a IEEE 802.1Q frame
        /// </summary>
        public EtherTypeEnum EtherType1 { get { return _etherType1; } }
        public TagControlInfo Tci { get { return _tci; } }
        public EtherTypeEnum EtherType2 { get { return _etherType2; } }
        public object Body { get { return _body; } }
        public EthernetFrame M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
        public byte[] M_RawBody { get { return __raw_body; } }
    }
}
