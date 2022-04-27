using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomMyRoomBgmUpdateRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_MY_ROOM_MY_ROOM_BGM_UPDATE_RES;

        public S2CMyRoomMyRoomBgmUpdateRes()
        {
        }

        public C2SMyRoomMyRoomBgmUpdateReq ItemId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMyRoomMyRoomBgmUpdateRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomMyRoomBgmUpdateRes obj)
            {
                C2SMyRoomMyRoomBgmUpdateReq req = obj.ItemId;
                WriteUInt64(buffer, 0);
                WriteUInt32(buffer, req.ItemId);
            }

            public override S2CMyRoomMyRoomBgmUpdateRes Read(IBuffer buffer)
            {
                S2CMyRoomMyRoomBgmUpdateRes obj = new S2CMyRoomMyRoomBgmUpdateRes();
                return obj;
            }
        }

    }
}
