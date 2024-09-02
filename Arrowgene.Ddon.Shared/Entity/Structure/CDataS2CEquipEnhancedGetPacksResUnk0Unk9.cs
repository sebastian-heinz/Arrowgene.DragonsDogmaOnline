using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CEquipEnhancedGetPacksResUnk0Unk9
    {
        public byte Unk0 { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CEquipEnhancedGetPacksResUnk0Unk9>
        {
            public override void Write(IBuffer buffer, CDataS2CEquipEnhancedGetPacksResUnk0Unk9 obj)
            {
                WriteByte(buffer, obj.Unk0);
            }
        
            public override CDataS2CEquipEnhancedGetPacksResUnk0Unk9 Read(IBuffer buffer)
            {
                CDataS2CEquipEnhancedGetPacksResUnk0Unk9 obj = new CDataS2CEquipEnhancedGetPacksResUnk0Unk9();
                obj.Unk0 = ReadByte(buffer);
                return obj;
            }
        }
    }
}