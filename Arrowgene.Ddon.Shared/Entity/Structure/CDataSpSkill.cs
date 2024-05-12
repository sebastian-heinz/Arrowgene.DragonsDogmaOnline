using System;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSpSkill
    {
        public byte SpSkillId { get; set; }
        public byte SpSkillLv { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            CDataSpSkill other = obj as CDataSpSkill;
            return other.SpSkillId == SpSkillId && other.SpSkillLv == SpSkillLv;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(SpSkillId, SpSkillLv);
        }

        public class Serializer : EntitySerializer<CDataSpSkill>
        {
            public override void Write(IBuffer buffer, CDataSpSkill obj)
            {
                WriteByte(buffer, obj.SpSkillId);
                WriteByte(buffer, obj.SpSkillLv);
            }

            public override CDataSpSkill Read(IBuffer buffer)
            {
                CDataSpSkill obj = new CDataSpSkill();
                obj.SpSkillId = ReadByte(buffer);
                obj.SpSkillLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}