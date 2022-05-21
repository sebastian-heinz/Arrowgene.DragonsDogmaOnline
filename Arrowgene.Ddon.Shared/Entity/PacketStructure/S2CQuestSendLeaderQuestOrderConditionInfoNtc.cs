using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSendLeaderQuestOrderConditionInfoNtc : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_SEND_LEADER_QUEST_ORDER_CONDITION_INFO_NTC;

        public S2CQuestSendLeaderQuestOrderConditionInfoNtc()
        {
            OrderConditionInfoList = new List<CDataOrderConditionInfo>();
        }

        public List<CDataOrderConditionInfo> OrderConditionInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestSendLeaderQuestOrderConditionInfoNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestSendLeaderQuestOrderConditionInfoNtc obj)
            {
                WriteEntityList<CDataOrderConditionInfo>(buffer, obj.OrderConditionInfoList);
            }

            public override S2CQuestSendLeaderQuestOrderConditionInfoNtc Read(IBuffer buffer)
            {
                S2CQuestSendLeaderQuestOrderConditionInfoNtc obj = new S2CQuestSendLeaderQuestOrderConditionInfoNtc();
                obj.OrderConditionInfoList = ReadEntityList<CDataOrderConditionInfo>(buffer);
                return obj;
            }
        }
    }
}