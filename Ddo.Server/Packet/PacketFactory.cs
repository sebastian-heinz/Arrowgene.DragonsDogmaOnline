using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Ddo.Server.Common;
using Ddo.Server.Logging;
using Ddo.Server.Setting;

namespace Ddo.Server.Packet
{
    public class PacketFactory
    {
        public const int PacketHeaderSize = 14;

        private readonly DdoLogger _logger;
        private readonly DdoSetting _setting;

        private int _position;
        private IBuffer _buffer;
        private PacketHeader _header;


        public PacketFactory(DdoSetting setting)
        {
            _logger = LogProvider.Logger<DdoLogger>(this);
            _setting = setting;
            _header = null;
            Reset();
        }

        public byte[] Write(DdoPacket packet)
        {
            if (packet.Data.Size > ushort.MaxValue)
            {
                _logger.Error(
                    $"Packet Size: {packet.Data.Size} exceeds maximum size of {ushort.MaxValue} for PacketId: {packet.Id}");
                return null;
            }

            byte[] data = packet.Data.GetAllBytes();
            packet.Header.DataSize = (ushort) data.Length;
            IBuffer buffer = BufferProvider.Provide();
            buffer.WriteInt16(packet.Header.Id, Endianness.Big);
            buffer.WriteInt16(packet.Header.DataSize, Endianness.Big);
            buffer.WriteBytes(data);
            byte[] final = buffer.GetAllBytes();
            return final;
        }

        public List<DdoPacket> Read(byte[] data)
        {
            List<DdoPacket> packets = new List<DdoPacket>();
            if (_buffer == null)
            {
                _buffer = BufferProvider.Provide(data);
            }
            else
            {
                _buffer.SetPositionEnd();
                _buffer.WriteBytes(data);
            }

            _buffer.Position = _position;

            bool read = true;
            while (read)
            {
                read = false;

                if (_header == null && _buffer.Size - _buffer.Position >= PacketHeaderSize)
                {
                    ushort dataSize = _buffer.ReadUInt16(Endianness.Big);
                    _header = new PacketHeader(0, dataSize);
                }

                if (_header != null && _buffer.Size - _buffer.Position >= _header.DataSize)
                {
                    byte[] packetData = _buffer.ReadBytes(_header.DataSize);
                    IBuffer buffer = BufferProvider.Provide(packetData);
                    DdoPacket packet = new DdoPacket(_header, buffer);
                    packets.Add(packet);
                    _header = null;
                    read = _buffer.Position != _buffer.Size;
                }
            }

            if (_buffer.Position == _buffer.Size)
            {
                Reset();
            }
            else
            {
                _position = _buffer.Position;
            }

            return packets;
        }

        private void Reset()
        {
            _header = null;
            _position = 0;
            _buffer = null;
        }
    }
}
