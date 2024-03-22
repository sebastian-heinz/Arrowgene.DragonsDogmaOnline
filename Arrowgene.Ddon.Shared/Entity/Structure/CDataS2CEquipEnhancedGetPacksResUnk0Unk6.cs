using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CEquipEnhancedGetPacksResUnk0Unk6
    {
        public uint Unk0 { get; set; }
        public ushort Unk1 { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>
        {
            public override void Write(IBuffer buffer, CDataS2CEquipEnhancedGetPacksResUnk0Unk6 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
            }
        
            public override CDataS2CEquipEnhancedGetPacksResUnk0Unk6 Read(IBuffer buffer)
            {
                CDataS2CEquipEnhancedGetPacksResUnk0Unk6 obj = new CDataS2CEquipEnhancedGetPacksResUnk0Unk6();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}