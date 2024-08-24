using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

/// <summary>
///     This must be sent directly after <see cref="C2SAchievementCompleteNtc" /> for any achievement that has rewards (furniture, crafting recipes).
///     It makes the client print to console in a regular green font the following: "[character-name] earned achievement "[achievement-name]"."
/// </summary>
public class C2SAchievementReceivableRewardNtc : IPacketStructure
{
    public PacketId Id => PacketId.S2C_ACHIEVEMENT_ACHIEVEMENT_RECEIVABLE_REWARD_NTC;

    public List<CDataAchieveRewardCommon> RewardList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<C2SAchievementReceivableRewardNtc>
    {
        public override void Write(IBuffer buffer, C2SAchievementReceivableRewardNtc obj)
        {
            WriteEntityList(buffer, obj.RewardList);
        }

        public override C2SAchievementReceivableRewardNtc Read(IBuffer buffer)
        {
            C2SAchievementReceivableRewardNtc obj = new C2SAchievementReceivableRewardNtc();

            obj.RewardList = ReadEntityList<CDataAchieveRewardCommon>(buffer);

            return obj;
        }
    }
}
