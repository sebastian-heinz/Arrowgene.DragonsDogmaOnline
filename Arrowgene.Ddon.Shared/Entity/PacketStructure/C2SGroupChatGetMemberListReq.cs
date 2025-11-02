using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGroupChatGetMemberListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GROUP_CHAT_GROUP_CHAT_GET_MEMBER_LIST_REQ;

        public ulong GroupId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SGroupChatGetMemberListReq>
        {
            public override void Write(IBuffer buffer, C2SGroupChatGetMemberListReq obj)
            {
                WriteUInt64(buffer, obj.GroupId);
            }

            public override C2SGroupChatGetMemberListReq Read(IBuffer buffer)
            {
                C2SGroupChatGetMemberListReq obj = new C2SGroupChatGetMemberListReq();
                obj.GroupId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
