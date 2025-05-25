using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemStatCostData
    {
        public byte StatLevel { get; set; } // Level of a stat
        public uint EPAmount { get; set; } // EP cost to move to next level

        public class Serializer : EntitySerializer<CDataJobEmblemStatCostData>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemStatCostData obj)
            {
                WriteByte(buffer, obj.StatLevel);
                WriteUInt32(buffer, obj.EPAmount);
            }

            public override CDataJobEmblemStatCostData Read(IBuffer buffer)
            {
                CDataJobEmblemStatCostData obj = new CDataJobEmblemStatCostData();
                obj.StatLevel = ReadByte(buffer);
                obj.EPAmount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
