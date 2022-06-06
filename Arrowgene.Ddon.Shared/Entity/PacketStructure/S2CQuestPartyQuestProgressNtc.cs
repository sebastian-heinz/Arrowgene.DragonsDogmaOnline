using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestPartyQuestProgressNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_PARTY_QUEST_PROGRESS_NTC;

        public S2CQuestPartyQuestProgressNtc()
        {
            ProgressCharacterId = 0;
            QuestOrderList = new List<CDataQuestOrderList>();
            SoloQuestOrderList = new List<CDataQuestIdScheduleId>();
            DeliveredItemRecordList = new List<CDataDeliveredItemRecord>();
            QuestKeyItemPointRecordList = new List<CDataQuestKeyItemPointRecord>();
        }

        public uint ProgressCharacterId { get; set; }
        public List<CDataQuestOrderList> QuestOrderList { get; set; }
        public List<CDataQuestIdScheduleId> SoloQuestOrderList { get; set; }
        public List<CDataDeliveredItemRecord> DeliveredItemRecordList { get; set; }
        public List<CDataQuestKeyItemPointRecord> QuestKeyItemPointRecordList { get; set; }


        public class Serializer : PacketEntitySerializer<S2CQuestPartyQuestProgressNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestPartyQuestProgressNtc obj)
            {
                WriteUInt32(buffer, obj.ProgressCharacterId);
                WriteEntityList<CDataQuestOrderList>(buffer, obj.QuestOrderList);
                WriteEntityList<CDataQuestIdScheduleId>(buffer, obj.SoloQuestOrderList);
                WriteEntityList<CDataDeliveredItemRecord>(buffer, obj.DeliveredItemRecordList);
                WriteEntityList<CDataQuestKeyItemPointRecord>(buffer, obj.QuestKeyItemPointRecordList);
            }

            public override S2CQuestPartyQuestProgressNtc Read(IBuffer buffer)
            {
                S2CQuestPartyQuestProgressNtc obj = new S2CQuestPartyQuestProgressNtc();
                obj.ProgressCharacterId = ReadUInt32(buffer);
                obj.QuestOrderList = ReadEntityList<CDataQuestOrderList>(buffer);
                obj.SoloQuestOrderList = ReadEntityList<CDataQuestIdScheduleId>(buffer);
                obj.DeliveredItemRecordList = ReadEntityList<CDataDeliveredItemRecord>(buffer);
                obj.QuestKeyItemPointRecordList = ReadEntityList<CDataQuestKeyItemPointRecord>(buffer);
                return obj;
            }
        }
    }
}