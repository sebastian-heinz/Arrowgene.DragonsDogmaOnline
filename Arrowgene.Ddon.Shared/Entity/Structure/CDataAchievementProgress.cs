using Arrowgene.Buffers;
using System;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataAchievementProgress
{
    public CDataAchievementIdentifier AchieveIdentifier { get; set; } = new();
    /// <summary>
    /// Current number of times the objective has been completed, used by client to calculate bar lengths and show completion rate.
    /// </summary>
    public uint CurrentNum { get; set; }
    /// <summary>
    /// Unknown what this is used for, in packet dumps this is always 0.
    /// </summary>
    public uint Sequence { get; set; }
    /// <summary>
    /// Any given date in the past works for the UI to show it as complete.
    /// </summary>
    public DateTimeOffset CompleteDate { get; set; }

    public class Serializer : EntitySerializer<CDataAchievementProgress>
    {
        public override void Write(IBuffer buffer, CDataAchievementProgress obj)
        {
            WriteEntity(buffer, obj.AchieveIdentifier);
            WriteUInt32(buffer, obj.CurrentNum);
            WriteUInt32(buffer, obj.Sequence);
            WriteInt64(buffer, obj.CompleteDate.ToUnixTimeSeconds());
        }

        public override CDataAchievementProgress Read(IBuffer buffer)
        {
            CDataAchievementProgress obj = new CDataAchievementProgress();
            obj.AchieveIdentifier = ReadEntity<CDataAchievementIdentifier>(buffer);
            obj.CurrentNum = ReadUInt32(buffer);
            obj.Sequence = ReadUInt32(buffer);
            obj.CompleteDate = DateTimeOffset.FromUnixTimeSeconds(ReadInt64(buffer));
            return obj;
        }
    }
}
