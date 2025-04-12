using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

/// <summary>
///     This must be sent directly after <see cref="S2CAchievementCompleteNtc" /> for any achievement that has rewards (furniture, crafting recipes).
///     It makes the client print to console in a regular green font the following: "[character-name] earned achievement "[achievement-name]"."
/// </summary>
public class S2CAchievementReceivableRewardNtc : IPacketStructure
{
    public PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_RECEIVABLE_REWARD_NTC;

    public List<CDataAchieveRewardCommon> RewardList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<S2CAchievementReceivableRewardNtc>
    {
        public override void Write(IBuffer buffer, S2CAchievementReceivableRewardNtc obj)
        {
            WriteEntityList(buffer, obj.RewardList);
        }

        public override S2CAchievementReceivableRewardNtc Read(IBuffer buffer)
        {
            S2CAchievementReceivableRewardNtc obj = new S2CAchievementReceivableRewardNtc();

            obj.RewardList = ReadEntityList<CDataAchieveRewardCommon>(buffer);

            return obj;
        }
    }
}
