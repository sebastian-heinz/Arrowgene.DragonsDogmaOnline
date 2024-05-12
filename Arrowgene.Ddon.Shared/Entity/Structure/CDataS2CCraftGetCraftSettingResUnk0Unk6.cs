using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataS2CCraftGetCraftSettingResUnk0Unk6
    {
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }
    
        public class Serializer : EntitySerializer<CDataS2CCraftGetCraftSettingResUnk0Unk6>
        {
            public override void Write(IBuffer buffer, CDataS2CCraftGetCraftSettingResUnk0Unk6 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }
        
            public override CDataS2CCraftGetCraftSettingResUnk0Unk6 Read(IBuffer buffer)
            {
                CDataS2CCraftGetCraftSettingResUnk0Unk6 obj = new CDataS2CCraftGetCraftSettingResUnk0Unk6();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}