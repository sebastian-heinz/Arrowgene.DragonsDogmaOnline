using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataBorderSupplyItem
    {
        public CDataBorderSupplyItem()
        {
            SupplyItemList = new List<CDataSupplyItem>();
        }

        public uint MinAreaPoint { get; set; }
        public List<CDataSupplyItem> SupplyItemList { get; set; }

        public class Serializer : EntitySerializer<CDataBorderSupplyItem>
        {
            public override void Write(IBuffer buffer, CDataBorderSupplyItem obj)
            {
                WriteUInt32(buffer, obj.MinAreaPoint);
                WriteEntityList(buffer, obj.SupplyItemList);
            }

            public override CDataBorderSupplyItem Read(IBuffer buffer)
            {
                CDataBorderSupplyItem obj = new CDataBorderSupplyItem();
                obj.MinAreaPoint = ReadUInt32(buffer);
                obj.SupplyItemList = ReadEntityList<CDataSupplyItem>(buffer);
                return obj;
            }
        }
    }
}
