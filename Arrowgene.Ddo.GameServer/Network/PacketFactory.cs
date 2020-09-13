using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.GameServer.Network
{
    public class PacketFactory
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(PacketFactory));

        public const int PacketLengthFieldSize = 2;
      //  public const int PacketIdFieldSize = 2;
        public const int PacketHeaderSize = PacketLengthFieldSize;

        private bool _readHeader;
        private uint _dataSize;
        private int _position;
        private IBuffer _buffer;
        private readonly GameServerSetting _setting;

        public PacketFactory(GameServerSetting setting)
        {
            _setting = setting;
            Reset();
        }

        public byte[] Write(Packet packet)
        {
            byte[] data = packet.Data.GetAllBytes();
            IBuffer buffer = Util.Buffer.Provide();
            int dataLength = data.Length + PacketHeaderSize;
            if (dataLength < 0 || dataLength > ushort.MaxValue)
            {
                Logger.Error($"dataLength < 0 || dataLength > ushort.MaxValue (dataLength:{dataLength})");
            }

            buffer.WriteUInt16((ushort) dataLength, Endianness.Big);
            buffer.WriteBytes(data);
            return buffer.GetAllBytes();
        }

        public List<Packet> Read(byte[] data)
        {
            List<Packet> packets = new List<Packet>();
            if (_buffer == null)
            {
                _buffer = Util.Buffer.Provide(data);
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
                if (!_readHeader && _buffer.Size - _buffer.Position >= PacketHeaderSize)
                {
                    _dataSize = _buffer.ReadUInt16(Endianness.Big);
                    if (_dataSize < PacketHeaderSize)
                    {
                        Logger.Error($"DataSize:{_dataSize} < PacketHeaderSize:{PacketHeaderSize}");
                        Reset();
                        return packets;
                    }

                    if (_dataSize > int.MaxValue)
                    {
                        Logger.Error($"DataSize:{_dataSize} < int.MaxValue:{int.MaxValue} - not supported");
                        Reset();
                        return packets;
                    }

                    _dataSize -= PacketHeaderSize;
                    _readHeader = true;
                }

                if (_readHeader && _buffer.Size - _buffer.Position >= _dataSize)
                {
                    byte[] packetData = _buffer.ReadBytes((int) _dataSize);
                    IBuffer buffer = Util.Buffer.Provide(packetData);
                    Packet packet = new Packet(buffer);
                    packets.Add(packet);
                    _readHeader = false;
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
            _readHeader = false;
            _dataSize = 0;
            _position = 0;
            _buffer = null;
        }
    }
}
