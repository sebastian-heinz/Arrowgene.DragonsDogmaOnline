using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendCancelFriendApplicationRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_FRIEND_CANCEL_FRIEND_APPLICATION_RES;
        
        public int Result { get; set; }


        public class Serializer : PacketEntitySerializer<S2CFriendCancelFriendApplicationRes>
        {
            public override void Write(IBuffer buffer, S2CFriendCancelFriendApplicationRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteInt32(buffer, obj.Result);
                
            }

            public override S2CFriendCancelFriendApplicationRes Read(IBuffer buffer)
            {
                S2CFriendCancelFriendApplicationRes obj = new S2CFriendCancelFriendApplicationRes();
                ReadServerResponse(buffer, obj);
                obj.Result = ReadInt32(buffer);
                return obj;
            }
            
        }
    }
}

