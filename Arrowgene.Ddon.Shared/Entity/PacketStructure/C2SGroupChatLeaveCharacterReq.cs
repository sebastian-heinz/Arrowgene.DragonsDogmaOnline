using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGroupChatLeaveCharacterReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GROUP_CHAT_GROUP_CHAT_LEAVE_CHARACTER_REQ;

        public ulong GroupId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SGroupChatLeaveCharacterReq>
        {
            public override void Write(IBuffer buffer, C2SGroupChatLeaveCharacterReq obj)
            {
                WriteUInt64(buffer, obj.GroupId);
            }

            public override C2SGroupChatLeaveCharacterReq Read(IBuffer buffer)
            {
                C2SGroupChatLeaveCharacterReq obj = new C2SGroupChatLeaveCharacterReq();
                obj.GroupId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
