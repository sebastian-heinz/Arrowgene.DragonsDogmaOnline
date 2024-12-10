using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSetQuestInfoList
    {
        public CDataSetQuestInfoList()
        {
            DiscoverRewardWalletPoint = new List<CDataWalletPoint>();
            DiscoverRewardExp = new List<CDataQuestExp>();
            SelectRewardItemIdList = new List<CDataCommonU32>();
            QuestEnemyInfoList = new List<CDataQuestEnemyInfo>();
            QuestLayoutFlagSetInfoList = new List<CDataQuestLayoutFlagSetInfo>();
            DeliveryItemList = new List<CDataDeliveryItem>();
            QuestOrderConditionParamList = new List<CDataQuestOrderConditionParam>();
        }

        public uint QuestScheduleId { get; set; }
        public uint QuestId { get; set; }
        public ulong OrderDate { get; set; }
        public ulong EndDistributionDate { get; set; }
        public uint ImageId { get; set; }
        public uint BaseLevel { get; set; }
        public ushort ContentJoinItemRank { get; set; }
        public uint DiscoverRewardItemId { get; set; }
        public List<CDataWalletPoint> DiscoverRewardWalletPoint { get; set; }
        public List<CDataQuestExp> DiscoverRewardExp { get; set; }
        public uint QuickPartyPopularity { get; set; }
        public List<CDataCommonU32> SelectRewardItemIdList { get; set; }
        public byte RandomRewardNum { get; set; }
        public byte ChargeRewardNum { get; set; }
        public ushort CompleteNum { get; set; }
        public bool IsDiscovery { get; set; }
        public List<CDataQuestEnemyInfo> QuestEnemyInfoList { get; set; }
        public List<CDataQuestLayoutFlagSetInfo> QuestLayoutFlagSetInfoList { get; set; }
        public List<CDataDeliveryItem> DeliveryItemList { get; set; }
        public List<CDataQuestOrderConditionParam> QuestOrderConditionParamList { get; set; }

        public class Serializer : EntitySerializer<CDataSetQuestInfoList>
        {
            public override void Write(IBuffer buffer, CDataSetQuestInfoList obj)
            {
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt64(buffer, obj.OrderDate);
                WriteUInt64(buffer, obj.EndDistributionDate);
                WriteUInt32(buffer, obj.ImageId);
                WriteUInt32(buffer, obj.BaseLevel);
                WriteUInt16(buffer, obj.ContentJoinItemRank);
                WriteUInt32(buffer, obj.DiscoverRewardItemId);
                WriteEntityList<CDataWalletPoint>(buffer, obj.DiscoverRewardWalletPoint);
                WriteEntityList<CDataQuestExp>(buffer, obj.DiscoverRewardExp);
                WriteUInt32(buffer, obj.QuickPartyPopularity);
                WriteEntityList<CDataCommonU32>(buffer, obj.SelectRewardItemIdList);
                WriteByte(buffer, obj.RandomRewardNum);
                WriteByte(buffer, obj.ChargeRewardNum);
                WriteUInt16(buffer, obj.CompleteNum);
                WriteBool(buffer, obj.IsDiscovery);
                WriteEntityList<CDataQuestEnemyInfo>(buffer, obj.QuestEnemyInfoList);
                WriteEntityList<CDataQuestLayoutFlagSetInfo>(buffer, obj.QuestLayoutFlagSetInfoList);
                WriteEntityList<CDataDeliveryItem>(buffer, obj.DeliveryItemList);
                WriteEntityList<CDataQuestOrderConditionParam>(buffer, obj.QuestOrderConditionParamList);
            }

            public override CDataSetQuestInfoList Read(IBuffer buffer)
            {
                CDataSetQuestInfoList obj = new CDataSetQuestInfoList();
                obj.QuestScheduleId = ReadUInt32(buffer);
                obj.QuestId = ReadUInt32(buffer);
                obj.OrderDate = ReadUInt64(buffer);
                obj.EndDistributionDate = ReadUInt64(buffer);
                obj.ImageId = ReadUInt32(buffer);
                obj.BaseLevel = ReadUInt32(buffer);
                obj.ContentJoinItemRank = ReadUInt16(buffer);
                obj.DiscoverRewardItemId = ReadUInt32(buffer);
                obj.DiscoverRewardWalletPoint = ReadEntityList<CDataWalletPoint>(buffer);
                obj.DiscoverRewardExp = ReadEntityList<CDataQuestExp>(buffer);
                obj.QuickPartyPopularity = ReadUInt32(buffer);
                obj.SelectRewardItemIdList = ReadEntityList<CDataCommonU32>(buffer);
                obj.RandomRewardNum = ReadByte(buffer);
                obj.ChargeRewardNum = ReadByte(buffer);
                obj.CompleteNum = ReadUInt16(buffer);
                obj.IsDiscovery = ReadBool(buffer);
                obj.QuestEnemyInfoList = ReadEntityList<CDataQuestEnemyInfo>(buffer);
                obj.QuestLayoutFlagSetInfoList = ReadEntityList<CDataQuestLayoutFlagSetInfo>(buffer);
                obj.DeliveryItemList = ReadEntityList<CDataDeliveryItem>(buffer);
                obj.QuestOrderConditionParamList = ReadEntityList<CDataQuestOrderConditionParam>(buffer);
                return obj;
            }
        }
    }
}
