using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftElement
    {
        public CDataCraftElement()
        {
            ItemUId = string.Empty;
        }

        public string ItemUId { get; set; }
        public byte SlotNo { get; set; }

        public class Serializer : EntitySerializer<CDataCraftElement>
        {
            public override void Write(IBuffer buffer, CDataCraftElement obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteByte(buffer, obj.SlotNo);
            }

            public override CDataCraftElement Read(IBuffer buffer)
            {
                CDataCraftElement obj = new CDataCraftElement();
                obj.ItemUId = ReadMtString(buffer);
                obj.SlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}
