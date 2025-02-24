using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendApplyFriendNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_FRIEND_APPLY_FRIEND_NTC;
        
        public CDataFriendInfo FriendInfo { get; set; } = new();


        public class Serializer : PacketEntitySerializer<S2CFriendApplyFriendNtc>
        {
            public override void Write(IBuffer buffer, S2CFriendApplyFriendNtc obj)
            {
                WriteEntity(buffer, obj.FriendInfo);
                
            }

            public override S2CFriendApplyFriendNtc Read(IBuffer buffer)
            {
                S2CFriendApplyFriendNtc obj = new S2CFriendApplyFriendNtc();
                obj.FriendInfo = ReadEntity<CDataFriendInfo>(buffer);
                return obj;
            }
            
        }
    }
}

