using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendApproveFriendRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_FRIEND_APPROVE_FRIEND_RES;

        public CDataFriendInfo FriendInfo { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CFriendApproveFriendRes>
        {
            public override void Write(IBuffer buffer, S2CFriendApproveFriendRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.FriendInfo);
                
            }

            public override S2CFriendApproveFriendRes Read(IBuffer buffer)
            {
                S2CFriendApproveFriendRes obj = new S2CFriendApproveFriendRes();
                ReadServerResponse(buffer, obj);
                obj.FriendInfo = ReadEntity<CDataFriendInfo>(buffer);
                return obj;
            }
            
        }
    }
}
