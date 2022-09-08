using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipElementParam
    {
        public byte SlotNo;
        public ushort ItemId;
        
        public class Serializer : EntitySerializer<CDataEquipElementParam>
        {
            public override void Write(IBuffer buffer, CDataEquipElementParam obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteUInt16(buffer, obj.ItemId);
            }

            public override CDataEquipElementParam Read(IBuffer buffer)
            {
                CDataEquipElementParam obj = new CDataEquipElementParam();
                obj.SlotNo = ReadByte(buffer);
                obj.ItemId = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
