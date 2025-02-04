using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftGetCraftProductRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CRAFT_GET_CRAFT_PRODUCT_RES;

        public CDataCraftProduct CraftProduct { get; set; }
        public List<CDataItemUpdateResult> UpdateItemList { get; set; }

        public C2SCraftGetCraftProductRes()
        {
            CraftProduct = new();
            UpdateItemList = new List<CDataItemUpdateResult>();
        }

        public class Serializer : PacketEntitySerializer<C2SCraftGetCraftProductRes>
        {
            public override void Write(IBuffer buffer, C2SCraftGetCraftProductRes obj)
            {
                WriteServerResponse(buffer, obj);

                WriteEntity(buffer, obj.CraftProduct);
                WriteEntityList(buffer, obj.UpdateItemList);
            }

            public override C2SCraftGetCraftProductRes Read(IBuffer buffer)
            {
                C2SCraftGetCraftProductRes obj = new C2SCraftGetCraftProductRes();

                ReadServerResponse(buffer, obj);

                obj.CraftProduct = ReadEntity<CDataCraftProduct>(buffer);
                obj.UpdateItemList = ReadEntityList<CDataItemUpdateResult>(buffer);

                return obj;
            }
        }
    }
}
