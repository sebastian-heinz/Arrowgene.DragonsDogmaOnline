using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceTreasurePointGetListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_TREASURE_POINT_GET_LIST_RES;

        public S2CInstanceTreasurePointGetListRes()
        {
        }

        public uint CategoryId { get; set; }
        public List<CDataTreasurePoint> TreasurePointList { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CInstanceTreasurePointGetListRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceTreasurePointGetListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.CategoryId);
                WriteEntityList(buffer, obj.TreasurePointList);
            }

            public override S2CInstanceTreasurePointGetListRes Read(IBuffer buffer)
            {
                S2CInstanceTreasurePointGetListRes obj = new S2CInstanceTreasurePointGetListRes();
                ReadServerResponse(buffer, obj);
                obj.CategoryId = ReadUInt32(buffer);
                obj.TreasurePointList = ReadEntityList<CDataTreasurePoint>(buffer);
                return obj;
            }
        }
    }
}
