using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetPartyQuestProgressInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_PARTY_QUEST_PROGRESS_INFO_RES;

        public S2CQuestGetPartyQuestProgressInfoRes()
        {
            QuestOrderList = new List<CDataQuestOrderList>();
            SoloQuestOrderList = new List<CDataQuestIdScheduleId>();
            DeliveredItemRecordList = new List<CDataDeliveredItemRecord>();
            QuestKeyItemPointRecordList = new List<CDataQuestKeyItemPointRecord>();
        }

        public List<CDataQuestOrderList> QuestOrderList { get; set; }
        public List<CDataQuestIdScheduleId> SoloQuestOrderList { get; set; }
        public List<CDataDeliveredItemRecord> DeliveredItemRecordList { get; set; }
        public List<CDataQuestKeyItemPointRecord> QuestKeyItemPointRecordList { get; set; }


        public class Serializer : PacketEntitySerializer<S2CQuestGetPartyQuestProgressInfoRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetPartyQuestProgressInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList<CDataQuestOrderList>(buffer, obj.QuestOrderList);
                WriteEntityList<CDataQuestIdScheduleId>(buffer, obj.SoloQuestOrderList);
                WriteEntityList<CDataDeliveredItemRecord>(buffer, obj.DeliveredItemRecordList);
                WriteEntityList<CDataQuestKeyItemPointRecord>(buffer, obj.QuestKeyItemPointRecordList);
            }

            public override S2CQuestGetPartyQuestProgressInfoRes Read(IBuffer buffer)
            {
                S2CQuestGetPartyQuestProgressInfoRes obj = new S2CQuestGetPartyQuestProgressInfoRes();
                ReadServerResponse(buffer, obj);
                obj.QuestOrderList = ReadEntityList<CDataQuestOrderList>(buffer);
                obj.SoloQuestOrderList = ReadEntityList<CDataQuestIdScheduleId>(buffer);
                obj.DeliveredItemRecordList = ReadEntityList<CDataDeliveredItemRecord>(buffer);
                obj.QuestKeyItemPointRecordList = ReadEntityList<CDataQuestKeyItemPointRecord>(buffer);
                return obj;
            }
        }
    }
}
