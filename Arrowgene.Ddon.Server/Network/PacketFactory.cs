using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Server.Network
{
    public class PacketFactory
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(PacketFactory));
        private static readonly Camellia _camellia = new Camellia();

        private static readonly byte[] CamelliaKey = new byte[]
        {
            0x66, 0x32, 0x33, 0x65, 0x39, 0x38, 0x48, 0x61, 0x66, 0x4A, 0x64, 0x53, 0x6F, 0x61, 0x6A, 0x38,
            0x30, 0x51, 0x42, 0x6A, 0x68, 0x68, 0x32, 0x33, 0x6F, 0x61, 0x6A, 0x67, 0x6B, 0x6C, 0x53, 0x61,
        };

        private static readonly byte[] CamelliaIv = new byte[]
        {
            0x24, 0x63, 0x62, 0x4D, 0x36, 0x57, 0x50, 0x29, 0x61, 0x58, 0x3D, 0x25, 0x4A, 0x5E, 0x7A, 0x41
        };

        private const int PacketLengthFieldSize = 2;
        private const int PacketHeaderSize = 9;

        private bool _readHeader;
        private uint _dataSize;
        private uint _packetCount;
        private int _position;
        private IBuffer _buffer;
        private readonly ServerSetting _setting;
        private byte[] _camelliaKey;
        private IPacketIdResolver _packetIdResolver;


        public PacketFactory(ServerSetting setting, IPacketIdResolver packetIdResolver)
        {
            _setting = setting;
            _camelliaKey = CamelliaKey;
            _packetCount = 0;
            _packetIdResolver = packetIdResolver;
            Reset();
        }

        public void SetCamelliaKey(byte[] camelliaKey)
        {
            _camelliaKey = Copy(camelliaKey);
        }

        public byte[] WriteWithoutHeader(Packet packet)
        {
            byte[] data = packet.Data;
            int totalLength = data.Length + PacketLengthFieldSize;
            if (totalLength < 0 || totalLength > ushort.MaxValue)
            {
                Logger.Error($"dataLength < 0 || dataLength > ushort.MaxValue (dataLength:{totalLength})");
            }

            byte[] encryptedPacketData = Encrypt(packet.Data);

            IBuffer buffer = Util.Buffer.Provide();
            buffer.WriteUInt16((ushort) encryptedPacketData.Length /* without header*/, Endianness.Big);
            buffer.WriteBytes(encryptedPacketData);
            return buffer.GetAllBytes();
        }

        public byte[] Write(Packet packet)
        {
            byte[] data = packet.Data;
            if (data == null)
            {
                Logger.Error($"data == null, tried to write invalid data");
                return null;
            }
            int totalLength = data.Length + PacketLengthFieldSize + PacketHeaderSize;
            if (totalLength < 0 || totalLength > ushort.MaxValue)
            {
                Logger.Error($"dataLength < 0 || dataLength > ushort.MaxValue (dataLength:{totalLength})");
            }

            IBuffer packetDataBuffer = Util.Buffer.Provide();
            packetDataBuffer.WriteByte(packet.Id.GroupId);
            packetDataBuffer.WriteUInt16(packet.Id.HandlerId,Endianness.Big);
            packetDataBuffer.WriteByte(packet.Id.HandlerSubId);
            packetDataBuffer.WriteByte(0x34);
            packetDataBuffer.WriteUInt32(_packetCount, Endianness.Big);
            packetDataBuffer.WriteBytes(data);
            
            byte[] packetData = packetDataBuffer.GetAllBytes();
            byte[] encryptedPacketData = Encrypt(packetData);
            
            IBuffer buffer = Util.Buffer.Provide();
            buffer.WriteUInt16((ushort) encryptedPacketData.Length /* without header*/, Endianness.Big);
            buffer.WriteBytes(encryptedPacketData);
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
                if (!_readHeader && _buffer.Size - _buffer.Position >= PacketLengthFieldSize)
                {
                    _dataSize = _buffer.ReadUInt16(Endianness.Big);
                    if (_dataSize < PacketLengthFieldSize)
                    {
                        Logger.Error($"DataSize:{_dataSize} < PacketLengthFieldSize:{PacketLengthFieldSize}");
                        Reset();
                        return packets;
                    }

                    if (_dataSize > int.MaxValue)
                    {
                        Logger.Error($"DataSize:{_dataSize} < int.MaxValue:{int.MaxValue} - not supported");
                        Reset();
                        return packets;
                    }

                    _readHeader = true;
                }

                if (_readHeader && _buffer.Size - _buffer.Position >= _dataSize)
                {
                    int dataSize = (int) _dataSize;
                    if (dataSize < PacketHeaderSize)
                    {
                        Logger.Error($"DataSize:{dataSize} < PacketHeaderSize:{PacketHeaderSize}");
                        Reset();
                        return packets;
                    }

                    byte[] encryptedPacketData = _buffer.ReadBytes(dataSize);
                    byte[] packetData = Decrypt(encryptedPacketData);
                    IBuffer packetBuffer = new StreamBuffer(packetData);
                    packetBuffer.SetPositionStart();
                    byte groupId = packetBuffer.ReadByte();
                    ushort handlerId = packetBuffer.ReadUInt16(Endianness.Big);
                    byte handlerSubId = packetBuffer.ReadByte();
                    byte unknownA = packetBuffer.ReadByte();
                    uint packetCount = packetBuffer.ReadUInt32(Endianness.Big);
                    
                    byte[] payload;
                    PacketId packetId;
                    
                    // abusing this as part of header validation
                    if (unknownA != 0 /*reading client packet*/ 
                        && unknownA != 0x34 /*reading server packet*/)
                    {
                        payload = packetData;
                        packetId = PacketId.C2L_CLIENT_CHALLENGE_REQ;
                    }
                    else
                    {
                        payload = packetBuffer.ReadBytes(packetBuffer.Size - packetBuffer.Position);
                        packetId = _packetIdResolver.Get(groupId, handlerId, handlerSubId);
                    }
                    Packet packet = new Packet(packetId, payload, PacketSource.Client, packetCount);
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

        public byte[] Encrypt(byte[] data)
        {
            _camellia.Encrypt(data, out Span<byte> encrypted, _camelliaKey, Copy(CamelliaIv));
            return encrypted.ToArray();
        }

        public byte[] Decrypt(byte[] encrypted)
        {
            _camellia.Decrypt(encrypted, out Span<byte> decrypted, _camelliaKey, Copy(CamelliaIv));
            return decrypted.ToArray();
        }

        private byte[] Copy(byte[] src)
        {
            int srcLen = src.Length;
            byte[] dst = new byte[srcLen];
            System.Buffer.BlockCopy(src, 0, dst, 0, srcLen);
            return dst;
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
