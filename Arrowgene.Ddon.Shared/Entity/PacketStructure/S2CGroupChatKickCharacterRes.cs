using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGroupChatKickCharacterRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GROUP_CHAT_GROUP_CHAT_KICK_CHARACTER_RES;

        public class Serializer : PacketEntitySerializer<S2CGroupChatKickCharacterRes>
        {
            public override void Write(IBuffer buffer, S2CGroupChatKickCharacterRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CGroupChatKickCharacterRes Read(IBuffer buffer)
            {
                S2CGroupChatKickCharacterRes obj = new S2CGroupChatKickCharacterRes();
                ReadServerResponse(buffer, obj);

                return obj;
            }
        }
    }
}
