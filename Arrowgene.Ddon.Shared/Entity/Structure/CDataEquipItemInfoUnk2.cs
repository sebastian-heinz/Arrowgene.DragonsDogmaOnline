using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipItemInfoUnk2
    {
        public byte EffectID; // 27 is inflict Gold, 26 is inflict petrification, etc. 
        public ushort EffectValue;
        
        public class Serializer : EntitySerializer<CDataEquipItemInfoUnk2>
        {
            public override void Write(IBuffer buffer, CDataEquipItemInfoUnk2 obj)
            {
                WriteByte(buffer, obj.EffectID);
                WriteUInt16(buffer, obj.EffectValue);
            }

            public override CDataEquipItemInfoUnk2 Read(IBuffer buffer)
            {
                CDataEquipItemInfoUnk2 obj = new CDataEquipItemInfoUnk2();
                obj.EffectID = ReadByte(buffer);
                obj.EffectValue = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
