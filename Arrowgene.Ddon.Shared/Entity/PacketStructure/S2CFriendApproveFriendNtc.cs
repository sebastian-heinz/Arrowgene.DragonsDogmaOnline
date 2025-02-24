using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendApproveFriendNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_FRIEND_APPROVE_FRIEND_NTC;

        public bool IsApproved { get; set; }
        public CDataFriendInfo FriendInfo { get; set; } = new();


        public class Serializer : PacketEntitySerializer<S2CFriendApproveFriendNtc>
        {
            public override void Write(IBuffer buffer, S2CFriendApproveFriendNtc obj)
            {
                WriteBool(buffer, obj.IsApproved);
                WriteEntity(buffer, obj.FriendInfo);
                
            }

            public override S2CFriendApproveFriendNtc Read(IBuffer buffer)
            {
                S2CFriendApproveFriendNtc obj = new S2CFriendApproveFriendNtc();
                obj.IsApproved = ReadBool(buffer);
                obj.FriendInfo = ReadEntity<CDataFriendInfo>(buffer);
                return obj;
            }
            
        }
    }
}

