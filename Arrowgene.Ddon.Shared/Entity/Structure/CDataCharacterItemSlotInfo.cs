using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterItemSlotInfo
    {
        public CDataCharacterItemSlotInfo()
        {
            Unk0=0;
            Unk1=0;
        }

        public byte Unk0;
        public ushort Unk1;

        public class Serializer : EntitySerializer<CDataCharacterItemSlotInfo>
        {
            public override void Write(IBuffer buffer, CDataCharacterItemSlotInfo obj)
            {
                WriteByte(buffer, obj.Unk0);
                WriteUInt16(buffer, obj.Unk1);
            }

            public override CDataCharacterItemSlotInfo Read(IBuffer buffer)
            {
                CDataCharacterItemSlotInfo obj = new CDataCharacterItemSlotInfo();
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
