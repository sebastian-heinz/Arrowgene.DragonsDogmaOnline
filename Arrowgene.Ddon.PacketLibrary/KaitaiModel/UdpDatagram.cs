// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using Kaitai;

namespace Arrowgene.Ddon.PacketLibrary.KaitaiModel
{

    /// <summary>
    /// UDP is a simple stateless transport layer (AKA OSI layer 4)
    /// protocol, one of the core Internet protocols. It provides source and
    /// destination ports, basic checksumming, but provides not guarantees
    /// of delivery, order of packets, or duplicate delivery.
    /// </summary>
    public partial class UdpDatagram : KaitaiStruct
    {
        public static UdpDatagram FromFile(string fileName)
        {
            return new UdpDatagram(new KaitaiStream(fileName));
        }

        public UdpDatagram(KaitaiStream p__io, KaitaiStruct p__parent = null, UdpDatagram p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _srcPort = m_io.ReadU2be();
            _dstPort = m_io.ReadU2be();
            _length = m_io.ReadU2be();
            _checksum = m_io.ReadU2be();
            _body = m_io.ReadBytes((Length - 8));
        }
        private ushort _srcPort;
        private ushort _dstPort;
        private ushort _length;
        private ushort _checksum;
        private byte[] _body;
        private UdpDatagram m_root;
        private KaitaiStruct m_parent;
        public ushort SrcPort { get { return _srcPort; } }
        public ushort DstPort { get { return _dstPort; } }
        public ushort Length { get { return _length; } }
        public ushort Checksum { get { return _checksum; } }
        public byte[] Body { get { return _body; } }
        public UdpDatagram M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
