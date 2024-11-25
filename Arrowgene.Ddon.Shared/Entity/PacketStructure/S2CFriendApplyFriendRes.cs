using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendApplyFriendRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_FRIEND_APPLY_FRIEND_RES;
        
        public S2CFriendApplyFriendRes()
        {
            FriendInfo = new();
        }

        public CDataFriendInfo FriendInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CFriendApplyFriendRes>
        {
            public override void Write(IBuffer buffer, S2CFriendApplyFriendRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntity(buffer, obj.FriendInfo);
                
            }

            public override S2CFriendApplyFriendRes Read(IBuffer buffer)
            {
                S2CFriendApplyFriendRes obj = new S2CFriendApplyFriendRes();
                ReadServerResponse(buffer, obj);
                obj.FriendInfo = ReadEntity<CDataFriendInfo>(buffer);
                return obj;
            }
            
        }
    }
}

