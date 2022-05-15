using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSkillParam
    {
        public CDataSkillParam()
        {
            Params = new List<CDataSkillLevelParam>();
        }

        public uint SkillNo { get; set; }
        public byte Job { get; set; }
        public byte Type { get; set; }
        public List<CDataSkillLevelParam> Params { get; set; }

        public class Serializer : EntitySerializer<CDataSkillParam>
        {
            public override void Write(IBuffer buffer, CDataSkillParam obj)
            {
                WriteUInt32(buffer, obj.SkillNo);
                WriteByte(buffer, obj.Job);
                WriteByte(buffer, obj.Type);
                WriteEntityList<CDataSkillLevelParam>(buffer, obj.Params);
            }

            public override CDataSkillParam Read(IBuffer buffer)
            {
                CDataSkillParam obj = new CDataSkillParam();
                obj.SkillNo = ReadUInt32(buffer);
                obj.Job = ReadByte(buffer);
                obj.Type = ReadByte(buffer);
                obj.Params = ReadEntityList<CDataSkillLevelParam>(buffer);
                return obj;
            }
        }
    }
}