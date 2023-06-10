using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataQuestList
    {
        public CDataQuestList() {
            Unk0 = new List<CDataWalletPoint>();
            Unk1 = new List<CDataQuestListUnk1>();
            FixedRewardItemList = new List<CDataRewardItem>();
            FixedRewardSelectItemList = new List<CDataRewardItem>();
            Unk7 = new List<CDataQuestListUnk7>();
            QuestOrderConditionParamList = new List<CDataQuestOrderConditionParam>();
            QuestLog = new CDataQuestLog();
            QuestFlagList = new List<CDataQuestFlag>();
            QuestLayoutFlagList = new List<CDataQuestLayoutFlag>();
            QuestProcessStateList = new List<CDataQuestProcessState>();
            QuestEnemyInfoList = new List<CDataQuestEnemyInfo>();
            QuestLayoutFlagSetInfoList = new List<CDataQuestLayoutFlagSetInfo>();
            DeliveryItemList = new List<CDataDeliveryItem>();
        }
    
        public uint KeyId { get; set; }
        public uint QuestScheduleId { get; set; }
        public uint QuestId { get; set; }
        public uint BaseLevel { get; set; }
        public ushort ContentJoinItemRank { get; set; }
        public List<CDataWalletPoint> Unk0 { get; set; }
        public List<CDataQuestListUnk1> Unk1 { get; set; }
        public uint Unk2 { get; set;}
        public uint Unk3 { get; set;}
        public uint Unk4 { get; set;}
        public ulong Unk5 { get; set; } // EndDistributionDate?
        public ulong Unk6 { get; set; } // EndDistributionDate?
        public List<CDataRewardItem> FixedRewardItemList;
        public List<CDataRewardItem> FixedRewardSelectItemList;
        public List<CDataQuestListUnk7> Unk7;
        public List<CDataQuestOrderConditionParam> QuestOrderConditionParamList;
        public CDataQuestLog QuestLog { get; set; }
        public List<CDataQuestFlag> QuestFlagList;
        public List<CDataQuestLayoutFlag> QuestLayoutFlagList;
        public List<CDataQuestProcessState> QuestProcessStateList;
        public List<CDataQuestEnemyInfo> QuestEnemyInfoList;
        public List<CDataQuestLayoutFlagSetInfo> QuestLayoutFlagSetInfoList;
        public List<CDataDeliveryItem> DeliveryItemList;
        public bool IsClientOrder;

        public class Serializer : EntitySerializer<CDataQuestList>
        {
            public override void Write(IBuffer buffer, CDataQuestList obj)
            {
                WriteUInt32(buffer, obj.KeyId);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt32(buffer, obj.BaseLevel);
                WriteUInt16(buffer, obj.ContentJoinItemRank);
                WriteEntityList<CDataWalletPoint>(buffer, obj.Unk0);
                WriteEntityList<CDataQuestListUnk1>(buffer, obj.Unk1);
                WriteUInt32(buffer, obj.Unk2);
                WriteUInt32(buffer, obj.Unk3);
                WriteUInt32(buffer, obj.Unk4);
                WriteUInt64(buffer, obj.Unk5);
                WriteUInt64(buffer, obj.Unk6);
                WriteEntityList<CDataRewardItem>(buffer, obj.FixedRewardItemList);
                WriteEntityList<CDataRewardItem>(buffer, obj.FixedRewardSelectItemList);
                WriteEntityList<CDataQuestListUnk7>(buffer, obj.Unk7);
                WriteEntityList<CDataQuestOrderConditionParam>(buffer, obj.QuestOrderConditionParamList);
                WriteEntity<CDataQuestLog>(buffer, obj.QuestLog);
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
                obj.Unk0 = ReadEntityList<CDataWalletPoint>(buffer);
                obj.Unk1 = ReadEntityList<CDataQuestListUnk1>(buffer);
                obj.Unk2 = ReadUInt32(buffer);
                obj.Unk3 = ReadUInt32(buffer);
                obj.Unk4 = ReadUInt32(buffer);
                obj.Unk5 = ReadUInt64(buffer);
                obj.Unk6 = ReadUInt64(buffer);
                obj.FixedRewardItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.FixedRewardSelectItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.Unk7 = ReadEntityList<CDataQuestListUnk7>(buffer);
                obj.QuestOrderConditionParamList = ReadEntityList<CDataQuestOrderConditionParam>(buffer);
                obj.QuestLog = ReadEntity<CDataQuestLog>(buffer);
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