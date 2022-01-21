using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public struct CDataEquipJobItem
    {
        public uint JobItemID;
        public byte EquipSlotNo;
    }

    public class CDataEquipJobItemSerializer : EntitySerializer<CDataEquipJobItem>
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
