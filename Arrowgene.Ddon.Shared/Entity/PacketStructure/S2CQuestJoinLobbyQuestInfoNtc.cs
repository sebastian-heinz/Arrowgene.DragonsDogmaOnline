using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CQuestJoinLobbyQuestInfoNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_QUEST_JOIN_LOBBY_QUEST_INFO_NTC;

        public S2CQuestJoinLobbyQuestInfoNtc()
        {
            LightQuestOrderList = new List<CDataLightQuestOrderList>();
            SetQuestOrderList = new List<CDataSetQuestOrderList>();
            MainQuestOrderList = new List<CDataMainQuestOrderList>();
            TutorialQuestOrderList = new List<CDataTutorialQuestOrderList>();
            LotQuestOrderList = new List<CDataLotQuestOrderList>();
            SubstoryQuestOrderList = new List<CDataSubstoryQuestOrderList>();
            MobHuntQuestOrderList = new List<CDataMobHuntQuestOrderList>();
            TimeLimitedQuestOrderList = new List<CDataTimeLimitedQuestOrderList>();
            WorldManageQuestOrderList = new List<CDataWorldManageQuestOrderList>();
            ExpiredQuestList = new List<CDataExpiredQuestList>();
            MainQuestIdList = new List<CDataQuestId>();
            TutorialQuestIdList = new List<CDataQuestId>();
            PriorityQuestList = new List<CDataPriorityQuest>();
            AreaRankList = new List<CDataAreaRank>();
            QuestDefine = new CDataQuestDefine();
        }

        public List<CDataLightQuestOrderList> LightQuestOrderList { get; set; }
        public List<CDataSetQuestOrderList> SetQuestOrderList { get; set; }
        public List<CDataMainQuestOrderList> MainQuestOrderList { get; set; }
        public List<CDataTutorialQuestOrderList> TutorialQuestOrderList { get; set; }
        public List<CDataLotQuestOrderList> LotQuestOrderList { get; set; }  // TYPE_PAWN=1?
        public List<CDataSubstoryQuestOrderList> SubstoryQuestOrderList { get; set; }
        public List<CDataMobHuntQuestOrderList> MobHuntQuestOrderList { get; set; }
        public List<CDataTimeLimitedQuestOrderList> TimeLimitedQuestOrderList { get; set; }
        public List<CDataWorldManageQuestOrderList> WorldManageQuestOrderList { get; set; }
        public List<CDataExpiredQuestList> ExpiredQuestList { get; set; }
        public List<CDataQuestId> MainQuestIdList { get; set; }
        public List<CDataQuestId> TutorialQuestIdList { get; set; }
        public List<CDataPriorityQuest> PriorityQuestList { get; set; }
        public List<CDataAreaRank> AreaRankList { get; set; }
        public CDataQuestDefine QuestDefine { get; set; }

        public class Serializer : PacketEntitySerializer<S2CQuestJoinLobbyQuestInfoNtc>
        {
            public override void Write(IBuffer buffer, S2CQuestJoinLobbyQuestInfoNtc obj)
            {
                WriteEntityList<CDataLightQuestOrderList>(buffer, obj.LightQuestOrderList);
                WriteEntityList<CDataSetQuestOrderList>(buffer, obj.SetQuestOrderList);
                WriteEntityList<CDataMainQuestOrderList>(buffer, obj.MainQuestOrderList);
                WriteEntityList<CDataTutorialQuestOrderList>(buffer, obj.TutorialQuestOrderList);
                WriteEntityList<CDataLotQuestOrderList>(buffer, obj.LotQuestOrderList);
                WriteEntityList<CDataSubstoryQuestOrderList>(buffer, obj.SubstoryQuestOrderList);
                WriteEntityList<CDataMobHuntQuestOrderList>(buffer, obj.MobHuntQuestOrderList);
                WriteEntityList<CDataTimeLimitedQuestOrderList>(buffer, obj.TimeLimitedQuestOrderList);
                WriteEntityList<CDataWorldManageQuestOrderList>(buffer, obj.WorldManageQuestOrderList);
                WriteEntityList<CDataExpiredQuestList>(buffer, obj.ExpiredQuestList);
                WriteEntityList<CDataQuestId>(buffer, obj.MainQuestIdList);
                WriteEntityList<CDataQuestId>(buffer, obj.TutorialQuestIdList);
                WriteEntityList<CDataPriorityQuest>(buffer, obj.PriorityQuestList);
                WriteEntityList<CDataAreaRank>(buffer, obj.AreaRankList);
                WriteEntity<CDataQuestDefine>(buffer, obj.QuestDefine);
            }

            public override S2CQuestJoinLobbyQuestInfoNtc Read(IBuffer buffer)
            {
                S2CQuestJoinLobbyQuestInfoNtc obj = new S2CQuestJoinLobbyQuestInfoNtc();
                obj.LightQuestOrderList = ReadEntityList<CDataLightQuestOrderList>(buffer);
                obj.SetQuestOrderList = ReadEntityList<CDataSetQuestOrderList>(buffer);
                obj.MainQuestOrderList = ReadEntityList<CDataMainQuestOrderList>(buffer);
                obj.TutorialQuestOrderList = ReadEntityList<CDataTutorialQuestOrderList>(buffer);
                obj.LotQuestOrderList = ReadEntityList<CDataLotQuestOrderList>(buffer);
                obj.SubstoryQuestOrderList = ReadEntityList<CDataSubstoryQuestOrderList>(buffer);
                obj.MobHuntQuestOrderList = ReadEntityList<CDataMobHuntQuestOrderList>(buffer);
                obj.TimeLimitedQuestOrderList = ReadEntityList<CDataTimeLimitedQuestOrderList>(buffer);
                obj.WorldManageQuestOrderList = ReadEntityList<CDataWorldManageQuestOrderList>(buffer);
                obj.ExpiredQuestList = ReadEntityList<CDataExpiredQuestList>(buffer);
                obj.MainQuestIdList = ReadEntityList<CDataQuestId>(buffer);
                obj.TutorialQuestIdList = ReadEntityList<CDataQuestId>(buffer);
                obj.PriorityQuestList = ReadEntityList<CDataPriorityQuest>(buffer);
                obj.AreaRankList = ReadEntityList<CDataAreaRank>(buffer);
                obj.QuestDefine = ReadEntity<CDataQuestDefine>(buffer);
                return obj;
            }
        }
    }
}
