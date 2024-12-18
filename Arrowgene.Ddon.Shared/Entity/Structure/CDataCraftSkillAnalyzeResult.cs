using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftSkillAnalyzeResult
    {
        public CDataCraftSkillAnalyzeResult()
        {
        }

        public CraftSkillType SkillType { get; set; }
        public byte Rate {  get; set; }
        public uint Value0 { get; set; }
        public uint Value1 { get; set; }

        public class Serializer : EntitySerializer<CDataCraftSkillAnalyzeResult>
        {
            public override void Write(IBuffer buffer, CDataCraftSkillAnalyzeResult obj)
            {
                WriteByte(buffer, (byte)obj.SkillType);
                WriteByte(buffer, obj.Rate);
                WriteUInt32(buffer, obj.Value0);
                WriteUInt32(buffer, obj.Value1);
            }

            public override CDataCraftSkillAnalyzeResult Read(IBuffer buffer)
            {
                CDataCraftSkillAnalyzeResult obj = new CDataCraftSkillAnalyzeResult();
                obj.SkillType = (CraftSkillType)ReadByte(buffer);
                obj.Rate = ReadByte(buffer);
                obj.Value0 = ReadUInt32(buffer);
                obj.Value1 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
