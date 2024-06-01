using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataPartyQuestProgressInfo
    {
        // CDataPartyContextPlayer
        public CDataPartyQuestProgressInfo()
        {
            QuestOrderList = new List<CDataQuestOrderList>();
            SoloQuestOrderList = new List<CDataQuestIdScheduleId>();
            DeliveredItemRecordList = new List<CDataDeliveredItemRecord>();
            QuestKeyItemPointRecordList = new List<CDataQuestKeyItemPointRecord>();
        }

        public List<CDataQuestOrderList> QuestOrderList {  get; set; }
        public List<CDataQuestIdScheduleId> SoloQuestOrderList {  get; set; }
        public List<CDataDeliveredItemRecord> DeliveredItemRecordList {  get; set; }
        public List<CDataQuestKeyItemPointRecord> QuestKeyItemPointRecordList {  get; set; }

        public class Serializer : EntitySerializer<CDataPartyQuestProgressInfo>
        {
            public override void Write(IBuffer buffer, CDataPartyQuestProgressInfo obj)
            {
                WriteEntityList<CDataQuestOrderList>(buffer, obj.QuestOrderList);
                WriteEntityList<CDataQuestIdScheduleId>(buffer, obj.SoloQuestOrderList);
                WriteEntityList<CDataDeliveredItemRecord>(buffer, obj.DeliveredItemRecordList);
                WriteEntityList<CDataQuestKeyItemPointRecord>(buffer, obj.QuestKeyItemPointRecordList);
            }

            public override CDataPartyQuestProgressInfo Read(IBuffer buffer)
            {
                CDataPartyQuestProgressInfo obj = new CDataPartyQuestProgressInfo();
                obj.QuestOrderList = ReadEntityList<CDataQuestOrderList>(buffer);
                obj.SoloQuestOrderList = ReadEntityList<CDataQuestIdScheduleId>(buffer);
                obj.DeliveredItemRecordList = ReadEntityList<CDataDeliveredItemRecord>(buffer);
                obj.QuestKeyItemPointRecordList = ReadEntityList<CDataQuestKeyItemPointRecord>(buffer);
                return obj;
            }
        }
    }
}
