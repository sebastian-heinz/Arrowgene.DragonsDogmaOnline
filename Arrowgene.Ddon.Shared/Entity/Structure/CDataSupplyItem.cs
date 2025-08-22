using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSupplyItem
    {
        public CDataSupplyItem()
        {
        }

        public ItemId ItemId { get; set; }
        public ushort ItemNum { get; set; }

        public class Serializer : EntitySerializer<CDataSupplyItem>
        {
            public override void Write(IBuffer buffer, CDataSupplyItem obj)
            {
                WriteUInt32(buffer, (uint)obj.ItemId);
                WriteUInt16(buffer, obj.ItemNum);
            }

            public override CDataSupplyItem Read(IBuffer buffer)
            {
                CDataSupplyItem obj = new CDataSupplyItem();
                obj.ItemId = (ItemId)ReadUInt32(buffer);
                obj.ItemNum = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
