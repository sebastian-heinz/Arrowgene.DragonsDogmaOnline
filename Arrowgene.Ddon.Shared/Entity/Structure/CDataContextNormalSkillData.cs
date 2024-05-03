using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    // Ingame: Core Skills
    public class CDataContextNormalSkillData
    {

        public CDataContextNormalSkillData(CDataNormalSkillParam normalSkillParam)
        {
            SkillIndex = (byte) normalSkillParam.Index;
        }

        public static List<CDataContextNormalSkillData> FromCDataNormalSkillParams(List<CDataNormalSkillParam> skillParams)
        {
            List<CDataContextNormalSkillData> obj = new List<CDataContextNormalSkillData>();
            foreach (var skillParam in skillParams)
            {
                obj.Add(new CDataContextNormalSkillData(skillParam));
            }
            return obj;
        }

        public CDataContextNormalSkillData()
        {
            SkillIndex=0;
        }

        public byte SkillIndex;

        public class Serializer : EntitySerializer<CDataContextNormalSkillData>
        {
            public override void Write(IBuffer buffer, CDataContextNormalSkillData obj)
            {
                WriteByte(buffer, obj.SkillIndex);
            }

            public override CDataContextNormalSkillData Read(IBuffer buffer)
            {
                CDataContextNormalSkillData obj = new CDataContextNormalSkillData();
                obj.SkillIndex = ReadByte(buffer);
                return obj;
            }
        }
    }
}
