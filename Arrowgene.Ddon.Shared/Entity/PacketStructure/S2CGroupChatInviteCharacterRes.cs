using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGroupChatInviteCharacterRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GROUP_CHAT_GROUP_CHAT_INVITE_CHARACTER_RES;

        public class Serializer : PacketEntitySerializer<S2CGroupChatInviteCharacterRes>
        {
            public override void Write(IBuffer buffer, S2CGroupChatInviteCharacterRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CGroupChatInviteCharacterRes Read(IBuffer buffer)
            {
                S2CGroupChatInviteCharacterRes obj = new S2CGroupChatInviteCharacterRes();
                ReadServerResponse(buffer, obj);

                return obj;
            }
        }
    }
}
