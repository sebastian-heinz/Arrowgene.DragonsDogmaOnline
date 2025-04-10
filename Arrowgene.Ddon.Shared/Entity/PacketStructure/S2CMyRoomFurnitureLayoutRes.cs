using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomFurnitureLayoutRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MY_ROOM_FURNITURE_LAYOUT_RES;

        public class Serializer : PacketEntitySerializer<S2CMyRoomFurnitureLayoutRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomFurnitureLayoutRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CMyRoomFurnitureLayoutRes Read(IBuffer buffer)
            {
                S2CMyRoomFurnitureLayoutRes obj = new S2CMyRoomFurnitureLayoutRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
