using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartySendBinaryMsgAllNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_SEND_BINARY_MSG_ALL_NTC;

        public C2SPartySendBinaryMsgAllNtc()
        {
            Data = new byte[0];
        }

        public byte[] Data { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPartySendBinaryMsgAllNtc>
        {
            public override void Write(IBuffer buffer, C2SPartySendBinaryMsgAllNtc obj)
            {
                WriteInt32(buffer, obj.Data.Length);
                WriteByteArray(buffer, obj.Data);
            }

            public override C2SPartySendBinaryMsgAllNtc Read(IBuffer buffer)
            {
                C2SPartySendBinaryMsgAllNtc obj = new C2SPartySendBinaryMsgAllNtc();
                int dataLength = ReadInt32(buffer);
                obj.Data = ReadByteArray(buffer, dataLength);
                return obj;
            }
        }
    }
}