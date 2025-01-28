using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Data.Common;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestProgressHandler : GameRequestPacketQueueHandler<C2SQuestQuestProgressReq, S2CQuestQuestProgressRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestProgressHandler));

        public QuestQuestProgressHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SQuestQuestProgressReq request)
        {
            PacketQueue packets = new();

            QuestProgressState questProgressState = QuestProgressState.InProgress;
            S2CQuestQuestProgressRes res = new S2CQuestQuestProgressRes();
            res.QuestScheduleId = request.QuestScheduleId;
            res.QuestProgressResult = 0;

            ushort processNo = request.ProcessNo;
            uint questScheduleId = request.QuestScheduleId;

            Logger.Debug($"QuestScheduleId={questScheduleId}, KeyId={request.KeyId} ProgressCharacterId={request.ProgressCharacterId}, ProcessNo={request.ProcessNo}\n");

            var quest = QuestManager.GetQuestByScheduleId(questScheduleId);

            if (quest == null)
            {
                // Tell the quest state machine that for these static quest packets
                // these processes are terminated
                res.QuestProcessState.Add(new CDataQuestProcessState()
                {
                    ProcessNo = processNo,
                    SequenceNo = 0x1,
                    BlockNo = 0xffff,
                });
            }
            else
            {
                QuestStateManager questStateManager = QuestManager.GetQuestStateManager(client, quest);

                var processState = questStateManager.GetProcessState(questScheduleId, processNo);
                res.QuestProcessState = quest.StateMachineExecute(Server, client, processState, out questProgressState);
                questStateManager.UpdateProcessState(questScheduleId, res.QuestProcessState);

                Server.Database.ExecuteInTransaction(connection =>
                {
                    if (questProgressState == QuestProgressState.Accepted && quest.QuestType == QuestType.World)
                    {
                        foreach (var memberClient in client.Party.Clients)
                        {
                            var questProgress = Server.Database.GetQuestProgressByScheduleId(memberClient.Character.CommonId, questScheduleId, connection);
                            if (questProgress != null)
                            {
                                continue;
                            }

                            // Add a new world quest record for the player
                            if (!Server.Database.InsertQuestProgress(memberClient.Character.CommonId, questScheduleId, quest.QuestType, 0, connection))
                            {
                                Logger.Error($"Failed to insert progress for the quest {quest.QuestId}");
                            }
                        }
                    }
                    else if (questProgressState == QuestProgressState.Accepted &&
                             (quest.QuestType == QuestType.Tutorial ||
                              quest.QuestType == QuestType.WildHunt ||
                              quest.QuestType == QuestType.Light))
                    {
                        // Add a new personal quest record for the player
                        if (!Server.Database.InsertQuestProgress(client.Character.CommonId, questScheduleId, quest.QuestType, 0, connection))
                        {
                            Logger.Error($"Failed to insert progress for the quest {quest.QuestId}");
                        }
                    }

                    if (questProgressState == QuestProgressState.Checkpoint || questProgressState == QuestProgressState.Accepted)
                    {
                        questStateManager.UpdateQuestProgress(questScheduleId, connection);
                    }
                    else if (questProgressState == QuestProgressState.Complete)
                    {
                        res.QuestProgressResult = 3; // ProcessEnd
                        var ntcs = CompleteQuest(quest, client, questStateManager, connection);
                        packets.AddRange(ntcs);
                    }
                });

                if (res.QuestProcessState.Count > 0)
                {
                    Logger.Info("==========================================================================================");
                    Logger.Info($"{quest.QuestId} ({quest.QuestScheduleId}): QuestBlock={res.QuestProcessState[0].ProcessNo}.{res.QuestProcessState[0].SequenceNo}.{res.QuestProcessState[0].BlockNo}");
                    Logger.Info("==========================================================================================");
                }

                if (!quest.IsPersonal)
                {
                    foreach (var memberClient in client.Party.Clients)
                    {
                        if (memberClient == client)
                        {
                            continue;
                        }

                        S2CQuestQuestProgressNtc ntc = new S2CQuestQuestProgressNtc()
                        {
                            ProgressCharacterId = memberClient.Character.CharacterId,
                            QuestScheduleId = quest.QuestScheduleId,
                            QuestProcessStateList = res.QuestProcessState,
                        };

                        memberClient.Enqueue(ntc, packets);
                    }
                }
            }

            client.Enqueue(res, packets);

            return packets;
        }

        private PacketQueue CompleteQuest(Quest quest, GameClient client, QuestStateManager questState, DbConnection? connectionIn = null)
        {
            PacketQueue packets = new();
            packets.AddRange(questState.DistributeQuestRewards(quest.QuestScheduleId, connectionIn));
            questState.CompleteQuestProgress(quest.QuestScheduleId, connectionIn);

            S2CQuestCompleteNtc completeNtc = new S2CQuestCompleteNtc()
            {
                QuestScheduleId = quest.QuestScheduleId,
                RandomRewardNum = quest.RandomRewardNum(),
                ChargeRewardNum = quest.RewardParams.ChargeRewardNum,
                ProgressBonusNum = quest.RewardParams.ProgressBonusNum,
                IsRepeatReward = quest.RewardParams.IsRepeatReward,
                IsUndiscoveredReward = quest.RewardParams.IsUndiscoveredReward,
                IsHelpReward = quest.RewardParams.IsHelpReward,
                IsPartyBonus = quest.RewardParams.IsPartyBonus,
            };

            if (quest.IsPersonal)
            {
                client.Enqueue(completeNtc, packets);

                // Finishing personal quests as a non-leader shouldn't adjust the list.
                if (client.Party.IsSolo || client.Party.Leader?.Client == client)
                {
                    packets.AddRange(client.Party.QuestState.UpdatePriorityQuestList(client.Party.Leader.Client, connectionIn));
                }
            }
            else
            {
                client.Party.EnqueueToAll(completeNtc, packets);
                packets.AddRange(client.Party.QuestState.UpdatePriorityQuestList(client.Party.Leader.Client, connectionIn));
            }

            if (quest.ResetPlayerAfterQuest)
            {
                foreach (var memberClient in client.Party.Clients)
                {
                    Server.CharacterManager.UpdateCharacterExtendedParamsNtc(memberClient, memberClient.Character);
                }
            }

            return packets;
        }
    }
}
