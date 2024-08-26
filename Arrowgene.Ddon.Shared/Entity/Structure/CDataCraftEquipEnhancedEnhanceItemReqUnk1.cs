using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftEquipEnhancedEnhanceItemReqUnk1
    {
        public CDataCraftEquipEnhancedEnhanceItemReqUnk1()
        {
        }
    
        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public ushort Unk2 { get; set; }
    
        public class Serializer : EntitySerializer<CDataCraftEquipEnhancedEnhanceItemReqUnk1>
        {
            public override void Write(IBuffer buffer, CDataCraftEquipEnhancedEnhanceItemReqUnk1 obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt16(buffer, obj.Unk2);
            }
        
            public override CDataCraftEquipEnhancedEnhanceItemReqUnk1 Read(IBuffer buffer)
            {
                CDataCraftEquipEnhancedEnhanceItemReqUnk1 obj = new CDataCraftEquipEnhancedEnhanceItemReqUnk1();
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}