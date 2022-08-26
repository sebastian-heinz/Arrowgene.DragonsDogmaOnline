using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SQuestSendLeaderWaitOrderQuestListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_QUEST_SEND_LEADER_WAIT_ORDER_QUEST_LIST_REQ;

        public C2SQuestSendLeaderWaitOrderQuestListReq()
        {
            QuestScheduleIdList = new List<CDataCommonU32>();
        }
        public List<CDataCommonU32> QuestScheduleIdList { get; set; }

        public class Serializer : PacketEntitySerializer<C2SQuestSendLeaderWaitOrderQuestListReq>
        {
            public override void Write(IBuffer buffer, C2SQuestSendLeaderWaitOrderQuestListReq obj)
            {
                WriteEntityList<CDataCommonU32>(buffer, obj.QuestScheduleIdList);
            }

            public override C2SQuestSendLeaderWaitOrderQuestListReq Read(IBuffer buffer)
            {
                C2SQuestSendLeaderWaitOrderQuestListReq obj = new C2SQuestSendLeaderWaitOrderQuestListReq();
                obj.QuestScheduleIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}