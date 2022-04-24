using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGpGetValidChatComGroupReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GP_GET_VALID_CHAT_COM_GROUP_REQ;

        public class Serializer : PacketEntitySerializer<C2SGpGetValidChatComGroupReq>
        {
            public override void Write(IBuffer buffer, C2SGpGetValidChatComGroupReq obj)
            {
            }

            public override C2SGpGetValidChatComGroupReq Read(IBuffer buffer)
            {
                return new C2SGpGetValidChatComGroupReq();
            }
        }
    }
}