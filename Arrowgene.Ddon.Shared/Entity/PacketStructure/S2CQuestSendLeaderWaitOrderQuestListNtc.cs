using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestSendLeaderWaitOrderQuestListNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_SEND_LEADER_WAIT_ORDER_QUEST_LIST_NTC;

        public S2CQuestSendLeaderWaitOrderQuestListNtc()
        {
            QuestScheduleIdList = new List<CDataCommonU32>();
        }

        public List<CDataCommonU32> QuestScheduleIdList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestSendLeaderWaitOrderQuestListNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestSendLeaderWaitOrderQuestListNtc obj)
            {
                WriteEntityList<CDataCommonU32>(buffer, obj.QuestScheduleIdList);
            }

            public override S2CQuestSendLeaderWaitOrderQuestListNtc Read(IBuffer buffer)
            {
                S2CQuestSendLeaderWaitOrderQuestListNtc obj = new S2CQuestSendLeaderWaitOrderQuestListNtc();
                obj.QuestScheduleIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}