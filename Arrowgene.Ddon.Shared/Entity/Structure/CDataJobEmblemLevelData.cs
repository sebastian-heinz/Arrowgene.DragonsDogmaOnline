using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemLevelData
    {
        public ushort Level { get; set; }
        public uint PPAmount { get; set; }
        public byte EPGain { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemLevelData>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemLevelData obj)
            {
                WriteUInt16(buffer, obj.Level);
                WriteUInt32(buffer, obj.PPAmount);
                WriteByte(buffer, obj.EPGain);
            }

            public override CDataJobEmblemLevelData Read(IBuffer buffer)
            {
                CDataJobEmblemLevelData obj = new CDataJobEmblemLevelData();
                obj.Level = ReadUInt16(buffer);
                obj.PPAmount = ReadUInt32(buffer);
                obj.EPGain = ReadByte(buffer);
                return obj;
            }
        }
    }
}
