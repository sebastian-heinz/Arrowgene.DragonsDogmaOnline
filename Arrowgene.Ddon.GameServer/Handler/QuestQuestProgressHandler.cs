using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestProgressHandler : GameStructurePacketHandler<C2SQuestQuestProgressReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestProgressHandler));

        public QuestQuestProgressHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestQuestProgressReq> packet)
        {
            QuestProgressState questProgressState = QuestProgressState.InProgress;
            S2CQuestQuestProgressRes res = new S2CQuestQuestProgressRes();
            res.QuestScheduleId = packet.Structure.QuestScheduleId;
            res.QuestProgressResult = 0;

            var partyQuestState = client.Party.QuestState;

            ushort processNo = packet.Structure.ProcessNo;
            QuestId questId = (QuestId) packet.Structure.QuestScheduleId;

            Logger.Debug($"QuestId={questId}, KeyId={packet.Structure.KeyId} ProgressCharacterId={packet.Structure.ProgressCharacterId}, QuestScheduleId={packet.Structure.QuestScheduleId}, ProcessNo={packet.Structure.ProcessNo}\n");

            if (!partyQuestState.HasQuest(questId))
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
                var processState = partyQuestState.GetProcessState(questId, processNo);
                
                var quest = QuestManager.GetQuest(questId);
                res.QuestProcessState = quest.StateMachineExecute(Server, client, processState, out questProgressState);

                partyQuestState.UpdateProcessState(questId, res.QuestProcessState);

                if (questProgressState == QuestProgressState.Accepted && quest.QuestType == QuestType.World)
                {
                    foreach (var memberClient in client.Party.Clients)
                    {
                        var questProgress = Server.Database.GetQuestProgressById(memberClient.Character.CommonId, quest.QuestId);
                        if (questProgress != null)
                        {
                            continue;
                        }

                        // Add a new world quest record for the player
                        if (!Server.Database.InsertQuestProgress(memberClient.Character.CommonId, quest.QuestId, quest.QuestType, 0))
                        {
                            Logger.Error($"Failed to insert progress for the quest {quest.QuestId}");
                        }
                    }
                }

                if (questProgressState == QuestProgressState.Checkpoint || questProgressState == QuestProgressState.Accepted)
                {
                    partyQuestState.UpdatePartyQuestProgress(Server, client.Party, questId);
                }
                else if (questProgressState == QuestProgressState.Complete)
                {
                    res.QuestProgressResult = 3; // ProcessEnd
                    CompleteQuest(quest, client, client.Party, partyQuestState);
                }

                if (res.QuestProcessState.Count > 0)
                {
                    Logger.Info("==========================================================================================");
                    Logger.Info($"{questId}: ProcessNo={res.QuestProcessState[0].ProcessNo}, SequenceNo={res.QuestProcessState[0].SequenceNo}, BlockNo={res.QuestProcessState[0].BlockNo},");
                    Logger.Info("==========================================================================================");
                }
            }

            foreach (var memberClient in client.Party.Clients)
            {
                if (memberClient == client)
                {
                    continue;
                }

                S2CQuestQuestProgressNtc ntc = new S2CQuestQuestProgressNtc()
                {
                    ProgressCharacterId = memberClient.Character.CharacterId,
                    QuestScheduleId = res.QuestScheduleId,
                    QuestProcessStateList = res.QuestProcessState,
                };

                memberClient.Send(ntc);
            }

            client.Send(res);
        }

        private void CompleteQuest(Quest quest, GameClient client, PartyGroup party, PartyQuestState partyQuestState)
        {
            // Distribute rewards to the party
            partyQuestState.DistributePartyQuestRewards(Server, party, quest.QuestId);

            // Resolve quest state for all members participating in the quest
            partyQuestState.CompletePartyQuestProgress(Server, party, quest.QuestId);

            S2CQuestCompleteNtc completeNtc = new S2CQuestCompleteNtc()
            {
                QuestScheduleId = (uint)quest.QuestId,
                RandomRewardNum = quest.RandomRewardNum(),
                ChargeRewardNum = quest.RewardParams.ChargeRewardNum,
                ProgressBonusNum = quest.RewardParams.ProgressBonusNum,
                IsRepeatReward = quest.RewardParams.IsRepeatReward,
                IsUndiscoveredReward = quest.RewardParams.IsUndiscoveredReward,
                IsHelpReward = quest.RewardParams.IsHelpReward,
                IsPartyBonus = quest.RewardParams.IsPartyBonus,
            };
            client.Party.SendToAll(completeNtc);

            S2CQuestSetPriorityQuestNtc priorityNtc = new S2CQuestSetPriorityQuestNtc()
            {
                CharacterId = client.Character.CharacterId
            };

            // Get the new list of priority quests from the leader
            var prioirtyQuests = Server.Database.GetPriorityQuests(party.Leader.Client.Character.CommonId);
            foreach (var priorityQuestId in prioirtyQuests)
            {
                var priorityQuest = QuestManager.GetQuest(priorityQuestId);
                var priorityQuestState = party.QuestState.GetQuestState(priorityQuestId);
                priorityNtc.PriorityQuestList.Add(priorityQuest.ToCDataPriorityQuest(priorityQuestState.Step));
            }
            client.Party.SendToAll(priorityNtc);

            if (quest.ResetPlayerAfterQuest)
            {
                foreach (var memberClient in client.Party.Clients)
                {
                    Server.CharacterManager.UpdateCharacterExtendedParamsNtc(memberClient, memberClient.Character);
                }
            }
        }
    }
}
