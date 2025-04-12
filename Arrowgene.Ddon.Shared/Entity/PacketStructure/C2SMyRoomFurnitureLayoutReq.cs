using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMyRoomFurnitureLayoutReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MY_ROOM_FURNITURE_LAYOUT_REQ;

        public List<CDataFurnitureLayoutData> FurnitureLayoutData { get; set; } = new();

        public class Serializer : PacketEntitySerializer<C2SMyRoomFurnitureLayoutReq>
        {
            public override void Write(IBuffer buffer, C2SMyRoomFurnitureLayoutReq obj)
            {
                WriteEntityList<CDataFurnitureLayoutData>(buffer, obj.FurnitureLayoutData);
            }

            public override C2SMyRoomFurnitureLayoutReq Read(IBuffer buffer)
            {
                C2SMyRoomFurnitureLayoutReq obj = new C2SMyRoomFurnitureLayoutReq();
                obj.FurnitureLayoutData = ReadEntityList<CDataFurnitureLayoutData>(buffer);
                return obj;
            }
        }
    }
}
