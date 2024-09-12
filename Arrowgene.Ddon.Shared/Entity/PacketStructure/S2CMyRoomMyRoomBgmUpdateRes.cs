using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomMyRoomBgmUpdateRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MY_ROOM_MY_ROOM_BGM_UPDATE_RES;

        public S2CMyRoomMyRoomBgmUpdateRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CMyRoomMyRoomBgmUpdateRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomMyRoomBgmUpdateRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CMyRoomMyRoomBgmUpdateRes Read(IBuffer buffer)
            {
                S2CMyRoomMyRoomBgmUpdateRes obj = new S2CMyRoomMyRoomBgmUpdateRes();

                ReadServerResponse(buffer, obj);

                return obj;
            }
        }
    }
}
