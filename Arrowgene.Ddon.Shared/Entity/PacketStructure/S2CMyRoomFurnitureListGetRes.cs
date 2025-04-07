using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMyRoomFurnitureListGetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MY_ROOM_FURNITURE_LIST_GET_RES;

        public List<CDataFurnitureLayout> FurnitureList { get; set; } = new();
        public CDataMyRoomOption MyRoomOption { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CMyRoomFurnitureListGetRes>
        {
            public override void Write(IBuffer buffer, S2CMyRoomFurnitureListGetRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntityList<CDataFurnitureLayout>(buffer, obj.FurnitureList);
                WriteEntity<CDataMyRoomOption>(buffer, obj.MyRoomOption);
            }

            public override S2CMyRoomFurnitureListGetRes Read(IBuffer buffer)
            {
                S2CMyRoomFurnitureListGetRes obj = new S2CMyRoomFurnitureListGetRes();

                ReadServerResponse(buffer, obj);

                obj.FurnitureList = ReadEntityList<CDataFurnitureLayout>(buffer);
                obj.MyRoomOption = ReadEntity<CDataMyRoomOption>(buffer);

                return obj;
            }
        }
    }
}
