using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestList
    {
        public CDataQuestList()
        {
            BaseWalletPoints = new List<CDataWalletPoint>();
            BaseExp = new List<CDataQuestExp>();
            FixedRewardItemList = new List<CDataRewardItem>();
            FixedRewardSelectItemList = new List<CDataRewardItem>();
            ContentsReleaseList = new List<CDataCharacterReleaseElement>();
            QuestOrderConditionParamList = new List<CDataQuestOrderConditionParam>();
            QuestAnnounceList = new List<CDataQuestAnnounce>();
            QuestTalkInfoList = new List<CDataQuestTalkInfo>();
            QuestFlagList = new List<CDataQuestFlag>();
            QuestLayoutFlagList = new List<CDataQuestLayoutFlag>();
            QuestProcessStateList = new List<CDataQuestProcessState>();
            QuestEnemyInfoList = new List<CDataQuestEnemyInfo>();
            QuestLayoutFlagSetInfoList = new List<CDataQuestLayoutFlagSetInfo>();
            DeliveryItemList = new List<CDataDeliveryItem>();
        }

        public UInt32 KeyId { get; set; }
        public UInt32 QuestScheduleId { get; set; }
        public UInt32 QuestId { get; set; }
        public UInt32 BaseLevel {  get; set; }
        public UInt16 ContentJoinItemRank {  get; set; }
        public List<CDataWalletPoint> BaseWalletPoints { get; set; }
        public List<CDataQuestExp> BaseExp {  get; set; }
        public UInt32 OrderNpcId { get; set; }
        public UInt32 NameMsgId {  get; set; }
        public UInt32 DetailMsgId { get; set; }
        public UInt64 DistributionStartDate {  get; set; } // OrderDate?
        public UInt64 DistributionEndDate {  get; set; }
        public List<CDataRewardItem> FixedRewardItemList { get; set; }
        public List<CDataRewardItem> FixedRewardSelectItemList { get; set; }
        public List<CDataCharacterReleaseElement> ContentsReleaseList { get; set; }
        public List<CDataQuestOrderConditionParam> QuestOrderConditionParamList { get; set; }
        public List<CDataQuestAnnounce> QuestAnnounceList { get; set; }
        public List<CDataQuestTalkInfo> QuestTalkInfoList { get; set; }
        public List<CDataQuestFlag> QuestFlagList { get; set; }
        public List<CDataQuestLayoutFlag> QuestLayoutFlagList { get; set; }
        public List<CDataQuestProcessState> QuestProcessStateList { get; set; }
        public List<CDataQuestEnemyInfo> QuestEnemyInfoList { get; set; }
        public List<CDataQuestLayoutFlagSetInfo> QuestLayoutFlagSetInfoList { get; set; }
        public List<CDataDeliveryItem> DeliveryItemList { get; set; }
        public bool IsClientOrder {  get; set; }

        public class Serializer : EntitySerializer<CDataQuestList>
        {
            public override void Write(IBuffer buffer, CDataQuestList obj)
            {
                WriteUInt32(buffer, obj.KeyId);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt32(buffer, obj.BaseLevel);
                WriteUInt16(buffer, obj.ContentJoinItemRank);
                WriteEntityList<CDataWalletPoint>(buffer, obj.BaseWalletPoints);
                WriteEntityList<CDataQuestExp>(buffer, obj.BaseExp);
                WriteUInt32(buffer, obj.OrderNpcId);
                WriteUInt32(buffer, obj.NameMsgId);
                WriteUInt32(buffer, obj.DetailMsgId);
                WriteUInt64(buffer, obj.DistributionStartDate);
                WriteUInt64(buffer, obj.DistributionEndDate);
                WriteEntityList<CDataRewardItem>(buffer, obj.FixedRewardItemList);
                WriteEntityList<CDataRewardItem>(buffer, obj.FixedRewardSelectItemList);
                WriteEntityList<CDataCharacterReleaseElement>(buffer, obj.ContentsReleaseList);
                WriteEntityList<CDataQuestOrderConditionParam>(buffer, obj.QuestOrderConditionParamList);
                // CQuestLog
                WriteEntityList<CDataQuestAnnounce>(buffer, obj.QuestAnnounceList);
                WriteEntityList<CDataQuestTalkInfo>(buffer, obj.QuestTalkInfoList);
                WriteEntityList<CDataQuestFlag>(buffer, obj.QuestFlagList);
                WriteEntityList<CDataQuestLayoutFlag>(buffer, obj.QuestLayoutFlagList);
                WriteEntityList<CDataQuestProcessState>(buffer, obj.QuestProcessStateList);
                WriteEntityList<CDataQuestEnemyInfo>(buffer, obj.QuestEnemyInfoList);
                WriteEntityList<CDataQuestLayoutFlagSetInfo>(buffer, obj.QuestLayoutFlagSetInfoList);
                WriteEntityList<CDataDeliveryItem>(buffer, obj.DeliveryItemList);
                WriteBool(buffer, obj.IsClientOrder);
            }

            public override CDataQuestList Read(IBuffer buffer)
            {
                CDataQuestList obj = new CDataQuestList();
                obj.KeyId = ReadUInt32(buffer);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.BaseLevel = ReadUInt32(buffer);
                obj.ContentJoinItemRank = ReadUInt16(buffer);
                obj.BaseWalletPoints = ReadEntityList<CDataWalletPoint>(buffer);
                obj.BaseExp = ReadEntityList<CDataQuestExp>(buffer);
                obj.OrderNpcId = ReadUInt32(buffer);
                obj.NameMsgId = ReadUInt32(buffer);
                obj.DetailMsgId = ReadUInt32(buffer);
                obj.DistributionStartDate = ReadUInt64(buffer);
                obj.DistributionEndDate = ReadUInt64(buffer);
                obj.FixedRewardItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.FixedRewardSelectItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.ContentsReleaseList = ReadEntityList<CDataCharacterReleaseElement>(buffer);
                obj.QuestOrderConditionParamList = ReadEntityList<CDataQuestOrderConditionParam>(buffer);
                obj.QuestAnnounceList = ReadEntityList<CDataQuestAnnounce>(buffer);
                obj.QuestTalkInfoList = ReadEntityList<CDataQuestTalkInfo>(buffer);
                obj.QuestFlagList = ReadEntityList<CDataQuestFlag>(buffer);
                obj.QuestLayoutFlagList = ReadEntityList<CDataQuestLayoutFlag>(buffer);
                obj.QuestProcessStateList = ReadEntityList<CDataQuestProcessState>(buffer);
                obj.QuestEnemyInfoList = ReadEntityList<CDataQuestEnemyInfo>(buffer);
                obj.QuestLayoutFlagSetInfoList = ReadEntityList<CDataQuestLayoutFlagSetInfo>(buffer);
                obj.DeliveryItemList = ReadEntityList<CDataDeliveryItem>(buffer);
                obj.IsClientOrder = ReadBool(buffer);
                return obj;
            }
        }
    }
}
