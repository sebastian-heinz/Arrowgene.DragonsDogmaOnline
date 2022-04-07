using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomUpdatePlanetariumRes : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_MY_ROOM_UPDATE_PLANETARIUM_RES;

        public S2CMyRoomUpdatePlanetariumRes()
        {
        }

        public C2SMyRoomUpdatePlanetariumReq ItemId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMyRoomUpdatePlanetariumRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomUpdatePlanetariumRes obj)
            {
                C2SMyRoomUpdatePlanetariumReq req = obj.ItemId;
                WriteUInt64(buffer, 0);
                WriteUInt32(buffer, req.ItemId);
            }

            public override S2CMyRoomUpdatePlanetariumRes Read(IBuffer buffer)
            {
                S2CMyRoomUpdatePlanetariumRes obj = new S2CMyRoomUpdatePlanetariumRes();
                return obj;
            }
        }

    }
}
