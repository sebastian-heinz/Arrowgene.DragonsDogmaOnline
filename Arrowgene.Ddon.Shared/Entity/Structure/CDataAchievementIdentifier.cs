using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataAchievementIdentifier
    {
        public uint UId;
        public uint Index;
    }

    public class CDataAchievementIdentifierSerializer : EntitySerializer<CDataAchievementIdentifier>
    {
        public override void Write(IBuffer buffer, CDataAchievementIdentifier obj)
        {
            WriteUInt32(buffer, obj.UId);
            WriteUInt32(buffer, obj.Index);
        }

        public override CDataAchievementIdentifier Read(IBuffer buffer)
        {
            CDataAchievementIdentifier obj = new CDataAchievementIdentifier();
            obj.UId = ReadUInt32(buffer);
            obj.Index = ReadUInt32(buffer);
            return obj;
        }
    }
}
