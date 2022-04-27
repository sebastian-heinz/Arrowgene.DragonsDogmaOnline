using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGpGetValidChatComGroupRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GP_GET_VALID_CHAT_COM_GROUP_RES;

        public S2CGpGetValidChatComGroupRes()
        {
            Unk0 = 0;
            Unk1 = 0;
            Unk2 = new CDataCommonU32();
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public CDataCommonU32 Unk2 { get; set; }

        public class Serializer : PacketEntitySerializer<S2CGpGetValidChatComGroupRes>
        {
            public override void Write(IBuffer buffer, S2CGpGetValidChatComGroupRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteEntity<CDataCommonU32>(buffer, obj.Unk2);
            }

            public override S2CGpGetValidChatComGroupRes Read(IBuffer buffer)
            {
                S2CGpGetValidChatComGroupRes packet = new S2CGpGetValidChatComGroupRes();
                ReadServerResponse(buffer, packet);
                packet.Unk0 = ReadUInt32(buffer);
                packet.Unk1 = ReadUInt32(buffer);
                packet.Unk2 = ReadEntity<CDataCommonU32>(buffer);
                return packet;
            }
        }
    }
}