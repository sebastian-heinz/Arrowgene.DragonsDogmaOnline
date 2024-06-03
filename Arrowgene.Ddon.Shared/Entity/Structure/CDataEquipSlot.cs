using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipSlot
    {

        public CDataEquipSlot()
        {
        }
        public int Unk0 { get; set; }
        public int Unk1 { get; set; }
        public byte Unk2 { get; set; }
        public ushort Unk3 { get; set; }

        public class Serializer : EntitySerializer<CDataEquipSlot>
        {
            public override void Write(IBuffer buffer, CDataEquipSlot obj)
            {
                WriteInt32(buffer, obj.Unk0);
                WriteInt32(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
                WriteUInt16(buffer, obj.Unk3);
            }

            public override CDataEquipSlot Read(IBuffer buffer)
            {
                    CDataEquipSlot obj = new CDataEquipSlot();
                    obj.Unk0 = ReadInt32(buffer);
                    obj.Unk1 = ReadInt32(buffer);
                    obj.Unk2 = ReadByte(buffer);
                    obj.Unk3 = ReadUInt16(buffer);
                    return obj;
                
            }
        }
    }
}
