using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipEnhanceUnk0
    {
        public byte Unk0 { get; set; }
        public uint Unk1 { get; set; }

        public class Serializer : EntitySerializer<CDataEquipEnhanceUnk0>
        {
            public override void Write(IBuffer buffer, CDataEquipEnhanceUnk0 obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
            }

            public override CDataEquipEnhanceUnk0 Read(IBuffer buffer)
            {
                CDataEquipEnhanceUnk0 obj = new CDataEquipEnhanceUnk0();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }

}
