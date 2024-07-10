using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipSlot
    {

        public CDataEquipSlot()
        {
        }

        public uint CharId { get; set; }
        public uint PawnId { get; set; }
        public byte EquipType { get; set; }
        public ushort EquipSlot { get; set; }

        public class Serializer : EntitySerializer<CDataEquipSlot>
        {
            public override void Write(IBuffer buffer, CDataEquipSlot obj)
            {
                WriteUInt32(buffer, obj.CharId);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, obj.EquipType);
                WriteUInt16(buffer, obj.EquipSlot);
            }

            public override CDataEquipSlot Read(IBuffer buffer)
            {
                    CDataEquipSlot obj = new CDataEquipSlot();
                    obj.CharId = ReadUInt32(buffer);
                    obj.PawnId = ReadUInt32(buffer);
                    obj.EquipType = ReadByte(buffer);
                    obj.EquipSlot = ReadUInt16(buffer);
                    return obj;
            }
        }
    }
}
