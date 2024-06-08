using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Collections.Generic;

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
                res.QuestProcessState = quest.StateMachineExecute(client, processState, out questProgressState);

                partyQuestState.UpdateProcessState(questId, res.QuestProcessState);

                if (questProgressState == QuestProgressState.Complete)
                {
                    SendRewards(client, client.Character, quest);

                    S2CQuestCompleteNtc completeNtc = new S2CQuestCompleteNtc()
                    {
                        QuestScheduleId = (uint)questId,
                        RandomRewardNum = quest.RandomRewardNum(),
                        ChargeRewardNum = quest.RewardParams.ChargeRewardNum,
                        ProgressBonusNum = quest.RewardParams.ProgressBonusNum,
                        IsRepeatReward = quest.RewardParams.IsRepeatReward,
                        IsUndiscoveredReward = quest.RewardParams.IsUndiscoveredReward,
                        IsHelpReward = quest.RewardParams.IsHelpReward,
                        IsPartyBonus = quest.RewardParams.IsPartyBonus,
                    };

                    client.Party.SendToAll(completeNtc);

                    if (quest.HasRewards())
                    {
                        foreach (var memberClient in client.Party.Clients) 
                        {
                            Server.RewardManager.AddQuestRewards(memberClient, quest);
                        }
                    }

                    // Remove the quest data from the party object
                    partyQuestState.CompleteQuest(questId);

                    if (quest.QuestType == QuestType.Main)
                    {
                        var leaderCommonId = client.Party.Leader.Client.Character.CommonId;
                        // TODO: Eventually handle all types of quests and for all members
                        Server.Database.RemoveQuestProgress(leaderCommonId, quest.QuestType, quest.QuestId);
                        if (quest.NextQuestId != QuestId.None)
                        {
                            var nextQuest = QuestManager.GetQuest(quest.NextQuestId);
                            Server.Database.InsertQuestProgress(leaderCommonId, nextQuest.QuestId, nextQuest.QuestType, 0);
                        }

                        Server.Database.InsertIfNotExistCompletedQuest(leaderCommonId, quest.QuestId, quest.QuestType);
                    }

                    if (quest.ResetPlayerAfterQuest)
                    {
                        foreach (var memberClient in client.Party.Clients)
                        {
                            Server.CharacterManager.UpdateCharacterExtendedParamsNtc(memberClient, memberClient.Character);
                        }
                    }
                }

                if (res.QuestProcessState.Count > 0)
                {
                    Logger.Info("==========================================================================================");
                    Logger.Info($"{questId}: ProcessNo={res.QuestProcessState[0].ProcessNo}, SequenceNo={res.QuestProcessState[0].SequenceNo}, BlockNo={res.QuestProcessState[0].BlockNo},");
                    Logger.Info("==========================================================================================");
                }
            }

            S2CQuestQuestProgressNtc ntc = new S2CQuestQuestProgressNtc()
            {
                ProgressCharacterId = client.Character.CharacterId,
                QuestScheduleId = res.QuestScheduleId,
                QuestProcessStateList = res.QuestProcessState,
            };
            client.Party.SendToAllExcept(ntc, client);

            client.Send(res);
        }

        private void SendRewards(GameClient client, Character character, Quest quest)
        {
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = (ushort)ItemNoticeType.Quest
            };

            foreach (var walletReward in quest.WalletRewards)
            {
                Server.WalletManager.AddToWallet(character, walletReward.Type, walletReward.Value);

                updateCharacterItemNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint()
                {
                    Type = walletReward.Type,
                    Value = Server.WalletManager.GetWalletAmount(character, walletReward.Type),
                    AddPoint = (int)walletReward.Value
                });
            }

            if (updateCharacterItemNtc.UpdateWalletList.Count > 0)
            {
                client.Send(updateCharacterItemNtc);
            }

            foreach (var expPoint in quest.ExpRewards)
            {
                Server.ExpManager.AddExp(client, character, expPoint.Reward, 0, 2); // I think type 2 means quest
            }
        }
    }
}
