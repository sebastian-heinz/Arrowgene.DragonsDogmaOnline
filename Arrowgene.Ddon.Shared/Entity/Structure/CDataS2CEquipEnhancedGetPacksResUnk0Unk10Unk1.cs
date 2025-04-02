using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1
    {
        public ushort BuffId { get; set; }
        public ushort Unk1 { get; set; } // Controls the % some how
        public byte Unk2 { get; set; } // Controls which window the text shows in?
    
        public class Serializer : EntitySerializer<CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1>
        {
            public override void Write(IBuffer buffer, CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1 obj)
            {
                WriteUInt16(buffer, obj.BuffId);
                WriteUInt16(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
            }
        
            public override CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1 Read(IBuffer buffer)
            {
                CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1 obj = new CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1();
                obj.BuffId = ReadUInt16(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                obj.Unk2 = ReadByte(buffer);
                return obj;
            }
        }
    }
}
