using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipItemInfoUnk2
    {
        public byte SlotNo;
        public ushort ItemId;
        
        public class Serializer : EntitySerializer<CDataEquipItemInfoUnk2>
        {
            public override void Write(IBuffer buffer, CDataEquipItemInfoUnk2 obj)
            {
                WriteByte(buffer, obj.SlotNo);
                WriteUInt16(buffer, obj.ItemId);
            }

            public override CDataEquipItemInfoUnk2 Read(IBuffer buffer)
            {
                CDataEquipItemInfoUnk2 obj = new CDataEquipItemInfoUnk2();
                obj.SlotNo = ReadByte(buffer);
                obj.ItemId = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
