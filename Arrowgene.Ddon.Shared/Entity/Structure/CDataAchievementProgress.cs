using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataAchievementProgress
{
    public CDataAchievementIdentifier AchieveIdentifier { get; set; }
    public uint CurrentNum { get; set; }
    public uint Sequence { get; set; }
    public long CompleteDate { get; set; }

    public class Serializer : EntitySerializer<CDataAchievementProgress>
    {
        public override void Write(IBuffer buffer, CDataAchievementProgress obj)
        {
            WriteEntity(buffer, obj.AchieveIdentifier);
            WriteUInt32(buffer, obj.CurrentNum);
            WriteUInt32(buffer, obj.Sequence);
            WriteInt64(buffer, obj.CompleteDate);
        }

        public override CDataAchievementProgress Read(IBuffer buffer)
        {
            CDataAchievementProgress obj = new CDataAchievementProgress();
            obj.AchieveIdentifier = ReadEntity<CDataAchievementIdentifier>(buffer);
            obj.CurrentNum = ReadUInt32(buffer);
            obj.Sequence = ReadUInt32(buffer);
            obj.CompleteDate = ReadInt64(buffer);
            return obj;
        }
    }
}
