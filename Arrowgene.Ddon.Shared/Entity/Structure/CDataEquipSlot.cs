using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipSlot
    {

        public CDataEquipSlot()
        {
        }

        public uint Unk0 { get; set; }
        public uint Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public ushort Unk3 { get; set; }

        public class Serializer : EntitySerializer<CDataEquipSlot>
        {
            public override void Write(IBuffer buffer, CDataEquipSlot obj)
            {
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
                WriteUInt16(buffer, obj.Unk3);
            }

            public override CDataEquipSlot Read(IBuffer buffer)
            {
                    CDataEquipSlot obj = new CDataEquipSlot();
                    obj.Unk0 = ReadUInt32(buffer);
                    obj.Unk1 = ReadUInt32(buffer);
                    obj.Unk2 = ReadByte(buffer);
                    obj.Unk3 = ReadUInt16(buffer);
                    return obj;
            }
        }
    }
}
