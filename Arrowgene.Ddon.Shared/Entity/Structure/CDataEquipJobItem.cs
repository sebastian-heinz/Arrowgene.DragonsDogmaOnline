using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipJobItem
    {
        public CDataEquipJobItem()
        {
            JobItemID=0;
            EquipSlotNo=0;
        }

        public uint JobItemID { get; set; }
        public byte EquipSlotNo { get; set; }

        public class Serializer : EntitySerializer<CDataEquipJobItem>
        {
            public override void Write(IBuffer buffer, CDataEquipJobItem obj)
            {
                WriteUInt32(buffer, obj.JobItemID);
                WriteByte(buffer, obj.EquipSlotNo);
            }

            public override CDataEquipJobItem Read(IBuffer buffer)
            {
                CDataEquipJobItem obj = new CDataEquipJobItem();
                obj.JobItemID = ReadUInt32(buffer);
                obj.EquipSlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }

}
