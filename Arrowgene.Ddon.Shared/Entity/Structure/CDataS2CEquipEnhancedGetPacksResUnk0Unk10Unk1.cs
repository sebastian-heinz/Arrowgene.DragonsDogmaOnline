using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1
    {
        public ushort Unk0 { get; set; }
        public ushort Unk1 { get; set; }
        public byte Unk2 { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1>
        {
            public override void Write(IBuffer buffer, CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1 obj)
            {
                WriteUInt16(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
            }
        
            public override CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1 Read(IBuffer buffer)
            {
                CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1 obj = new CDataS2CEquipEnhancedGetPacksResUnk0Unk10Unk1();
                obj.Unk0 = ReadUInt16(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                obj.Unk2 = ReadByte(buffer);
                return obj;
            }
        }
    }
}