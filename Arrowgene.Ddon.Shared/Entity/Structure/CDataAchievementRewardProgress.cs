using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

/// <summary>
///     Shown in the UI in Achievement -> Reward -> Backgrounds
///     Unlockable backgrounds for Arisen profile
///     Rewarded for amount of overall number of received achievements
/// </summary>
public class CDataAchievementRewardProgress
{
    /// <summary>
    ///     Related to uGUIAchievement/ui/00_message/ui/arisencard_background_name.gmd, i.e. ARISENCARD_BACKGROUND_NAME_53 for
    ///     "Cubes" == RewardId 53
    /// </summary>
    public uint RewardId { get; set; }

    /// <summary>
    ///     How many achievements have been awarded to the player, if CurrentNum == TargetNum the item becomes Receivable in
    ///     the UI
    /// </summary>
    public uint CurrentNum { get; set; }

    /// <summary>
    ///     Target number of achievements the player must achieve to receive the reward
    /// </summary>
    public uint TargetNum { get; set; }

    /// <summary>
    ///     If CurrentNum == TargetNum && IsReceived is false, it becomes receivable in the UI
    /// </summary>
    public bool IsReceived { get; set; }

    public class Serializer : EntitySerializer<CDataAchievementRewardProgress>
    {
        public override void Write(IBuffer buffer, CDataAchievementRewardProgress obj)
        {
            WriteUInt32(buffer, obj.RewardId);
            WriteUInt32(buffer, obj.CurrentNum);
            WriteUInt32(buffer, obj.TargetNum);
            WriteBool(buffer, obj.IsReceived);
        }

        public override CDataAchievementRewardProgress Read(IBuffer buffer)
        {
            CDataAchievementRewardProgress obj = new CDataAchievementRewardProgress();
            obj.RewardId = ReadUInt32(buffer);
            obj.CurrentNum = ReadUInt32(buffer);
            obj.TargetNum = ReadUInt32(buffer);
            obj.IsReceived = ReadBool(buffer);
            return obj;
        }
    }
}
