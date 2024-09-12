using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMyRoomUpdatePlanetariumReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MY_ROOM_UPDATE_PLANETARIUM_REQ;

        public uint PlanetariumId { get; set; }
        
        public C2SMyRoomUpdatePlanetariumReq()
        {
            PlanetariumId = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SMyRoomUpdatePlanetariumReq>
        {
            public override void Write(IBuffer buffer, C2SMyRoomUpdatePlanetariumReq obj)
            {
                WriteUInt32(buffer, obj.PlanetariumId);
            }

            public override C2SMyRoomUpdatePlanetariumReq Read(IBuffer buffer)
            {
                C2SMyRoomUpdatePlanetariumReq obj = new C2SMyRoomUpdatePlanetariumReq();
                obj.PlanetariumId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
