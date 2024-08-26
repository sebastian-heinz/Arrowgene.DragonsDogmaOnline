using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftEquipEnhancedEnhanceItemReqUnk0
    {
        public CDataCraftEquipEnhancedEnhanceItemReqUnk0()
        {
        }
    
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }
    
        public class Serializer : EntitySerializer<CDataCraftEquipEnhancedEnhanceItemReqUnk0>
        {
            public override void Write(IBuffer buffer, CDataCraftEquipEnhancedEnhanceItemReqUnk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }
        
            public override CDataCraftEquipEnhancedEnhanceItemReqUnk0 Read(IBuffer buffer)
            {
                CDataCraftEquipEnhancedEnhanceItemReqUnk0 obj = new CDataCraftEquipEnhancedEnhanceItemReqUnk0();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}