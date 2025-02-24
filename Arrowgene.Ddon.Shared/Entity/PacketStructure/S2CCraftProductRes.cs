using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CCraftGetCraftProductRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_GET_CRAFT_PRODUCT_RES;

        public CDataCraftProduct CraftProduct { get; set; }
        public List<CDataItemUpdateResult> UpdateItemList { get; set; }

        public S2CCraftGetCraftProductRes()
        {
            CraftProduct = new();
            UpdateItemList = new List<CDataItemUpdateResult>();
        }

        public class Serializer : PacketEntitySerializer<S2CCraftGetCraftProductRes>
        {
            public override void Write(IBuffer buffer, S2CCraftGetCraftProductRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntity(buffer, obj.CraftProduct);
                WriteEntityList(buffer, obj.UpdateItemList);
            }

            public override S2CCraftGetCraftProductRes Read(IBuffer buffer)
            {
                S2CCraftGetCraftProductRes obj = new S2CCraftGetCraftProductRes();

                ReadServerResponse(buffer, obj);

                obj.CraftProduct = ReadEntity<CDataCraftProduct>(buffer);
                obj.UpdateItemList = ReadEntityList<CDataItemUpdateResult>(buffer);

                return obj;
            }
        }
    }
}
