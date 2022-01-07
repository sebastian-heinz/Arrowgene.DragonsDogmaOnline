// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;
using System.Collections.Generic;

namespace Arrowgene.Ddon.PacketLibrary.KaitaiModel
{

    /// <summary>
    /// PPI is a standard for link layer packet encapsulation, proposed as
    /// generic extensible container to store both captured in-band data and
    /// out-of-band data. Originally it was developed to provide 802.11n
    /// radio information, but can be used for other purposes as well.
    /// 
    /// Sample capture: https://wiki.wireshark.org/SampleCaptures?action=AttachFile&amp;do=get&amp;target=Http.cap  
    /// </summary>
    /// <remarks>
    /// Reference: <a href="https://www.cacetech.com/documents/PPI_Header_format_1.0.1.pdf">PPI header format spec, section 3</a>
    /// </remarks>
    public partial class PacketPpi : KaitaiStruct
    {
        public static PacketPpi FromFile(string fileName)
        {
            return new PacketPpi(new KaitaiStream(fileName));
        }


        public enum PfhType
        {
            Radio80211Common = 2,
            Radio80211nMacExt = 3,
            Radio80211nMacPhyExt = 4,
            SpectrumMap = 5,
            ProcessInfo = 6,
            CaptureInfo = 7,
        }

        public enum Linktype
        {
            NullLinktype = 0,
            Ethernet = 1,
            Ax25 = 3,
            Ieee8025 = 6,
            ArcnetBsd = 7,
            Slip = 8,
            Ppp = 9,
            Fddi = 10,
            PppHdlc = 50,
            PppEther = 51,
            AtmRfc1483 = 100,
            Raw = 101,
            CHdlc = 104,
            Ieee80211 = 105,
            Frelay = 107,
            Loop = 108,
            LinuxSll = 113,
            Ltalk = 114,
            Pflog = 117,
            Ieee80211Prism = 119,
            IpOverFc = 122,
            Sunatm = 123,
            Ieee80211Radiotap = 127,
            ArcnetLinux = 129,
            AppleIpOverIeee1394 = 138,
            Mtp2WithPhdr = 139,
            Mtp2 = 140,
            Mtp3 = 141,
            Sccp = 142,
            Docsis = 143,
            LinuxIrda = 144,
            User0 = 147,
            User1 = 148,
            User2 = 149,
            User3 = 150,
            User4 = 151,
            User5 = 152,
            User6 = 153,
            User7 = 154,
            User8 = 155,
            User9 = 156,
            User10 = 157,
            User11 = 158,
            User12 = 159,
            User13 = 160,
            User14 = 161,
            User15 = 162,
            Ieee80211Avs = 163,
            BacnetMsTp = 165,
            PppPppd = 166,
            GprsLlc = 169,
            GpfT = 170,
            GpfF = 171,
            LinuxLapd = 177,
            BluetoothHciH4 = 187,
            UsbLinux = 189,
            Ppi = 192,
            Ieee802154 = 195,
            Sita = 196,
            Erf = 197,
            BluetoothHciH4WithPhdr = 201,
            Ax25Kiss = 202,
            Lapd = 203,
            PppWithDir = 204,
            CHdlcWithDir = 205,
            FrelayWithDir = 206,
            IpmbLinux = 209,
            Ieee802154NonaskPhy = 215,
            UsbLinuxMmapped = 220,
            Fc2 = 224,
            Fc2WithFrameDelims = 225,
            Ipnet = 226,
            CanSocketcan = 227,
            Ipv4 = 228,
            Ipv6 = 229,
            Ieee802154Nofcs = 230,
            Dbus = 231,
            DvbCi = 235,
            Mux27010 = 236,
            Stanag5066DPdu = 237,
            Nflog = 239,
            Netanalyzer = 240,
            NetanalyzerTransparent = 241,
            Ipoib = 242,
            Mpeg2Ts = 243,
            Ng40 = 244,
            NfcLlcp = 245,
            Infiniband = 247,
            Sctp = 248,
            Usbpcap = 249,
            RtacSerial = 250,
            BluetoothLeLl = 251,
            Netlink = 253,
            BluetoothLinuxMonitor = 254,
            BluetoothBredrBb = 255,
            BluetoothLeLlWithPhdr = 256,
            ProfibusDl = 257,
            Pktap = 258,
            Epon = 259,
            IpmiHpm2 = 260,
            ZwaveR1R2 = 261,
            ZwaveR3 = 262,
            WattstopperDlm = 263,
            Iso14443 = 264,
        }
        public PacketPpi(KaitaiStream p__io, KaitaiStruct p__parent = null, PacketPpi p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _header = new PacketPpiHeader(m_io, this, m_root);
            __raw_fields = m_io.ReadBytes((Header.PphLen - 8));
            var io___raw_fields = new KaitaiStream(__raw_fields);
            _fields = new PacketPpiFields(io___raw_fields, this, m_root);
            switch (Header.PphDlt) {
            case Linktype.Ppi: {
                __raw_body = m_io.ReadBytesFull();
                var io___raw_body = new KaitaiStream(__raw_body);
                _body = new PacketPpi(io___raw_body);
                break;
            }
            case Linktype.Ethernet: {
                __raw_body = m_io.ReadBytesFull();
                var io___raw_body = new KaitaiStream(__raw_body);
                _body = new EthernetFrame(io___raw_body);
                break;
            }
            default: {
                _body = m_io.ReadBytesFull();
                break;
            }
            }
        }
        public partial class PacketPpiFields : KaitaiStruct
        {
            public static PacketPpiFields FromFile(string fileName)
            {
                return new PacketPpiFields(new KaitaiStream(fileName));
            }

