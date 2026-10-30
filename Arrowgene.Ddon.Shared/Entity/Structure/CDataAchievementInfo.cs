using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAchievementInfo
    {
        public uint UId { get; set; }
        public byte Category { get; set; }

        public class Serializer : EntitySerializer<CDataAchievementInfo>
        {
            public override void Write(IBuffer buffer, CDataAchievementInfo obj)
            {
                WriteUInt32(buffer, obj.UId);
                WriteByte(buffer, obj.Category);
            }

            public override CDataAchievementInfo Read(IBuffer buffer)
            {
                CDataAchievementInfo obj = new CDataAchievementInfo();
                obj.UId = ReadUInt32(buffer);
                obj.Category = ReadByte(buffer);
                return obj;
            }
        }
    }
}
