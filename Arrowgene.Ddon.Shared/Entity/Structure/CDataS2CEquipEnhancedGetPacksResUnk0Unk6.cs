using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CEquipEnhancedGetPacksResUnk0Unk6
    {
        public uint ItemId { get; set; }
        public ushort Num { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CEquipEnhancedGetPacksResUnk0Unk6>
        {
            public override void Write(IBuffer buffer, CDataS2CEquipEnhancedGetPacksResUnk0Unk6 obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteUInt16(buffer, obj.Num);
            }
        
            public override CDataS2CEquipEnhancedGetPacksResUnk0Unk6 Read(IBuffer buffer)
            {
                CDataS2CEquipEnhancedGetPacksResUnk0Unk6 obj = new CDataS2CEquipEnhancedGetPacksResUnk0Unk6();
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
