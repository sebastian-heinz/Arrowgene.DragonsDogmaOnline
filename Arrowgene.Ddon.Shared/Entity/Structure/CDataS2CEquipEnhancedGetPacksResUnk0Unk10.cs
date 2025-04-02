using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CEquipEnhancedGetPacksResUnk0Unk10
    {
        public CDataS2CEquipEnhancedGetPacksResUnk0Unk10() {
            EffectParamList = new List<CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1>();
        }
    
        public byte Unk0 { get; set; }
        public List<CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1> EffectParamList { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CEquipEnhancedGetPacksResUnk0Unk10>
        {
            public override void Write(IBuffer buffer, CDataS2CEquipEnhancedGetPacksResUnk0Unk10 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1>(buffer, obj.EffectParamList);
            }
        
            public override CDataS2CEquipEnhancedGetPacksResUnk0Unk10 Read(IBuffer buffer)
            {
                CDataS2CEquipEnhancedGetPacksResUnk0Unk10 obj = new CDataS2CEquipEnhancedGetPacksResUnk0Unk10();
                obj.Unk0 = ReadByte(buffer);
                obj.EffectParamList = ReadEntityList<CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1>(buffer);
                return obj;
            }
        }
    }
}
