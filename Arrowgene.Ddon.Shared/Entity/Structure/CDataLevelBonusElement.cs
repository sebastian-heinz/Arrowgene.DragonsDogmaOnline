using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataLevelBonusElement
    {
        public uint MinLevel { get; set; } // Maybe?
        public uint MaxLevel { get; set; } // Maybe?
        public ushort GoldRatio { get; set; }
        public ushort ExpRatio { get; set; }
        public ushort RimRatio { get; set; }
        public ushort AreaPointRatio { get; set; }

        public class Serializer : EntitySerializer<CDataLevelBonusElement>
        {
            public override void Write(IBuffer buffer, CDataLevelBonusElement obj)
            {
                WriteUInt32(buffer, obj.MinLevel);
                WriteUInt32(buffer, obj.MaxLevel);
                WriteUInt16(buffer, obj.GoldRatio);
                WriteUInt16(buffer, obj.ExpRatio);
                WriteUInt16(buffer, obj.RimRatio);
                WriteUInt16(buffer, obj.AreaPointRatio);
            }

            public override CDataLevelBonusElement Read(IBuffer buffer)
            {
                CDataLevelBonusElement obj = new CDataLevelBonusElement();
                obj.MinLevel = ReadUInt32(buffer);
                obj.MaxLevel = ReadUInt32(buffer);
                obj.GoldRatio = ReadUInt16(buffer);
                obj.ExpRatio = ReadUInt16(buffer);
                obj.RimRatio = ReadUInt16(buffer);
                obj.AreaPointRatio = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
