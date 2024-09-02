using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataEquipSlot
    {
        public CDataEquipSlot()
        {
            CharacterId = 0;
            PawnId = 0;
        }

        public uint CharacterId { get; set; }
        public uint PawnId {  get; set; }
        public EquipType EquipType {  get; set; }
        public ushort EquipSlotNo {  get; set; }

        public class Serializer : EntitySerializer<CDataEquipSlot>
        {
            public override void Write(IBuffer buffer, CDataEquipSlot obj)
            {
                WriteUInt32(buffer, obj.CharacterId);
                WriteUInt32(buffer, obj.PawnId);
                WriteByte(buffer, (byte) obj.EquipType);
                WriteUInt16(buffer, obj.EquipSlotNo);
            }

            public override CDataEquipSlot Read(IBuffer buffer)
            {
                CDataEquipSlot obj = new CDataEquipSlot();
                obj.CharacterId = ReadUInt32(buffer);
                obj.PawnId = ReadUInt32(buffer);
                obj.EquipType = (EquipType) ReadByte(buffer);
                obj.EquipSlotNo = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
