using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SItemGetEmbodyPayCostReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_ITEM_GET_EMBODY_PAY_COST_REQ;

        public C2SItemGetEmbodyPayCostReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SItemGetEmbodyPayCostReq>
        {
            public override void Write(IBuffer buffer, C2SItemGetEmbodyPayCostReq obj)
            {
            }

            public override C2SItemGetEmbodyPayCostReq Read(IBuffer buffer)
            {
                C2SItemGetEmbodyPayCostReq obj = new C2SItemGetEmbodyPayCostReq();
                return obj;
            }
        }
    }
}
