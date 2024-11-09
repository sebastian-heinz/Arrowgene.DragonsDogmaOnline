using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SFriendGetFriendListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_FRIEND_GET_FRIEND_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SFriendGetFriendListReq>
        {
            public override void Write(IBuffer buffer, C2SFriendGetFriendListReq obj)
            {
            }

            public override C2SFriendGetFriendListReq Read(IBuffer buffer)
            {
                C2SFriendGetFriendListReq obj = new C2SFriendGetFriendListReq();
                return obj;
            }
        }

    }
}
