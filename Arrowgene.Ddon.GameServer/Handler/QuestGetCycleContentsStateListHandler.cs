using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestGetCycleContentsStateListHandler : GameRequestPacketHandler<C2SQuestGetCycleContentsStateListReq, S2CQuestGetCycleContentsStateListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestGetCycleContentsStateListHandler));

        public QuestGetCycleContentsStateListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CQuestGetCycleContentsStateListRes Handle(GameClient client, C2SQuestGetCycleContentsStateListReq request)
        {
#if false
            /*
             * @note If something goes wrong, we can always change this preprocessor directive to
             * true and get back the original functionality before we started to play with this function.
             */
            EntitySerializer<S2CQuestJoinLobbyQuestInfoNtc> serializer = EntitySerializer.Get<S2CQuestJoinLobbyQuestInfoNtc>();
            S2CQuestJoinLobbyQuestInfoNtc pcap = serializer.Read(InGameDump.data_Dump_20A);
            client.Send(pcap);
#else
            S2CQuestJoinLobbyQuestInfoNtc pcap = EntitySerializer.Get<S2CQuestJoinLobbyQuestInfoNtc>().Read(InGameDump.data_Dump_20A);
            S2CQuestJoinLobbyQuestInfoNtc ntc = new S2CQuestJoinLobbyQuestInfoNtc();

            ntc.WorldManageQuestOrderList = pcap.WorldManageQuestOrderList; // Recover paths + change vocation

            ntc.QuestDefine = pcap.QuestDefine; // Recover quest log data to be able to accept quests
            ntc.QuestDefine.OrderMaxNum = Server.GameLogicSettings.QuestOrderMax;
            ntc.QuestDefine.RewardBoxMaxNum = Server.GameLogicSettings.RewardBoxMax;

            // pcap.MainQuestIdList; (this will add back all missing functionality which depends on complete MSQ)
            var completedMsq = client.Character.CompletedQuests.Values.Where(x => x.QuestType == QuestType.Main);
            foreach (var msq in completedMsq)
            {
                ntc.MainQuestIdList.Add(new CDataQuestId() { QuestId = (uint)msq.QuestId });
            }

            var completedTutorials = client.Character.CompletedQuests.Values.Where(x => x.QuestType == QuestType.Tutorial);
            foreach (var tut in completedTutorials)
            {
                ntc.TutorialQuestIdList.Add(new CDataQuestId() { QuestId = (uint)tut.QuestId });
            }

            var allQuestsInProgress = Server.Database.GetQuestProgressByType(client.Character.CommonId, QuestType.All);
            foreach (var questProgress in allQuestsInProgress)
            {
                var quest = QuestManager.GetQuestByScheduleId(questProgress.QuestScheduleId);
                if (quest == null || !quest.IsActive(Server, client))
                {
                    continue;
                }

                switch (questProgress.QuestType)
                {
                    case QuestType.Tutorial:
                        var tutorialQuest = quest.ToCDataTutorialQuestOrderList(questProgress.Step);
                        ntc.TutorialQuestOrderList.Add(tutorialQuest);
                        break;
                    case QuestType.WildHunt:
                        var mobHuntQuest = quest.ToCDataMobHuntQuestOrderList(questProgress.Step);
                        ntc.MobHuntQuestOrderList.Add(mobHuntQuest);
                        break;
                    case QuestType.Light:
                        var lightQuest = quest.ToCDataLightQuestOrderList(questProgress.Step);
                        if (lightQuest.Detail.BoardType == 1 && lightQuest.Detail.GetAp == 0)
                        {
                            lightQuest.Detail.GetAp = Server.AreaRankManager.GetAreaPointReward(quest);
                        }
                        ntc.LightQuestOrderList.Add(lightQuest);
                        break;
                }
            }

            if (client.Party != null)
            {
                var priorityQuests = Server.Database.GetPriorityQuestScheduleIds(client.Party.Leader.Client.Character.CommonId);
                foreach (var questScheduleId in priorityQuests)
                {
                    var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                    if (quest == null || !quest.IsActive(Server, client))
                    {
                        continue;
                    }
                    
                    ntc.PriorityQuestList.Add(new CDataPriorityQuest()
                    {
                        QuestId = (uint)quest.QuestId,
                        QuestScheduleId = (uint)quest.QuestScheduleId
                    });
                }
            }

            client.Send(ntc);
#endif
            return new();

            // client.Send(InGameDump.Dump_24);
        }
    }
}