            public PacketPpiFields(KaitaiStream p__io, PacketPpi p__parent = null, PacketPpi p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _entries = new List<PacketPpiField>();
                {
                    var i = 0;
                    while (!m_io.IsEof) {
                        _entries.Add(new PacketPpiField(m_io, this, m_root));
                        i++;
                    }
                }
            }
            private List<PacketPpiField> _entries;
            private PacketPpi m_root;
            private PacketPpi m_parent;
            public List<PacketPpiField> Entries { get { return _entries; } }
            public PacketPpi M_Root { get { return m_root; } }
            public PacketPpi M_Parent { get { return m_parent; } }
        }

        /// <remarks>
        /// Reference: <a href="https://www.cacetech.com/documents/PPI_Header_format_1.0.1.pdf">PPI header format spec, section 4.1.3</a>
        /// </remarks>
        public partial class Radio80211nMacExtBody : KaitaiStruct
        {
            public static Radio80211nMacExtBody FromFile(string fileName)
            {
                return new Radio80211nMacExtBody(new KaitaiStream(fileName));
            }

            public Radio80211nMacExtBody(KaitaiStream p__io, PacketPpi.PacketPpiField p__parent = null, PacketPpi p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _flags = new MacFlags(m_io, this, m_root);
                _aMpduId = m_io.ReadU4le();
                _numDelimiters = m_io.ReadU1();
                _reserved = m_io.ReadBytes(3);
            }
            private MacFlags _flags;
            private uint _aMpduId;
            private byte _numDelimiters;
            private byte[] _reserved;
            private PacketPpi m_root;
            private PacketPpi.PacketPpiField m_parent;
            public MacFlags Flags { get { return _flags; } }
            public uint AMpduId { get { return _aMpduId; } }
            public byte NumDelimiters { get { return _numDelimiters; } }
            public byte[] Reserved { get { return _reserved; } }
            public PacketPpi M_Root { get { return m_root; } }
            public PacketPpi.PacketPpiField M_Parent { get { return m_parent; } }
        }
        public partial class MacFlags : KaitaiStruct
        {
            public static MacFlags FromFile(string fileName)
            {
                return new MacFlags(new KaitaiStream(fileName));
            }

