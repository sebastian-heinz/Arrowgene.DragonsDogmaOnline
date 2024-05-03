using Arrowgene.Buffers;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMainQuest
    {
        public CDataMainQuest()
        {
            BaseWalletPoints = new List<CDataWalletPoint>();
            BaseExp = new List<CDataQuestExp>();
            FixedRewardItemList = new List<CDataRewardItem>();
            FixedRewardSelectItemList = new List<CDataRewardItem>();
            Unk3 = new List<CDataCommonU32>();
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
        public UInt32 Unk0 { get; set; } // OrderNpcId?
        public UInt32 Unk1 {  get; set; } // NameMsgId?
        public UInt32 Unk2 { get; set; } // DetailMsgId?
        public UInt64 DistributionStartDate {  get; set; }
        public UInt64 DistributionEndDate {  get; set; }
        public List<CDataRewardItem> FixedRewardItemList { get; set; }
        public List<CDataRewardItem> FixedRewardSelectItemList { get; set; }
        public List<CDataCommonU32> Unk3 { get; set; }
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

        public class Serializer : EntitySerializer<CDataMainQuest>
        {
            public override void Write(IBuffer buffer, CDataMainQuest obj)
            {
                WriteUInt32(buffer, obj.KeyId);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt32(buffer, obj.BaseLevel);
                WriteUInt16(buffer, obj.ContentJoinItemRank);
                WriteEntityList<CDataWalletPoint>(buffer, obj.BaseWalletPoints);
                WriteEntityList<CDataQuestExp>(buffer, obj.BaseExp);
                WriteUInt32(buffer, obj.Unk0);
                WriteUInt32(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt64(buffer, obj.DistributionStartDate);
                WriteUInt64(buffer, obj.DistributionEndDate);
                WriteEntityList<CDataRewardItem>(buffer, obj.FixedRewardItemList);
                WriteEntityList<CDataRewardItem>(buffer, obj.FixedRewardSelectItemList);
                WriteEntityList<CDataCommonU32>(buffer, obj.Unk3);
                WriteEntityList<CDataQuestOrderConditionParam>(buffer, obj.QuestOrderConditionParamList);
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

            public override CDataMainQuest Read(IBuffer buffer)
            {
                CDataMainQuest obj = new CDataMainQuest();
                obj.KeyId = ReadUInt32(buffer);
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.BaseLevel = ReadUInt32(buffer);
                obj.ContentJoinItemRank = ReadUInt16(buffer);
                obj.BaseWalletPoints = ReadEntityList<CDataWalletPoint>(buffer);
                obj.BaseExp = ReadEntityList<CDataQuestExp>(buffer);
                obj.Unk0 = ReadUInt32(buffer);
                obj.Unk1 = ReadUInt32(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.DistributionStartDate = ReadUInt64(buffer);
                obj.DistributionEndDate = ReadUInt64(buffer);
                obj.FixedRewardItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.FixedRewardSelectItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.Unk3 = ReadEntityList<CDataCommonU32>(buffer);
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
