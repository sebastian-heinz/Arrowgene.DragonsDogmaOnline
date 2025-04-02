using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipEnhanceItem
    {
        public ItemId ItemId { get; set; }
        public ushort Amount { get; set; }

        public class Serializer : EntitySerializer<CDataEquipEnhanceItem>
        {
            public override void Write(IBuffer buffer, CDataEquipEnhanceItem obj)
            {
                WriteUInt32(buffer, (uint) obj.ItemId);
                WriteUInt16(buffer, obj.Amount);
            }

            public override CDataEquipEnhanceItem Read(IBuffer buffer)
            {
                CDataEquipEnhanceItem obj = new CDataEquipEnhanceItem();
                obj.ItemId = (ItemId) ReadUInt32(buffer);
                obj.Amount = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
