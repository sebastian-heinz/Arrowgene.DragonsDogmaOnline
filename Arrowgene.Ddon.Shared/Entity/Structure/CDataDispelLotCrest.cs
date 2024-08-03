using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataDispelLotCrest
    {
        public CDataDispelLotCrest()
        {
            Unk2 = new List<CDataDispelLotCrestUnk2>();
        }

        public uint CrestItemId { get; set; }
        public byte CrestItemRate { get; set; }
        public byte Unk0 { get; set; }
        public byte Unk1 {  get; set; }

        public List<CDataDispelLotCrestUnk2> Unk2 {  get; set; }

        public class Serializer : EntitySerializer<CDataDispelLotCrest>
        {
            public override void Write(IBuffer buffer, CDataDispelLotCrest obj)
            {
                WriteUInt32(buffer, obj.CrestItemId);
                WriteByte(buffer, obj.CrestItemRate);
                WriteByte(buffer, obj.Unk0);
                WriteByte(buffer, obj.Unk1);
                WriteEntityList(buffer, obj.Unk2);
            }

            public override CDataDispelLotCrest Read(IBuffer buffer)
            {
                CDataDispelLotCrest obj = new CDataDispelLotCrest();
                obj.CrestItemId = ReadUInt32(buffer);
                obj.CrestItemRate = ReadByte(buffer);
                obj.Unk0 = ReadByte(buffer);
                obj.Unk1 = ReadByte(buffer);
                obj.Unk2 = ReadEntityList<CDataDispelLotCrestUnk2>(buffer);
                return obj;
            }
        }
    }
}

