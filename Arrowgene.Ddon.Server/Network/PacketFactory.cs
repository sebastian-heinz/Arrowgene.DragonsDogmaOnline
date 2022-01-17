using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Logging;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Crypto;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.Shared
{
    public class PacketFactory
    {
        private static readonly DdonLogger Logger = LogProvider.Logger<DdonLogger>(typeof(PacketFactory));
        private static readonly Camellia Camellia = new Camellia();

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
        private const int PacketMinimumDataSize = 16;
        private const uint CamelliaKeyLength = 32 * 8;

        private bool _readHeader;
        private ushort _dataSize;
        private uint _packetCount;
        private int _position;
        private IBuffer _buffer;
        private readonly ServerSetting _setting;
        private byte[] _camelliaKey;
        private IPacketIdResolver _packetIdResolver;
        private Memory<byte> _t8Encrypt;
        private Memory<byte> _t8Decrypt;
        private Memory<byte> _camelliaSubKey;


        public PacketFactory(ServerSetting setting, IPacketIdResolver packetIdResolver)
        {
            _setting = setting;
            _packetCount = 1;
            _packetIdResolver = packetIdResolver;
            _t8Encrypt = new Memory<byte>(new byte[8]);
            _t8Decrypt = new Memory<byte>(new byte[8]);
            SetCamelliaKey(CamelliaKey);
            Reset();
        }

        public void SetCamelliaKey(byte[] camelliaKey)
        {
            _camelliaKey = Util.Copy(camelliaKey);
            Camellia.KeySchedule(_camelliaKey, out _camelliaSubKey);
        }

        public byte[] WriteDataWithLengthPrefix(byte[] data)
        {
            if (data == null)
            {
                Logger.Error($"data == null, tried to write invalid data");
                return null;
            }

            int totalLength = data.Length + PacketLengthFieldSize;
            if (totalLength < 0 || totalLength > ushort.MaxValue)
            {
                Logger.Error($"dataLength < 0 || dataLength > ushort.MaxValue (dataLength:{totalLength})");
                return null;
            }

            byte[] encryptedPacketData = Encrypt(data);

            IBuffer buffer = Util.Buffer.Provide();
            buffer.WriteUInt16((ushort) encryptedPacketData.Length /* without length prefix */, Endianness.Big);
            buffer.WriteBytes(encryptedPacketData);
            return buffer.GetAllBytes();
        }

        public byte[] Write(Packet packet)
        {
            packet.Source = PacketSource.Server;
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
                return null;
            }

            if (true)
            {
                // temporary solution to attach original packet name to custom packet names
                PacketId knownPacketId = _packetIdResolver.Get(packet.Id.GroupId, packet.Id.HandlerId, packet.Id.HandlerSubId);
                if (knownPacketId != PacketId.UNKNOWN
                    && !packet.Id.Name.Contains("->")
                    && knownPacketId.Name != packet.Id.Name
                )
                {
                    string combinedName = knownPacketId.Name + "->" + packet.Id.Name;
                    packet.Id = new PacketId(packet.Id.GroupId, packet.Id.HandlerId, packet.Id.HandlerSubId, combinedName);
                }
            }

            if (packet.Id.HandlerSubId == 16)
            {
                // only increase for _NTC packets
                packet.Count = _packetCount;
                _packetCount++;
            }
            else
            {
                packet.Count = 0;
            }

            IBuffer packetDataBuffer = Util.Buffer.Provide();
            packetDataBuffer.WriteByte(packet.Id.GroupId);
            packetDataBuffer.WriteUInt16(packet.Id.HandlerId, Endianness.Big);
            packetDataBuffer.WriteByte(packet.Id.HandlerSubId);
            packetDataBuffer.WriteByte((byte) packet.Source);
            packetDataBuffer.WriteUInt32(packet.Count, Endianness.Big);

            packetDataBuffer.WriteBytes(data);

            // Align packet data to 16 bytes.
            var neededBytes = 16 - (packetDataBuffer.Position % 16);
            packetDataBuffer.WriteBytes(new byte[neededBytes]);

            byte[] packetData = packetDataBuffer.GetAllBytes();
            byte[] encryptedPacketData = Encrypt(packetData);

            IBuffer buffer = Util.Buffer.Provide();
            buffer.WriteUInt16((ushort) encryptedPacketData.Length /* without length prefix */, Endianness.Big);
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
                    if (_dataSize < PacketMinimumDataSize)
                    {
                        Logger.Error($"DataSize:{_dataSize} < PacketMinimumDataSize:{PacketMinimumDataSize}");
                        Reset();
                        return packets;
                    }

                    _readHeader = true;
                }

                if (_readHeader && _buffer.Size - _buffer.Position >= _dataSize)
                {
                    byte[] encryptedPacketData = _buffer.ReadBytes(_dataSize);
                    byte[] packetData = Decrypt(encryptedPacketData);

                    IBuffer packetBuffer = new StreamBuffer(packetData);
                    packetBuffer.SetPositionStart();
                    byte groupId = packetBuffer.ReadByte();
                    ushort handlerId = packetBuffer.ReadUInt16(Endianness.Big);
                    byte handlerSubId = packetBuffer.ReadByte();
                    byte packetSourceByte = packetBuffer.ReadByte();
                    uint packetCount = packetBuffer.ReadUInt32(Endianness.Big);

                    byte[] payload;
                    PacketId packetId;
                    PacketSource packetSource = PacketSource.Unknown;
                    if (Enum.IsDefined(typeof(PacketSource), packetSourceByte))
                    {
                        packetSource = (PacketSource) packetSourceByte;
                    }

                    // abusing this as part of header validation
                    if (packetSource != PacketSource.Server
                        && packetSource != PacketSource.Client)
                    {
                        // TODO potentially change this to check if it is the first packet of a session
                        // and treat it as the challenge
                        payload = packetData;
                        if (_packetIdResolver.ServerType == ServerType.Game)
                        {
                            packetId = PacketId.C2S_CERT_CLIENT_CHALLENGE_REQ;
                        }
                        else if (_packetIdResolver.ServerType == ServerType.Login)
                        {
                            packetId = PacketId.C2L_CLIENT_CHALLENGE_REQ;
                        }
                        else
                        {
                            Logger.Error($"Invalid Server Type");
                            Reset();
                            return packets;
                        }
                    }
                    else
                    {
                        payload = packetBuffer.ReadBytes(packetBuffer.Size - packetBuffer.Position);
                        packetId = _packetIdResolver.Get(groupId, handlerId, handlerSubId);
                    }

                    Packet packet = new Packet(packetId, payload, packetSource, packetCount);
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
            Camellia.Encrypt(
                data,
                out Span<byte> encrypted,
                CamelliaKeyLength,
                _camelliaSubKey.Span,
                Util.Copy(CamelliaIv),
                _t8Encrypt.Span
            );
            return encrypted.ToArray();
        }

        public byte[] Decrypt(byte[] encrypted)
        {
            Camellia.Decrypt(
                encrypted,
                out Span<byte> decrypted,
                CamelliaKeyLength,
                _camelliaSubKey.Span,
                Util.Copy(CamelliaIv),
                _t8Decrypt.Span
            );
            return decrypted.ToArray();
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
