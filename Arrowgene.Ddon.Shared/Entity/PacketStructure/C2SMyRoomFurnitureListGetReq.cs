using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMyRoomFurnitureListGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MY_ROOM_FURNITURE_LIST_GET_REQ;

        public C2SMyRoomFurnitureListGetReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMyRoomFurnitureListGetReq>
        {
            public override void Write(IBuffer buffer, C2SMyRoomFurnitureListGetReq obj)
            {
            }

            public override C2SMyRoomFurnitureListGetReq Read(IBuffer buffer)
            {
                C2SMyRoomFurnitureListGetReq obj = new C2SMyRoomFurnitureListGetReq();
                return obj;
            }
        }
    }
}