            public MacFlags(KaitaiStream p__io, KaitaiStruct p__parent = null, PacketPpi p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unused1 = m_io.ReadBitsIntBe(1) != 0;
                _aggregateDelimiter = m_io.ReadBitsIntBe(1) != 0;
                _moreAggregates = m_io.ReadBitsIntBe(1) != 0;
                _aggregate = m_io.ReadBitsIntBe(1) != 0;
                _dupRx = m_io.ReadBitsIntBe(1) != 0;
                _rxShortGuard = m_io.ReadBitsIntBe(1) != 0;
                _isHt40 = m_io.ReadBitsIntBe(1) != 0;
                _greenfield = m_io.ReadBitsIntBe(1) != 0;
                m_io.AlignToByte();
                _unused2 = m_io.ReadBytes(3);
            }
            private bool _unused1;
            private bool _aggregateDelimiter;
            private bool _moreAggregates;
            private bool _aggregate;
            private bool _dupRx;
            private bool _rxShortGuard;
            private bool _isHt40;
            private bool _greenfield;
            private byte[] _unused2;
            private PacketPpi m_root;
            private KaitaiStruct m_parent;
            public bool Unused1 { get { return _unused1; } }

            /// <summary>
            /// Aggregate delimiter CRC error after this frame
            /// </summary>
            public bool AggregateDelimiter { get { return _aggregateDelimiter; } }

            /// <summary>
            /// More aggregates
            /// </summary>
            public bool MoreAggregates { get { return _moreAggregates; } }

            /// <summary>
            /// Aggregate
            /// </summary>
            public bool Aggregate { get { return _aggregate; } }

            /// <summary>
            /// Duplicate RX
            /// </summary>
            public bool DupRx { get { return _dupRx; } }

            /// <summary>
            /// RX short guard interval (SGI)
            /// </summary>
            public bool RxShortGuard { get { return _rxShortGuard; } }

            /// <summary>
            /// true = HT40, false = HT20
            /// </summary>
            public bool IsHt40 { get { return _isHt40; } }

            /// <summary>
            /// Greenfield
            /// </summary>
            public bool Greenfield { get { return _greenfield; } }
            public byte[] Unused2 { get { return _unused2; } }
            public PacketPpi M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }

        /// <remarks>
        /// Reference: <a href="https://www.cacetech.com/documents/PPI_Header_format_1.0.1.pdf">PPI header format spec, section 3.1</a>
        /// </remarks>
        public partial class PacketPpiHeader : KaitaiStruct
        {
            public static PacketPpiHeader FromFile(string fileName)
            {
                return new PacketPpiHeader(new KaitaiStream(fileName));
            }

            public PacketPpiHeader(KaitaiStream p__io, PacketPpi p__parent = null, PacketPpi p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _pphVersion = m_io.ReadU1();
                _pphFlags = m_io.ReadU1();
                _pphLen = m_io.ReadU2le();
                _pphDlt = ((PacketPpi.Linktype) m_io.ReadU4le());
            }
            private byte _pphVersion;
            private byte _pphFlags;
            private ushort _pphLen;
            private Linktype _pphDlt;
            private PacketPpi m_root;
            private PacketPpi m_parent;
            public byte PphVersion { get { return _pphVersion; } }
            public byte PphFlags { get { return _pphFlags; } }
            public ushort PphLen { get { return _pphLen; } }
            public Linktype PphDlt { get { return _pphDlt; } }
            public PacketPpi M_Root { get { return m_root; } }
            public PacketPpi M_Parent { get { return m_parent; } }
        }

        /// <remarks>
        /// Reference: <a href="https://www.cacetech.com/documents/PPI_Header_format_1.0.1.pdf">PPI header format spec, section 4.1.2</a>
        /// </remarks>
        public partial class Radio80211CommonBody : KaitaiStruct
        {
            public static Radio80211CommonBody FromFile(string fileName)
            {
                return new Radio80211CommonBody(new KaitaiStream(fileName));
            }

