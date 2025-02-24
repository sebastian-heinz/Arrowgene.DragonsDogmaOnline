using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyRecvBinaryMsgAllNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_RECV_BINARY_MSG_ALL_NTC;

        public uint CharacterId { get; set; }
        public byte[] Data { get; set; } = System.Array.Empty<byte>();
        public OnlineStatus OnlineStatus { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyRecvBinaryMsgAllNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyRecvBinaryMsgAllNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteInt32(buffer, obj.Data.Length);
                WriteByteArray(buffer, obj.Data);
                WriteByte(buffer, (byte) obj.OnlineStatus);
            }

            public override S2CPartyRecvBinaryMsgAllNtc Read(IBuffer buffer)
            {
                S2CPartyRecvBinaryMsgAllNtc obj = new S2CPartyRecvBinaryMsgAllNtc();
                obj.CharacterId = ReadUInt32(buffer);
                int dataLength = ReadInt32(buffer);
                obj.Data = ReadByteArray(buffer, dataLength);
                obj.OnlineStatus = (OnlineStatus) ReadByte(buffer);
                return obj;
            }
        }
    }
}
