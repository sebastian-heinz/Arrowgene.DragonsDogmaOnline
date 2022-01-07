// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;

namespace Arrowgene.Ddon.PacketLibrary.KaitaiModel
{
    public partial class IcmpPacket : KaitaiStruct
    {
        public static IcmpPacket FromFile(string fileName)
        {
            return new IcmpPacket(new KaitaiStream(fileName));
        }


        public enum IcmpTypeEnum
        {
            EchoReply = 0,
            DestinationUnreachable = 3,
            SourceQuench = 4,
            Redirect = 5,
            Echo = 8,
            TimeExceeded = 11,
        }
        public IcmpPacket(KaitaiStream p__io, KaitaiStruct p__parent = null, IcmpPacket p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _icmpType = ((IcmpTypeEnum) m_io.ReadU1());
            if (IcmpType == IcmpTypeEnum.DestinationUnreachable) {
                _destinationUnreachable = new DestinationUnreachableMsg(m_io, this, m_root);
            }
            if (IcmpType == IcmpTypeEnum.TimeExceeded) {
                _timeExceeded = new TimeExceededMsg(m_io, this, m_root);
            }
            if ( ((IcmpType == IcmpTypeEnum.Echo) || (IcmpType == IcmpTypeEnum.EchoReply)) ) {
                _echo = new EchoMsg(m_io, this, m_root);
            }
        }
        public partial class DestinationUnreachableMsg : KaitaiStruct
        {
            public static DestinationUnreachableMsg FromFile(string fileName)
            {
                return new DestinationUnreachableMsg(new KaitaiStream(fileName));
            }


            public enum DestinationUnreachableCode
            {
                NetUnreachable = 0,
                HostUnreachable = 1,
                ProtocolUnreachable = 2,
                PortUnreachable = 3,
                FragmentationNeededAndDfSet = 4,
                SourceRouteFailed = 5,
                DstNetUnkown = 6,
                SdtHostUnkown = 7,
                SrcIsolated = 8,
                NetProhibitedByAdmin = 9,
                HostProhibitedByAdmin = 10,
                NetUnreachableForTos = 11,
                HostUnreachableForTos = 12,
                CommunicationProhibitedByAdmin = 13,
                HostPrecedenceViolation = 14,
                PrecedenceCuttoffInEffect = 15,
            }
            public DestinationUnreachableMsg(KaitaiStream p__io, IcmpPacket p__parent = null, IcmpPacket p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _code = ((DestinationUnreachableCode) m_io.ReadU1());
                _checksum = m_io.ReadU2be();
            }
            private DestinationUnreachableCode _code;
            private ushort _checksum;
            private IcmpPacket m_root;
            private IcmpPacket m_parent;
            public DestinationUnreachableCode Code { get { return _code; } }
            public ushort Checksum { get { return _checksum; } }
            public IcmpPacket M_Root { get { return m_root; } }
            public IcmpPacket M_Parent { get { return m_parent; } }
        }
        public partial class TimeExceededMsg : KaitaiStruct
        {
            public static TimeExceededMsg FromFile(string fileName)
            {
                return new TimeExceededMsg(new KaitaiStream(fileName));
            }


            public enum TimeExceededCode
            {
                TimeToLiveExceededInTransit = 0,
                FragmentReassemblyTimeExceeded = 1,
            }
            public TimeExceededMsg(KaitaiStream p__io, IcmpPacket p__parent = null, IcmpPacket p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _code = ((TimeExceededCode) m_io.ReadU1());
                _checksum = m_io.ReadU2be();
            }
            private TimeExceededCode _code;
            private ushort _checksum;
            private IcmpPacket m_root;
            private IcmpPacket m_parent;
            public TimeExceededCode Code { get { return _code; } }
            public ushort Checksum { get { return _checksum; } }
            public IcmpPacket M_Root { get { return m_root; } }
            public IcmpPacket M_Parent { get { return m_parent; } }
        }
        public partial class EchoMsg : KaitaiStruct
        {
            public static EchoMsg FromFile(string fileName)
            {
                return new EchoMsg(new KaitaiStream(fileName));
            }

            public EchoMsg(KaitaiStream p__io, IcmpPacket p__parent = null, IcmpPacket p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _code = m_io.ReadBytes(1);
                if (!((KaitaiStream.ByteArrayCompare(Code, new byte[] { 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0 }, Code, M_Io, "/types/echo_msg/seq/0");
                }
                _checksum = m_io.ReadU2be();
                _identifier = m_io.ReadU2be();
                _seqNum = m_io.ReadU2be();
                _data = m_io.ReadBytesFull();
            }
            private byte[] _code;
            private ushort _checksum;
            private ushort _identifier;
            private ushort _seqNum;
            private byte[] _data;
            private IcmpPacket m_root;
            private IcmpPacket m_parent;
            public byte[] Code { get { return _code; } }
            public ushort Checksum { get { return _checksum; } }
            public ushort Identifier { get { return _identifier; } }
            public ushort SeqNum { get { return _seqNum; } }
            public byte[] Data { get { return _data; } }
            public IcmpPacket M_Root { get { return m_root; } }
            public IcmpPacket M_Parent { get { return m_parent; } }
        }
        private IcmpTypeEnum _icmpType;
        private DestinationUnreachableMsg _destinationUnreachable;
        private TimeExceededMsg _timeExceeded;
        private EchoMsg _echo;
        private IcmpPacket m_root;
        private KaitaiStruct m_parent;
        public IcmpTypeEnum IcmpType { get { return _icmpType; } }
        public DestinationUnreachableMsg DestinationUnreachable { get { return _destinationUnreachable; } }
        public TimeExceededMsg TimeExceeded { get { return _timeExceeded; } }
        public EchoMsg Echo { get { return _echo; } }
        public IcmpPacket M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
