using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddo.GameServer.Logging;
using Arrowgene.Ddo.Shared;
using Arrowgene.Logging;

namespace Arrowgene.Ddo.GameServer.Network
{
    public class PacketFactory
    {
        private static readonly DdoLogger Logger = LogProvider.Logger<DdoLogger>(typeof(PacketFactory));

        public const int PacketLengthFieldSize = 2;
        public const int PacketHeaderSize = PacketLengthFieldSize;

        private bool _readHeader;
        private uint _dataSize;
        private int _position;
        private PacketId _packetId;
        private IBuffer _buffer;
        private readonly GameServerSetting _setting;

        public PacketFactory(GameServerSetting setting)
        {
            _setting = setting;
            Reset();
        }

        public byte[] Write(Packet packet)
        {
            byte[] data = packet.Data;
            IBuffer buffer = Util.Buffer.Provide();
            int dataLength = data.Length + PacketHeaderSize;
            if (dataLength < 0 || dataLength > ushort.MaxValue)
            {
                Logger.Error($"dataLength < 0 || dataLength > ushort.MaxValue (dataLength:{dataLength})");
            }
            
            buffer.WriteUInt16((ushort) data.Length /* without header*/, Endianness.Big);
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

                    // todo identify if packet contains ID and parse
                    _packetId = PacketId.ClientChallengeReq_C2L;

                    _readHeader = true;
                }

                if (_readHeader && _buffer.Size - _buffer.Position >= _dataSize)
                {
                    byte[] packetData = _buffer.ReadBytes((int) _dataSize);
                    Packet packet = new Packet(_packetId, packetData);
                    packets.Add(packet);
                    _readHeader = false;
                    _packetId = PacketId.Unknown;
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
            _packetId = PacketId.Unknown;
        }
    }
}