            public Radio80211CommonBody(KaitaiStream p__io, PacketPpi.PacketPpiField p__parent = null, PacketPpi p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _tsfTimer = m_io.ReadU8le();
                _flags = m_io.ReadU2le();
                _rate = m_io.ReadU2le();
                _channelFreq = m_io.ReadU2le();
                _channelFlags = m_io.ReadU2le();
                _fhssHopset = m_io.ReadU1();
                _fhssPattern = m_io.ReadU1();
                _dbmAntsignal = m_io.ReadS1();
                _dbmAntnoise = m_io.ReadS1();
            }
            private ulong _tsfTimer;
            private ushort _flags;
            private ushort _rate;
            private ushort _channelFreq;
            private ushort _channelFlags;
            private byte _fhssHopset;
            private byte _fhssPattern;
            private sbyte _dbmAntsignal;
            private sbyte _dbmAntnoise;
            private PacketPpi m_root;
            private PacketPpi.PacketPpiField m_parent;
            public ulong TsfTimer { get { return _tsfTimer; } }
            public ushort Flags { get { return _flags; } }
            public ushort Rate { get { return _rate; } }
            public ushort ChannelFreq { get { return _channelFreq; } }
            public ushort ChannelFlags { get { return _channelFlags; } }
            public byte FhssHopset { get { return _fhssHopset; } }
            public byte FhssPattern { get { return _fhssPattern; } }
            public sbyte DbmAntsignal { get { return _dbmAntsignal; } }
            public sbyte DbmAntnoise { get { return _dbmAntnoise; } }
            public PacketPpi M_Root { get { return m_root; } }
            public PacketPpi.PacketPpiField M_Parent { get { return m_parent; } }
        }

        /// <remarks>
        /// Reference: <a href="https://www.cacetech.com/documents/PPI_Header_format_1.0.1.pdf">PPI header format spec, section 3.1</a>
        /// </remarks>
        public partial class PacketPpiField : KaitaiStruct
        {
            public static PacketPpiField FromFile(string fileName)
            {
                return new PacketPpiField(new KaitaiStream(fileName));
            }

            public PacketPpiField(KaitaiStream p__io, PacketPpi.PacketPpiFields p__parent = null, PacketPpi p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _pfhType = ((PacketPpi.PfhType) m_io.ReadU2le());
                _pfhDatalen = m_io.ReadU2le();
                switch (PfhType) {
                case PacketPpi.PfhType.Radio80211Common: {
                    __raw_body = m_io.ReadBytes(PfhDatalen);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new Radio80211CommonBody(io___raw_body, this, m_root);
                    break;
                }
                case PacketPpi.PfhType.Radio80211nMacExt: {
                    __raw_body = m_io.ReadBytes(PfhDatalen);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new Radio80211nMacExtBody(io___raw_body, this, m_root);
                    break;
                }
                case PacketPpi.PfhType.Radio80211nMacPhyExt: {
                    __raw_body = m_io.ReadBytes(PfhDatalen);
                    var io___raw_body = new KaitaiStream(__raw_body);
                    _body = new Radio80211nMacPhyExtBody(io___raw_body, this, m_root);
                    break;
                }
                default: {
                    _body = m_io.ReadBytes(PfhDatalen);
                    break;
                }
                }
            }
            private PfhType _pfhType;
            private ushort _pfhDatalen;
            private object _body;
            private PacketPpi m_root;
            private PacketPpi.PacketPpiFields m_parent;
            private byte[] __raw_body;
            public PfhType PfhType { get { return _pfhType; } }
            public ushort PfhDatalen { get { return _pfhDatalen; } }
            public object Body { get { return _body; } }
            public PacketPpi M_Root { get { return m_root; } }
            public PacketPpi.PacketPpiFields M_Parent { get { return m_parent; } }
            public byte[] M_RawBody { get { return __raw_body; } }
        }

        /// <remarks>
        /// Reference: <a href="https://www.cacetech.com/documents/PPI_Header_format_1.0.1.pdf">PPI header format spec, section 4.1.4</a>
        /// </remarks>
        public partial class Radio80211nMacPhyExtBody : KaitaiStruct
        {
            public static Radio80211nMacPhyExtBody FromFile(string fileName)
            {
                return new Radio80211nMacPhyExtBody(new KaitaiStream(fileName));
            }

