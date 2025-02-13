using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataQuestOrderList
{
    public CDataQuestOrderList()
    {
        FixedRewardItem = new List<CDataRewardItem>();
        FixedRewardSelectItem = new List<CDataRewardItem>();
        BaseWalletPoints = new List<CDataWalletPoint>();
        BaseExp = new List<CDataQuestExp>();
        ContentsReleaseList = new List<CDataCharacterReleaseElement>();
        QuestLog = new CDataQuestLog();
        QuestFlagList = new List<CDataQuestFlag>();
        QuestLayoutFlagList = new List<CDataQuestLayoutFlag>();
        QuestProcessStateList = new List<CDataQuestProcessState>();
        QuestOrderConditionParam = new List<CDataQuestOrderConditionParam>();
        QuestEnemyInfoList = new List<CDataQuestEnemyInfo>();
        Unk8 = new List<CDataQuestOrderListUnk8>();
        DeliveryItemList = new List<CDataDeliveryItem>();
        QuestLayoutFlagSetInfoList = new List<CDataQuestLayoutFlagSetInfo>();
    }

    public uint KeyId { get; set; }
    public uint QuestScheduleId { get; set; }
    public uint QuestId { get; set; }
    public uint AreaId { get; set; }
    public uint BaseLevel { get; set; }
    public ushort ContentJoinItemRank { get; set; }
    public List<CDataWalletPoint> BaseWalletPoints { get; set; }
    public List<CDataQuestExp> BaseExp { get; set; }
    public uint NpcId { get; set;} // NpcId?
    public uint Unk3 { get; set;} // MsgId?
    public uint Unk4 { get; set;} // DetailMsgId
    public ulong Unk5 { get; set; } // ??
    public ulong Unk6 { get; set; } // OrderDate?
    public ulong Unk6A { get; set; } // EndDistributionDate?
    public List<CDataRewardItem> FixedRewardItem { get; set; }
    public List<CDataRewardItem> FixedRewardSelectItem { get; set; }
    public List<CDataCharacterReleaseElement> ContentsReleaseList { get; set; }
    public CDataQuestLog QuestLog { get; set; }
    public List<CDataQuestFlag> QuestFlagList { get; set; }
    public List<CDataQuestLayoutFlag> QuestLayoutFlagList { get; set; }
    public List<CDataQuestProcessState> QuestProcessStateList { get; set; }
    public List<CDataQuestOrderConditionParam> QuestOrderConditionParam { get; set; }
    public List<CDataQuestEnemyInfo> QuestEnemyInfoList { get; set; }
    public List<CDataQuestLayoutFlagSetInfo> QuestLayoutFlagSetInfoList { get; set; }
    public List<CDataQuestOrderListUnk8> Unk8 { get; set; }
    public List<CDataDeliveryItem> DeliveryItemList { get; set; }
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
            WriteEntityList<CDataWalletPoint>(buffer, obj.BaseWalletPoints);
            WriteEntityList<CDataQuestExp>(buffer, obj.BaseExp);
            WriteUInt32(buffer, obj.NpcId);
            WriteUInt32(buffer, obj.Unk3);
            WriteUInt32(buffer, obj.Unk4);
            WriteUInt64(buffer, obj.Unk5);
            WriteUInt64(buffer, obj.Unk6);
            WriteUInt64(buffer, obj.Unk6A);
            WriteEntityList(buffer, obj.FixedRewardItem);
            WriteEntityList(buffer, obj.FixedRewardSelectItem);
            WriteEntityList<CDataCharacterReleaseElement>(buffer, obj.ContentsReleaseList);
            WriteEntity<CDataQuestLog>(buffer, obj.QuestLog);
            WriteEntityList<CDataQuestFlag>(buffer, obj.QuestFlagList);
            WriteEntityList<CDataQuestLayoutFlag>(buffer, obj.QuestLayoutFlagList);
            WriteEntityList<CDataQuestProcessState>(buffer, obj.QuestProcessStateList);
            WriteEntityList<CDataQuestOrderConditionParam>(buffer, obj.QuestOrderConditionParam);
            WriteEntityList<CDataQuestEnemyInfo>(buffer, obj.QuestEnemyInfoList);
            WriteEntityList<CDataQuestLayoutFlagSetInfo>(buffer, obj.QuestLayoutFlagSetInfoList);
            WriteEntityList<CDataQuestOrderListUnk8>(buffer, obj.Unk8);
            WriteEntityList<CDataDeliveryItem>(buffer, obj.DeliveryItemList);
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
            obj.BaseWalletPoints = ReadEntityList<CDataWalletPoint>(buffer);
            obj.BaseExp = ReadEntityList<CDataQuestExp>(buffer);
            obj.NpcId = ReadUInt32(buffer);
            obj.Unk3 = ReadUInt32(buffer);
            obj.Unk4 = ReadUInt32(buffer);
            obj.Unk5 = ReadUInt64(buffer);
            obj.Unk6 = ReadUInt64(buffer);
            obj.Unk6A = ReadUInt64(buffer);
            obj.FixedRewardItem = ReadEntityList<CDataRewardItem>(buffer);
            obj.FixedRewardSelectItem = ReadEntityList<CDataRewardItem>(buffer);
            obj.ContentsReleaseList = ReadEntityList<CDataCharacterReleaseElement>(buffer);
            obj.QuestLog = ReadEntity<CDataQuestLog>(buffer);
            obj.QuestFlagList = ReadEntityList<CDataQuestFlag>(buffer);
            obj.QuestLayoutFlagList = ReadEntityList<CDataQuestLayoutFlag>(buffer);
            obj.QuestProcessStateList = ReadEntityList<CDataQuestProcessState>(buffer);
            obj.QuestOrderConditionParam = ReadEntityList<CDataQuestOrderConditionParam>(buffer);
            obj.QuestEnemyInfoList = ReadEntityList<CDataQuestEnemyInfo>(buffer);
            obj.QuestLayoutFlagSetInfoList = ReadEntityList<CDataQuestLayoutFlagSetInfo>(buffer);
            obj.Unk8 = ReadEntityList<CDataQuestOrderListUnk8>(buffer);
            obj.DeliveryItemList = ReadEntityList<CDataDeliveryItem>(buffer);
            obj.IsClientOrder = ReadBool(buffer);
            obj.IsEnable = ReadBool(buffer);
            obj.CanProgress = ReadBool(buffer);
            return obj;
        }
    }
}
