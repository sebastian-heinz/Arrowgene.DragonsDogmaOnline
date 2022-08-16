using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestSendLeaderQuestOrderConditionInfoReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_SEND_LEADER_QUEST_ORDER_CONDITION_INFO_REQ;

        public C2SQuestSendLeaderQuestOrderConditionInfoReq()
        {
            OrderConditionInfoList = new List<CDataOrderConditionInfo>();
        }

        public List<CDataOrderConditionInfo> OrderConditionInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestSendLeaderQuestOrderConditionInfoReq>
        {
            public override void Write(IBuffer buffer, C2SQuestSendLeaderQuestOrderConditionInfoReq obj)
            {
                WriteEntityList<CDataOrderConditionInfo>(buffer, obj.OrderConditionInfoList);
            }

            public override C2SQuestSendLeaderQuestOrderConditionInfoReq Read(IBuffer buffer)
            {
                C2SQuestSendLeaderQuestOrderConditionInfoReq obj = new C2SQuestSendLeaderQuestOrderConditionInfoReq();
                obj.OrderConditionInfoList = ReadEntityList<CDataOrderConditionInfo>(buffer);
                return obj;
            }
        }
    }
}