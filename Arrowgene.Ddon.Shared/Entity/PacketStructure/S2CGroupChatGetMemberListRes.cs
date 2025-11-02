using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CGroupChatGetMemberListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_GROUP_CHAT_GROUP_CHAT_GET_MEMBER_LIST_RES;

        public List<CDataCharacterListElement> GroupMemberList { get; set; } = [];

        public class Serializer : PacketEntitySerializer<S2CGroupChatGetMemberListRes>
        {
            public override void Write(IBuffer buffer, S2CGroupChatGetMemberListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.GroupMemberList);
            }

            public override S2CGroupChatGetMemberListRes Read(IBuffer buffer)
            {
                S2CGroupChatGetMemberListRes obj = new S2CGroupChatGetMemberListRes();
                ReadServerResponse(buffer, obj);
                obj.GroupMemberList = ReadEntityList<CDataCharacterListElement>(buffer);

                return obj;
            }
        }
    }
}
