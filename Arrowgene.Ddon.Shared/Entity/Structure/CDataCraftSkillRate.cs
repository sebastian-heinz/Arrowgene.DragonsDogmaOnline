using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftSkillRate
    {
        public byte PawnType { get; set; }
        public byte SkillType { get; set; }
        public byte Rate { get; set; }
    
        public class Serializer : EntitySerializer<CDataCraftSkillRate>
        {
            public override void Write(IBuffer buffer, CDataCraftSkillRate obj)
            {
                WriteByte(buffer, obj.PawnType);
                WriteByte(buffer, obj.SkillType);
                WriteByte(buffer, obj.Rate);
            }
        
            public override CDataCraftSkillRate Read(IBuffer buffer)
            {
                CDataCraftSkillRate obj = new CDataCraftSkillRate();
                obj.PawnType = ReadByte(buffer);
                obj.SkillType = ReadByte(buffer);
                obj.Rate = ReadByte(buffer);
                return obj;
            }
        }
    }
}