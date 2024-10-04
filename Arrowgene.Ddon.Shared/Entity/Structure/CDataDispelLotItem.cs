using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLotItem
    {
        public CDataDispelLotItem()
        {
        }

        public uint ItemId { get; set; }
        public byte ItemRate { get; set; }
        public byte CrestNum { get; set; }
        public ushort Amount {  get; set; }
        public bool Unk1 { get; set; }
        public byte Unk2 { get; set; }

        public class Serializer : EntitySerializer<CDataDispelLotItem>
        {
            public override void Write(IBuffer buffer, CDataDispelLotItem obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteByte(buffer, obj.ItemRate);
                WriteByte(buffer, obj.CrestNum);
                WriteUInt16(buffer, obj.Amount);
                WriteBool(buffer, obj.Unk1);
                WriteByte(buffer, obj.Unk2);
            }

            public override CDataDispelLotItem Read(IBuffer buffer)
            {
                CDataDispelLotItem obj = new CDataDispelLotItem();
                obj.ItemId = ReadUInt32(buffer);
                obj.ItemRate = ReadByte(buffer);
                obj.CrestNum = ReadByte(buffer);
                obj.Amount = ReadUInt16(buffer);
                obj.Unk1 = ReadBool(buffer);
                obj.Unk2 = ReadByte(buffer);
                return obj;
            }
        }
    }
}

