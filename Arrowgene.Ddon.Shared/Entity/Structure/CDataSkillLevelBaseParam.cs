using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSkillLevelBaseParam
    {
        public JobId Job { get; set; }
        public uint SkillNo { get; set; }
        public byte SkillLv { get; set; }

        public class Serializer : EntitySerializer<CDataSkillLevelBaseParam>
        {
            public override void Write(IBuffer buffer, CDataSkillLevelBaseParam obj)
            {
                WriteByte(buffer, (byte) obj.Job);
                WriteUInt32(buffer, obj.SkillNo);
                WriteByte(buffer, obj.SkillLv);
            }

            public override CDataSkillLevelBaseParam Read(IBuffer buffer)
            {
                CDataSkillLevelBaseParam obj = new CDataSkillLevelBaseParam();
                obj.Job = (JobId) ReadByte(buffer);
                obj.SkillNo = ReadUInt32(buffer);
                obj.SkillLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}
