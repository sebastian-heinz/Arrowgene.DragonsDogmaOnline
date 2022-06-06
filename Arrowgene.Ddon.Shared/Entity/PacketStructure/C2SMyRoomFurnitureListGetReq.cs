using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMyRoomFurnitureListGetReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MY_ROOM_FURNITURE_LIST_GET_REQ;

        public uint Data0 { get; set; }

        public C2SMyRoomFurnitureListGetReq()
        {
            Data0 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SMyRoomFurnitureListGetReq>
        {
            public override void Write(IBuffer buffer, C2SMyRoomFurnitureListGetReq obj)
            {
                WriteUInt32(buffer, obj.Data0);
            }

            public override C2SMyRoomFurnitureListGetReq Read(IBuffer buffer)
            {
                C2SMyRoomFurnitureListGetReq obj = new C2SMyRoomFurnitureListGetReq();
                obj.Data0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
