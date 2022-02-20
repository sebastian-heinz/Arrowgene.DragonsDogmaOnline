using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Network
{
    public class Packet : IPacket
    {
        public Packet(PacketId id, byte[] data, PacketSource source, uint count)
        {
            Id = id;
            Source = source;
            Data = data;
            Count = count;
        }

        public Packet(PacketId id, byte[] data)
        {
            Id = id;
            Source = PacketSource.Unknown;
            Data = data;
            Count = 0;
        }

        public PacketId Id { get; set; }
        public byte[] Data { get; set; }
        public PacketSource Source { get; set; }
        public uint Count { get; set; }

        public byte[] GetHeaderBytes()
        {
            if (Id == PacketId.C2S_CERT_CLIENT_CHALLENGE_REQ
                || Id == PacketId.C2L_CLIENT_CHALLENGE_REQ)
            {
                return new byte[0];
            }

            IBuffer buffer = new StreamBuffer();
            buffer.WriteByte(Id.GroupId);
            buffer.WriteUInt16(Id.HandlerId, Endianness.Big);
            buffer.WriteByte(Id.HandlerSubId);
            buffer.WriteByte((byte) Source);
            buffer.WriteUInt32(Count, Endianness.Big);
            return buffer.GetAllBytes();
        }

        public IBuffer AsBuffer()
        {
            IBuffer buffer = new StreamBuffer(Data);
            buffer.SetPositionStart();
            return buffer;
        }

        public string PrintHeader()
        {
            return $"{Source} #{Count:000000} ({Id.GroupId}.{Id.HandlerId}.{Id.HandlerSubId}) {Id.Name}";
        }

        public string PrintHeaderBytes()
        {
            return $"{Util.HexDump(GetHeaderBytes())}";
        }

        public string PrintData()
        {
            return Util.HexDump(Data);
        }

        public override string ToString()
        {
            return $"{PrintHeader()}{Environment.NewLine}{PrintHeaderBytes()}{PrintData()}";
        }
    }
}