            public Radio80211nMacPhyExtBody(KaitaiStream p__io, PacketPpi.PacketPpiField p__parent = null, PacketPpi p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _flags = new MacFlags(m_io, this, m_root);
                _aMpduId = m_io.ReadU4le();
                _numDelimiters = m_io.ReadU1();
                _mcs = m_io.ReadU1();
                _numStreams = m_io.ReadU1();
                _rssiCombined = m_io.ReadU1();
                _rssiAntCtl = new List<byte>((int) (4));
                for (var i = 0; i < 4; i++)
                {
                    _rssiAntCtl.Add(m_io.ReadU1());
                }
                _rssiAntExt = new List<byte>((int) (4));
                for (var i = 0; i < 4; i++)
                {
                    _rssiAntExt.Add(m_io.ReadU1());
                }
                _extChannelFreq = m_io.ReadU2le();
                _extChannelFlags = new ChannelFlags(m_io, this, m_root);
                _rfSignalNoise = new List<SignalNoise>((int) (4));
                for (var i = 0; i < 4; i++)
                {
                    _rfSignalNoise.Add(new SignalNoise(m_io, this, m_root));
                }
                _evm = new List<uint>((int) (4));
                for (var i = 0; i < 4; i++)
                {
                    _evm.Add(m_io.ReadU4le());
                }
            }
            public partial class ChannelFlags : KaitaiStruct
            {
                public static ChannelFlags FromFile(string fileName)
                {
                    return new ChannelFlags(new KaitaiStream(fileName));
                }

                public ChannelFlags(KaitaiStream p__io, PacketPpi.Radio80211nMacPhyExtBody p__parent = null, PacketPpi p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _spectrum2ghz = m_io.ReadBitsIntBe(1) != 0;
                    _ofdm = m_io.ReadBitsIntBe(1) != 0;
                    _cck = m_io.ReadBitsIntBe(1) != 0;
                    _turbo = m_io.ReadBitsIntBe(1) != 0;
                    _unused = m_io.ReadBitsIntBe(8);
                    _gfsk = m_io.ReadBitsIntBe(1) != 0;
                    _dynCckOfdm = m_io.ReadBitsIntBe(1) != 0;
                    _onlyPassiveScan = m_io.ReadBitsIntBe(1) != 0;
                    _spectrum5ghz = m_io.ReadBitsIntBe(1) != 0;
                }
                private bool _spectrum2ghz;
                private bool _ofdm;
                private bool _cck;
                private bool _turbo;
                private ulong _unused;
                private bool _gfsk;
                private bool _dynCckOfdm;
                private bool _onlyPassiveScan;
                private bool _spectrum5ghz;
                private PacketPpi m_root;
                private PacketPpi.Radio80211nMacPhyExtBody m_parent;

                /// <summary>
                /// 2 GHz spectrum
                /// </summary>
                public bool Spectrum2ghz { get { return _spectrum2ghz; } }

                /// <summary>
                /// OFDM (Orthogonal Frequency-Division Multiplexing)
                /// </summary>
                public bool Ofdm { get { return _ofdm; } }

                /// <summary>
                /// CCK (Complementary Code Keying)
                /// </summary>
                public bool Cck { get { return _cck; } }
                public bool Turbo { get { return _turbo; } }
                public ulong Unused { get { return _unused; } }

                /// <summary>
                /// Gaussian Frequency Shift Keying
                /// </summary>
                public bool Gfsk { get { return _gfsk; } }

                /// <summary>
                /// Dynamic CCK-OFDM
                /// </summary>
                public bool DynCckOfdm { get { return _dynCckOfdm; } }

                /// <summary>
                /// Only passive scan allowed
                /// </summary>
                public bool OnlyPassiveScan { get { return _onlyPassiveScan; } }

                /// <summary>
                /// 5 GHz spectrum
                /// </summary>
                public bool Spectrum5ghz { get { return _spectrum5ghz; } }
                public PacketPpi M_Root { get { return m_root; } }
                public PacketPpi.Radio80211nMacPhyExtBody M_Parent { get { return m_parent; } }
            }

