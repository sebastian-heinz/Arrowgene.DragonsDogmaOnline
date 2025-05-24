using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipStatParam
    {
        public byte EffectID; // 27 is inflict Gold, 26 is inflict petrification, etc. 
        public ushort EffectValue;
        
        public class Serializer : EntitySerializer<CDataEquipStatParam>
        {
            public override void Write(IBuffer buffer, CDataEquipStatParam obj)
            {
                WriteByte(buffer, obj.EffectID);
                WriteUInt16(buffer, obj.EffectValue);
            }

            public override CDataEquipStatParam Read(IBuffer buffer)
            {
                CDataEquipStatParam obj = new CDataEquipStatParam();
                obj.EffectID = ReadByte(buffer);
                obj.EffectValue = ReadUInt16(buffer);
                return obj;
            }
        }
    }

}
