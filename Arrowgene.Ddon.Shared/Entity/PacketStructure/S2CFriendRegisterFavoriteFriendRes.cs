using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CFriendRegisterFavoriteFriendRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_FRIEND_REGISTER_FAVORITE_FRIEND_RES;

        public class Serializer : PacketEntitySerializer<S2CFriendRegisterFavoriteFriendRes>
        {
            public override void Write(IBuffer buffer, S2CFriendRegisterFavoriteFriendRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CFriendRegisterFavoriteFriendRes Read(IBuffer buffer)
            {
                S2CFriendRegisterFavoriteFriendRes obj = new S2CFriendRegisterFavoriteFriendRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
