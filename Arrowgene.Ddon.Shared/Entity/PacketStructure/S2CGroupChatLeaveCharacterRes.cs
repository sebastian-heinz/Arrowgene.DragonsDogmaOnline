using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGroupChatLeaveCharacterRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GROUP_CHAT_GROUP_CHAT_LEAVE_CHARACTER_RES;

        public class Serializer : PacketEntitySerializer<S2CGroupChatLeaveCharacterRes>
        {
            public override void Write(IBuffer buffer, S2CGroupChatLeaveCharacterRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CGroupChatLeaveCharacterRes Read(IBuffer buffer)
            {
                S2CGroupChatLeaveCharacterRes obj = new S2CGroupChatLeaveCharacterRes();
                ReadServerResponse(buffer, obj);

                return obj;
            }
        }
    }
}
