using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
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
            S2CQuestJoinLobbyQuestInfoNtc pcap = EntitySerializer.Get<S2CQuestJoinLobbyQuestInfoNtc>().Read(InGameDump.data_Dump_20B);
            S2CQuestJoinLobbyQuestInfoNtc ntc = new S2CQuestJoinLobbyQuestInfoNtc();

            ntc.WorldManageQuestOrderList = pcap.WorldManageQuestOrderList; // Recover paths + change vocation

            // TODO: Eventually populate all flags based player state for q7* quests
            foreach (var quest in ntc.WorldManageQuestOrderList)
            {
                quest.Param.QuestFlagList.Clear();
                quest.Param.QuestLayoutFlagList.Clear();
                quest.Param.QuestFlagList = client.Character.GetWorldManageQuestUnlocks((QuestId) quest.Param.QuestId);
                quest.Param.QuestLayoutFlagList = client.Character.GetWorldManageLayoutUnlocks((QuestId)quest.Param.QuestId);
            }

            ntc.QuestDefine = pcap.QuestDefine; // Recover quest log data to be able to accept quests
            ntc.QuestDefine.OrderMaxNum = Server.GameSettings.GameServerSettings.QuestOrderMax;
            ntc.QuestDefine.RewardBoxMaxNum = Server.GameSettings.GameServerSettings.RewardBoxMax;

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

            // Add special quests not normally part of DDON
            foreach (var questId in new List<QuestId>() {QuestId.WorldManageMonsterCaution , QuestId.WorldManageJobTutorial, QuestId.WorldManageDebug})
            {
                var customWorldManageQuest = QuestManager.GetQuestByQuestId(questId);
                ntc.WorldManageQuestOrderList.Add(customWorldManageQuest.ToCDataWorldManageQuestOrderList(0));
            }

            List<QuestProgress> allQuestsInProgress = new();
            List<uint> priorityQuests = new();
            bool decayedQuests = false;
            Server.Database.ExecuteInTransaction(connection =>
            {
                allQuestsInProgress = Server.Database.GetQuestProgressByType(client.Character.CommonId, QuestType.All, connection);
                priorityQuests = client.Party is not null ? Server.Database.GetPriorityQuestScheduleIds(client.Party.Leader.Client.Character.CommonId, connection) : new();

                decayedQuests = Server.LightQuestManager.HandleQuestDecay(client.Character, allQuestsInProgress, priorityQuests, connection).Any();
            });

            foreach (var questProgress in allQuestsInProgress)
            {
                var quest = QuestManager.GetQuestByScheduleId(questProgress.QuestScheduleId);
                if (quest == null || !quest.IsActive(client))
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
                            lightQuest.Detail.GetAp = AreaRankManager.GetAreaPointReward(quest);
                        }
                        ntc.LightQuestOrderList.Add(lightQuest);
                        break;
                }
            }

            if (client.Party != null)
            {
                foreach (var questScheduleId in priorityQuests)
                {
                    var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                    if (quest == null || !quest.IsActive(client))
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

            if (decayedQuests)
            {
                client.Send(new S2CLobbyChatMsgNotice()
                {
                    Type = LobbyChatMsgType.ManagementAlertC,
                    Message = "A quest has been canceled because the\ndelivery time period has ended."
                });
            }

            return new();
        }
    }
}
