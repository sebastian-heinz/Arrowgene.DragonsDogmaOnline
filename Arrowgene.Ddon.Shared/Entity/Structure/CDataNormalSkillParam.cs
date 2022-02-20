using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataNormalSkillParam
    {
        public CDataNormalSkillParam()
        {
            Job=0;
            SkillNo=0;
            Index=0;
            PreSkillNo=0;
        }

        public byte Job { get; set; }
        public uint SkillNo { get; set; }
        public uint Index { get; set; }
        public uint PreSkillNo { get; set; }

        public class Serializer : EntitySerializer<CDataNormalSkillParam>
        {
            public override void Write(IBuffer buffer, CDataNormalSkillParam obj)
            {
                WriteByte(buffer, obj.Job);
                WriteUInt32(buffer, obj.SkillNo);
                WriteUInt32(buffer, obj.Index);
                WriteUInt32(buffer, obj.PreSkillNo);
            }

            public override CDataNormalSkillParam Read(IBuffer buffer)
            {
                CDataNormalSkillParam obj = new CDataNormalSkillParam();
                obj.Job = ReadByte(buffer);
                obj.SkillNo = ReadUInt32(buffer);
                obj.Index = ReadUInt32(buffer);
                obj.PreSkillNo = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
