using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomUpdatePlanetariumRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MY_ROOM_UPDATE_PLANETARIUM_RES;

        public S2CMyRoomUpdatePlanetariumRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CMyRoomUpdatePlanetariumRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomUpdatePlanetariumRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CMyRoomUpdatePlanetariumRes Read(IBuffer buffer)
            {
                S2CMyRoomUpdatePlanetariumRes obj = new S2CMyRoomUpdatePlanetariumRes();

                ReadServerResponse(buffer, obj);

                return obj;
            }
        }
    }
}
