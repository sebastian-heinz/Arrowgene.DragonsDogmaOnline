using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartySendBinaryMsgNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_SEND_BINARY_MSG_NTC;

        public C2SPartySendBinaryMsgNtc()
        {
            CharacterIdList = new List<CDataCommonU32>();
            Data = new byte[0];
        }

        public List<CDataCommonU32> CharacterIdList { get; set; }
        public byte[] Data { get; set; }

        public class Serializer : PacketEntitySerializer<C2SPartySendBinaryMsgNtc>
        {
            public override void Write(IBuffer buffer, C2SPartySendBinaryMsgNtc obj)
            {
                WriteEntityList<CDataCommonU32>(buffer, obj.CharacterIdList);
                WriteInt32(buffer, obj.Data.Length);
                WriteByteArray(buffer, obj.Data);
            }

            public override C2SPartySendBinaryMsgNtc Read(IBuffer buffer)
            {
                C2SPartySendBinaryMsgNtc obj = new C2SPartySendBinaryMsgNtc();
                obj.CharacterIdList = ReadEntityList<CDataCommonU32>(buffer);
                int dataLength = ReadInt32(buffer);
                obj.Data = ReadByteArray(buffer, dataLength);
                return obj;
            }
        }
    }
}