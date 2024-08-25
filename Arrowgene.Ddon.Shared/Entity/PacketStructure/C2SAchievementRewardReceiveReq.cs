using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure;

public class C2SAchievementRewardReceiveReq : IPacketStructure
{
    public PacketId Id => PacketId.C2S_ACHIEVEMENT_ACHIEVEMENT_REWARD_RECEIVE_REQ;

    /// <summary>
    ///     Filled by client, corresponds to the reward selected in the UI and part of GetReceivableRewardListRes
    /// </summary>
    public List<CDataAchieveRewardCommon> RewardList { get; set; } = new();

    public class Serializer : PacketEntitySerializer<C2SAchievementRewardReceiveReq>
    {
        public override void Write(IBuffer buffer, C2SAchievementRewardReceiveReq obj)
        {
            WriteEntityList(buffer, obj.RewardList);
        }

        public override C2SAchievementRewardReceiveReq Read(IBuffer buffer)
        {
            C2SAchievementRewardReceiveReq obj = new C2SAchievementRewardReceiveReq();

            obj.RewardList = ReadEntityList<CDataAchieveRewardCommon>(buffer);

            return obj;
        }
    }
}
