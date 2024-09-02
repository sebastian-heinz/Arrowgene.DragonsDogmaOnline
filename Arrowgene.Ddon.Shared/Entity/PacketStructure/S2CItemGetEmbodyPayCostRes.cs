using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CItemGetEmbodyPayCostRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ITEM_GET_EMBODY_PAY_COST_RES;

        public S2CItemGetEmbodyPayCostRes()
        {
            EmbodyCostList = new List<CDataItemEmbodyCostParam>();
        }

        public List <CDataItemEmbodyCostParam> EmbodyCostList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CItemGetEmbodyPayCostRes>
        {
            public override void Write(IBuffer buffer, S2CItemGetEmbodyPayCostRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.EmbodyCostList);
            }

            public override S2CItemGetEmbodyPayCostRes Read(IBuffer buffer)
            {
                S2CItemGetEmbodyPayCostRes obj = new S2CItemGetEmbodyPayCostRes();
                ReadServerResponse(buffer, obj);
                obj.EmbodyCostList = ReadEntityList<CDataItemEmbodyCostParam>(buffer);
                return obj;
            }
        }
    }
}