            /// <summary>
            /// RF signal + noise pair at a single antenna
            /// </summary>
            public partial class SignalNoise : KaitaiStruct
            {
                public static SignalNoise FromFile(string fileName)
                {
                    return new SignalNoise(new KaitaiStream(fileName));
                }

                public SignalNoise(KaitaiStream p__io, PacketPpi.Radio80211nMacPhyExtBody p__parent = null, PacketPpi p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _signal = m_io.ReadS1();
                    _noise = m_io.ReadS1();
                }
                private sbyte _signal;
                private sbyte _noise;
                private PacketPpi m_root;
                private PacketPpi.Radio80211nMacPhyExtBody m_parent;

                /// <summary>
                /// RF signal, dBm
                /// </summary>
                public sbyte Signal { get { return _signal; } }

                /// <summary>
                /// RF noise, dBm
                /// </summary>
                public sbyte Noise { get { return _noise; } }
                public PacketPpi M_Root { get { return m_root; } }
                public PacketPpi.Radio80211nMacPhyExtBody M_Parent { get { return m_parent; } }
            }
            private MacFlags _flags;
            private uint _aMpduId;
            private byte _numDelimiters;
            private byte _mcs;
            private byte _numStreams;
            private byte _rssiCombined;
            private List<byte> _rssiAntCtl;
            private List<byte> _rssiAntExt;
            private ushort _extChannelFreq;
            private ChannelFlags _extChannelFlags;
            private List<SignalNoise> _rfSignalNoise;
            private List<uint> _evm;
            private PacketPpi m_root;
            private PacketPpi.PacketPpiField m_parent;
            public MacFlags Flags { get { return _flags; } }
            public uint AMpduId { get { return _aMpduId; } }
            public byte NumDelimiters { get { return _numDelimiters; } }

            /// <summary>
            /// Modulation Coding Scheme (MCS)
            /// </summary>
            public byte Mcs { get { return _mcs; } }

            /// <summary>
            /// Number of spatial streams (0 = unknown)
            /// </summary>
            public byte NumStreams { get { return _numStreams; } }

            /// <summary>
            /// RSSI (Received Signal Strength Indication), combined from all active antennas / channels
            /// </summary>
            public byte RssiCombined { get { return _rssiCombined; } }

            /// <summary>
            /// RSSI (Received Signal Strength Indication) for antennas 0-3, control channel
            /// </summary>
            public List<byte> RssiAntCtl { get { return _rssiAntCtl; } }

            /// <summary>
            /// RSSI (Received Signal Strength Indication) for antennas 0-3, extension channel
            /// </summary>
            public List<byte> RssiAntExt { get { return _rssiAntExt; } }

            /// <summary>
            /// Extension channel frequency (MHz)
            /// </summary>
            public ushort ExtChannelFreq { get { return _extChannelFreq; } }

            /// <summary>
            /// Extension channel flags
            /// </summary>
            public ChannelFlags ExtChannelFlags { get { return _extChannelFlags; } }

            /// <summary>
            /// Signal + noise values for antennas 0-3
            /// </summary>
            public List<SignalNoise> RfSignalNoise { get { return _rfSignalNoise; } }

            /// <summary>
            /// EVM (Error Vector Magnitude) for chains 0-3
            /// </summary>
            public List<uint> Evm { get { return _evm; } }
            public PacketPpi M_Root { get { return m_root; } }
            public PacketPpi.PacketPpiField M_Parent { get { return m_parent; } }
        }
        private PacketPpiHeader _header;
        private PacketPpiFields _fields;
        private object _body;
        private PacketPpi m_root;
        private KaitaiStruct m_parent;
        private byte[] __raw_fields;
        private byte[] __raw_body;
        public PacketPpiHeader Header { get { return _header; } }
        public PacketPpiFields Fields { get { return _fields; } }
        public object Body { get { return _body; } }
        public PacketPpi M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
        public byte[] M_RawFields { get { return __raw_fields; } }
        public byte[] M_RawBody { get { return __raw_body; } }
    }
}
