using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    // Ingame: Core Skills
    public class CDataContextNormalSkillData
    {
        public CDataContextNormalSkillData()
        {
            SkillNo=0;
        }

        public byte SkillNo;

        public class Serializer : EntitySerializer<CDataContextNormalSkillData>
        {
            public override void Write(IBuffer buffer, CDataContextNormalSkillData obj)
            {
                WriteByte(buffer, obj.SkillNo);
            }

            public override CDataContextNormalSkillData Read(IBuffer buffer)
            {
                CDataContextNormalSkillData obj = new CDataContextNormalSkillData();
                obj.SkillNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}