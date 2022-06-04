using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSkillLevelParam
    {
        public byte Lv { get; set; }
        public uint RequireJobLevel { get; set; }
        public uint RequireJobPoint { get; set; }
        public bool IsRelease { get; set; }

        public class Serializer : EntitySerializer<CDataSkillLevelParam>
        {
            public override void Write(IBuffer buffer, CDataSkillLevelParam obj)
            {
                WriteByte(buffer, obj.Lv);
                WriteUInt32(buffer, obj.RequireJobLevel);
                WriteUInt32(buffer, obj.RequireJobPoint);
                WriteBool(buffer, obj.IsRelease);
            }

            public override CDataSkillLevelParam Read(IBuffer buffer)
            {
                CDataSkillLevelParam obj = new CDataSkillLevelParam();
                obj.Lv = ReadByte(buffer);
                obj.RequireJobLevel = ReadUInt32(buffer);
                obj.RequireJobPoint = ReadUInt32(buffer);
                obj.IsRelease = ReadBool(buffer);
                return obj;
            }
        }
    }
}