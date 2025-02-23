using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CInstanceTreasurePointGetCategoryListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_INSTANCE_TREASURE_POINT_GET_CATEGORY_LIST_RES;

        public S2CInstanceTreasurePointGetCategoryListRes()
        {
        }

        public List<CDataTreasurePointCategory> TreasurePointCategories { get; set; } = new();

        public C2SInstanceTreasurePointGetCategoryListReq ReqData { get; set; }

        public class Serializer : PacketEntitySerializer<S2CInstanceTreasurePointGetCategoryListRes>
        {
            public override void Write(IBuffer buffer, S2CInstanceTreasurePointGetCategoryListRes obj)
            {
                C2SInstanceTreasurePointGetCategoryListReq req = obj.ReqData;
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.TreasurePointCategories);
            }

            public override S2CInstanceTreasurePointGetCategoryListRes Read(IBuffer buffer)
            {
                S2CInstanceTreasurePointGetCategoryListRes obj = new S2CInstanceTreasurePointGetCategoryListRes();
                ReadServerResponse(buffer, obj);
                ReadEntityList<CDataTreasurePointCategory>(buffer);
                return obj;
            }
        }
    }
}
