using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendRemoveFriendRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_FRIEND_REMOVE_FRIEND_RES;
        
        public int Result { get; set; }


        public class Serializer : PacketEntitySerializer<S2CFriendRemoveFriendRes>
        {
            public override void Write(IBuffer buffer, S2CFriendRemoveFriendRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteInt32(buffer, obj.Result);
                
            }

            public override S2CFriendRemoveFriendRes Read(IBuffer buffer)
            {
                S2CFriendRemoveFriendRes obj = new S2CFriendRemoveFriendRes();
                ReadServerResponse(buffer, obj);
                obj.Result = ReadInt32(buffer);
                return obj;
            }
            
        }
    }
}

