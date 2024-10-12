using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSupplyItem
    {
        public CDataSupplyItem()
        {
        }

        public uint ItemId { get; set; }
        public ushort ItemNum { get; set; }

        public class Serializer : EntitySerializer<CDataSupplyItem>
        {
            public override void Write(IBuffer buffer, CDataSupplyItem obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.ItemNum);
            }

            public override CDataSupplyItem Read(IBuffer buffer)
            {
                CDataSupplyItem obj = new CDataSupplyItem();
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemNum = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
