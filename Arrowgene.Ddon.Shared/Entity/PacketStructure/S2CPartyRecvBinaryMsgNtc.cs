using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartyRecvBinaryMsgNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTY_RECV_BINARY_MSG_NOTICE;

        public uint CharacterId { get; set; }
        public byte[] Data { get; set; }
        public byte OnlineStatus { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartyRecvBinaryMsgNtc>
        {
            public override void Write(IBuffer buffer, S2CPartyRecvBinaryMsgNtc obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteInt32(buffer, obj.Data.Length);
                WriteByteArray(buffer, obj.Data);
                WriteByte(buffer, obj.OnlineStatus);
            }

            public override S2CPartyRecvBinaryMsgNtc Read(IBuffer buffer)
            {
                S2CPartyRecvBinaryMsgNtc obj = new S2CPartyRecvBinaryMsgNtc();
                obj.CharacterId = ReadUInt32(buffer);
                int dataLength = ReadInt32(buffer);
                obj.Data = ReadByteArray(buffer, dataLength);
                obj.OnlineStatus = ReadByte(buffer);
                return obj;
            }
        }
    }
}