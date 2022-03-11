using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestOrderList
{
    public CDataQuestOrderList()
    {
        FixedRewardItem = new List<CDataRewardItem>();
        FixedRewardSelectItem = new List<CDataRewardItem>();
        QuestAnnounce = new List<CDataQuestAnnounce>();
        QuestTalkInfo = new List<CDataQuestTalkInfo>();
        QuestFlag = new List<CDataQuestFlag>();
        QuestLayoutFlag = new List<CDataQuestLayoutFlag>();
        QuestProcessState = new List<CDataQuestProcessState>();
        QuestOrderConditionParam = new List<CDataQuestOrderConditionParam>();
        QuestEnemyInfo = new List<CDataQuestEnemyInfo>();
        QuestLayoutFlagSetInfo = new List<CDataQuestLayoutFlagSetInfo>();
    }

    public uint KeyId { get; set; }
    public uint QuestScheduleId { get; set; }
    public uint QuestId { get; set; }
    public uint AreaId { get; set; }
    public uint BaseLevel { get; set; }
    public ushort ContentJoinItemRank { get; set; }
    public uint BaseGold { get; set; }
    public uint BaseExp { get; set; }
    public uint BaseRim { get; set; }
    public uint OrderNpcId { get; set; }
    public uint NameMsgId { get; set; }
    public uint DetailMsgId { get; set; }
    public ulong OrderDate { get; set; }
    public ulong EndDistributionDate { get; set; }
    public List<CDataRewardItem> FixedRewardItem { get; set; }
    public List<CDataRewardItem> FixedRewardSelectItem { get; set; }
    public List<CDataQuestAnnounce> QuestAnnounce { get; set; }
    public List<CDataQuestTalkInfo> QuestTalkInfo { get; set; }
    public List<CDataQuestFlag> QuestFlag { get; set; }
    public List<CDataQuestLayoutFlag> QuestLayoutFlag { get; set; }
    public List<CDataQuestProcessState> QuestProcessState { get; set; }
    public List<CDataQuestOrderConditionParam> QuestOrderConditionParam { get; set; }
    public List<CDataQuestEnemyInfo> QuestEnemyInfo { get; set; }
    public List<CDataQuestLayoutFlagSetInfo> QuestLayoutFlagSetInfo { get; set; }
    public bool IsClientOrder { get; set; }
    public bool IsEnable { get; set; }
    public bool CanProgress { get; set; }

    public class Serializer : EntitySerializer<CDataQuestOrderList>
    {
        public override void Write(IBuffer buffer, CDataQuestOrderList obj)
        {
            WriteUInt32(buffer, obj.KeyId);
            WriteUInt32(buffer, obj.QuestScheduleId);
            WriteUInt32(buffer, obj.QuestId);
            WriteUInt32(buffer, obj.AreaId);
            WriteUInt32(buffer, obj.BaseLevel);
            WriteUInt16(buffer, obj.ContentJoinItemRank);
            WriteUInt32(buffer, obj.BaseGold);
            WriteUInt32(buffer, obj.BaseExp);
            WriteUInt32(buffer, obj.BaseRim);
            WriteUInt32(buffer, obj.OrderNpcId);
            WriteUInt32(buffer, obj.NameMsgId);
            WriteUInt32(buffer, obj.DetailMsgId);
            WriteUInt64(buffer, obj.OrderDate);
            WriteUInt64(buffer, obj.EndDistributionDate);
            WriteEntityList(buffer, obj.FixedRewardItem);
            WriteEntityList(buffer, obj.FixedRewardSelectItem);
            WriteEntityList(buffer, obj.QuestAnnounce);
            WriteEntityList(buffer, obj.QuestTalkInfo);
            WriteEntityList(buffer, obj.QuestFlag);
            WriteEntityList(buffer, obj.QuestLayoutFlag);
            WriteEntityList(buffer, obj.QuestProcessState);
            WriteEntityList(buffer, obj.QuestOrderConditionParam);
            WriteEntityList(buffer, obj.QuestEnemyInfo);
            WriteEntityList(buffer, obj.QuestLayoutFlagSetInfo);
            WriteBool(buffer, obj.IsClientOrder);
            WriteBool(buffer, obj.IsEnable);
            WriteBool(buffer, obj.CanProgress);
        }

        public override CDataQuestOrderList Read(IBuffer buffer)
        {
            CDataQuestOrderList obj = new CDataQuestOrderList();
            obj.KeyId = ReadUInt32(buffer);
            obj.QuestScheduleId = ReadUInt32(buffer);
            obj.QuestId = ReadUInt32(buffer);
            obj.AreaId = ReadUInt32(buffer);
            obj.BaseLevel = ReadUInt32(buffer);
            obj.ContentJoinItemRank = ReadUInt16(buffer);
            obj.BaseGold = ReadUInt32(buffer);
            obj.BaseExp = ReadUInt32(buffer);
            obj.BaseRim = ReadUInt32(buffer);
            obj.OrderNpcId = ReadUInt32(buffer);
            obj.NameMsgId = ReadUInt32(buffer);
            obj.DetailMsgId = ReadUInt32(buffer);
            obj.OrderDate = ReadUInt64(buffer);
            obj.EndDistributionDate = ReadUInt64(buffer);
            obj.FixedRewardItem = ReadEntityList<CDataRewardItem>(buffer);
            obj.FixedRewardSelectItem = ReadEntityList<CDataRewardItem>(buffer);
            obj.QuestAnnounce = ReadEntityList<CDataQuestAnnounce>(buffer);
            obj.QuestTalkInfo = ReadEntityList<CDataQuestTalkInfo>(buffer);
            obj.QuestFlag = ReadEntityList<CDataQuestFlag>(buffer);
            obj.QuestLayoutFlag = ReadEntityList<CDataQuestLayoutFlag>(buffer);
            obj.QuestProcessState = ReadEntityList<CDataQuestProcessState>(buffer);
            obj.QuestOrderConditionParam = ReadEntityList<CDataQuestOrderConditionParam>(buffer);
            obj.QuestEnemyInfo = ReadEntityList<CDataQuestEnemyInfo>(buffer);
            obj.QuestLayoutFlagSetInfo = ReadEntityList<CDataQuestLayoutFlagSetInfo>(buffer);
            obj.IsClientOrder = ReadBool(buffer);
            obj.IsEnable = ReadBool(buffer);
            obj.CanProgress = ReadBool(buffer);
            return obj;
        }
    }
}
