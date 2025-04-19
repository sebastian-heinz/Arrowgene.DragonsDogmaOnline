using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSkillParam
    {
        public CDataSkillParam()
        {
            Params = new List<CDataSkillLevelParam>();
        }

        public uint SkillNo { get; set; }
        public JobId Job { get; set; }
        public ReleaseType Type { get; set; }
        public List<CDataSkillLevelParam> Params { get; set; }

        public class Serializer : EntitySerializer<CDataSkillParam>
        {
            public override void Write(IBuffer buffer, CDataSkillParam obj)
            {
                WriteUInt32(buffer, obj.SkillNo);
                WriteByte(buffer, (byte) obj.Job);
                WriteByte(buffer, (byte) obj.Type);
                WriteEntityList<CDataSkillLevelParam>(buffer, obj.Params);
            }

            public override CDataSkillParam Read(IBuffer buffer)
            {
                CDataSkillParam obj = new CDataSkillParam();
                obj.SkillNo = ReadUInt32(buffer);
                obj.Job = (JobId) ReadByte(buffer);
                obj.Type = (ReleaseType) ReadByte(buffer);
                obj.Params = ReadEntityList<CDataSkillLevelParam>(buffer);
                return obj;
            }
        }
    }
}
