using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Crest
    {
        public uint Slot { get; set; }
        public uint CrestId { get; set; }
        public uint Amount { get; set; }

        // TODO: Rename to ToCDataEquipElementParam
        public CDataEquipElementParam ToCDataEquipElementParam()
        {
            return new CDataEquipElementParam()
            {
                SlotNo = (byte)Slot,
                CrestId = CrestId,
                Add = (ushort)Amount
            };
        }
    }
}
