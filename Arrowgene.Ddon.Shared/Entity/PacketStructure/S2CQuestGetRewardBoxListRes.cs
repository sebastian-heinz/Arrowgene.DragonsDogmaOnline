using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetRewardBoxListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_REWARD_BOX_LIST_RES;

        public S2CQuestGetRewardBoxListRes()
        {
            RewardBoxRecordList = new List<CDataRewardBoxRecord>();
        }

        public List<CDataRewardBoxRecord> RewardBoxRecordList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestGetRewardBoxListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetRewardBoxListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataRewardBoxRecord>(buffer, obj.RewardBoxRecordList);
            }

            public override S2CQuestGetRewardBoxListRes Read(IBuffer buffer)
            {
                S2CQuestGetRewardBoxListRes obj = new S2CQuestGetRewardBoxListRes();
                ReadServerResponse(buffer, obj);
                obj.RewardBoxRecordList = ReadEntityList<CDataRewardBoxRecord>(buffer);
                return obj;
            }
        }
    }
}
