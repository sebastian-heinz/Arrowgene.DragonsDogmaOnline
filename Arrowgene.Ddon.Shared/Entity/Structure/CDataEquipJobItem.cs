using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipJobItem
    {
        public CDataEquipJobItem()
        {
            JobItemId = 0;
            EquipSlotNo = 0;
        }

        public uint JobItemId { get; set; }
        public byte EquipSlotNo { get; set; }

        public class Serializer : EntitySerializer<CDataEquipJobItem>
        {
            public override void Write(IBuffer buffer, CDataEquipJobItem obj)
            {
                WriteUInt32(buffer, obj.JobItemId);
                WriteByte(buffer, obj.EquipSlotNo);
            }

            public override CDataEquipJobItem Read(IBuffer buffer)
            {
                CDataEquipJobItem obj = new CDataEquipJobItem();
                obj.JobItemId = ReadUInt32(buffer);
                obj.EquipSlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}
