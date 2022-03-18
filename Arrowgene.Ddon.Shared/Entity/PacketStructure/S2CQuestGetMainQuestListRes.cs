using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestGetMainQuestListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_QUEST_GET_MAIN_QUEST_LIST_RES;

        public S2CQuestGetMainQuestListRes()
        {
            FixedRewardItemList = new List<CDataRewardItem>();
            FixedRewardSelectItemList = new List<CDataRewardItem>();
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

        public uint KeyId { get; set; }
        public uint QuestScheduleId { get; set; }
        public uint QuestId { get; set; }
        public uint BaseLevel { get; set; }
        public ushort ContentJoinItemRank { get; set; }
        public uint BaseGold { get; set; }
        public uint BaseExp { get; set; }
        public uint BaseRim { get; set; }
        public uint OrderNpcId { get; set; }
        public uint NameMsgId { get; set; }
        public uint DetailMsgId { get; set; }
        public ulong EndDistributionDate { get; set; }
        public List<CDataRewardItem> FixedRewardItemList { get; set; }
        public List<CDataRewardItem> FixedRewardSelectItemList { get; set; }
        public List<CDataQuestOrderConditionParam> QuestOrderConditionParamList { get; set; }
        public List<CDataQuestAnnounce> QuestAnnounceList { get; set; }
        public List<CDataQuestTalkInfo> QuestTalkInfoList { get; set; }
        public List<CDataQuestFlag> QuestFlagList { get; set; }
        public List<CDataQuestLayoutFlag> QuestLayoutFlagList { get; set; }
        public List<CDataQuestProcessState> QuestProcessStateList { get; set; }
        public List<CDataQuestEnemyInfo> QuestEnemyInfoList { get; set; }
        public List<CDataQuestLayoutFlagSetInfo> QuestLayoutFlagSetInfoList { get; set; }
        public List<CDataDeliveryItem> DeliveryItemList { get; set; }


        public class Serializer : PacketEntitySerializer<S2CQuestGetMainQuestListRes>
        {
            public override void Write(IBuffer buffer, S2CQuestGetMainQuestListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.KeyId);
                WriteUInt32(buffer, obj.QuestScheduleId);
                WriteUInt32(buffer, obj.QuestId);
                WriteUInt32(buffer, obj.BaseLevel);
                WriteUInt16(buffer, obj.ContentJoinItemRank);
                WriteUInt32(buffer, obj.BaseGold);
                WriteUInt32(buffer, obj.BaseExp);
                WriteUInt32(buffer, obj.BaseRim);
                WriteUInt32(buffer, obj.OrderNpcId);
                WriteUInt32(buffer, obj.NameMsgId);
                WriteUInt32(buffer, obj.DetailMsgId);
                WriteUInt64(buffer, obj.EndDistributionDate);
                WriteEntityList<CDataRewardItem>(buffer, obj.FixedRewardItemList);
                WriteEntityList<CDataRewardItem>(buffer, obj.FixedRewardSelectItemList);
                WriteEntityList<CDataQuestOrderConditionParam>(buffer, obj.QuestOrderConditionParamList);
                WriteEntityList<CDataQuestAnnounce>(buffer, obj.QuestAnnounceList);
                WriteEntityList<CDataQuestTalkInfo>(buffer, obj.QuestTalkInfoList);
                WriteEntityList<CDataQuestFlag>(buffer, obj.QuestFlagList);
                WriteEntityList<CDataQuestLayoutFlag>(buffer, obj.QuestLayoutFlagList);
                WriteEntityList<CDataQuestProcessState>(buffer, obj.QuestProcessStateList);
                WriteEntityList<CDataQuestEnemyInfo>(buffer, obj.QuestEnemyInfoList);
                WriteEntityList<CDataQuestLayoutFlagSetInfo>(buffer, obj.QuestLayoutFlagSetInfoList);
                WriteEntityList<CDataDeliveryItem>(buffer, obj.DeliveryItemList);
            }

            public override S2CQuestGetMainQuestListRes Read(IBuffer buffer)
            {
                S2CQuestGetMainQuestListRes obj = new S2CQuestGetMainQuestListRes();
                ReadServerResponse(buffer, obj);
                obj.KeyId = buffer.ReadUInt32(Endianness.Big);
                obj.QuestScheduleId = buffer.ReadUInt32(Endianness.Big);
                obj.QuestId = buffer.ReadUInt32(Endianness.Big);
                obj.BaseLevel = buffer.ReadUInt32(Endianness.Big);
                obj.ContentJoinItemRank = buffer.ReadUInt16(Endianness.Big);
                obj.BaseGold = buffer.ReadUInt32(Endianness.Big);
                obj.BaseExp = buffer.ReadUInt32(Endianness.Big);
                obj.BaseRim = buffer.ReadUInt32(Endianness.Big);
                obj.OrderNpcId = buffer.ReadUInt32(Endianness.Big);
                obj.NameMsgId = buffer.ReadUInt32(Endianness.Big);
                obj.DetailMsgId = buffer.ReadUInt32(Endianness.Big);
                obj.EndDistributionDate = buffer.ReadUInt64(Endianness.Big);
                obj.FixedRewardItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.FixedRewardSelectItemList = ReadEntityList<CDataRewardItem>(buffer);
                obj.QuestOrderConditionParamList = ReadEntityList<CDataQuestOrderConditionParam>(buffer);
                obj.QuestAnnounceList = ReadEntityList<CDataQuestAnnounce>(buffer);
                obj.QuestTalkInfoList = ReadEntityList<CDataQuestTalkInfo>(buffer);
                obj.QuestFlagList = ReadEntityList<CDataQuestFlag>(buffer);
                obj.QuestLayoutFlagList = ReadEntityList<CDataQuestLayoutFlag>(buffer);
                obj.QuestProcessStateList = ReadEntityList<CDataQuestProcessState>(buffer);
                obj.QuestEnemyInfoList = ReadEntityList<CDataQuestEnemyInfo>(buffer);
                obj.QuestLayoutFlagSetInfoList = ReadEntityList<CDataQuestLayoutFlagSetInfo>(buffer);
                obj.DeliveryItemList = ReadEntityList<CDataDeliveryItem>(buffer);
                return obj;
            }
        }
    }
}
