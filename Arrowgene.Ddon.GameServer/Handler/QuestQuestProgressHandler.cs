using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.Tokens;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class QuestQuestProgressHandler : StructurePacketHandler<GameClient, C2SQuestQuestProgressReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestQuestProgressHandler));

        private readonly WalletManager _WalletManager;
        private readonly ExpManager _ExpManager;
        private readonly ItemManager _ItemManager;

        public QuestQuestProgressHandler(DdonGameServer server) : base(server)
        {
            _WalletManager = server.WalletManager;
            _ExpManager = server.ExpManager;
            _ItemManager = server.ItemManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SQuestQuestProgressReq> packet)
        {
            QuestProgressState questProgressState = QuestProgressState.InProgress;
            S2CQuestQuestProgressRes res = new S2CQuestQuestProgressRes();
            res.QuestScheduleId = packet.Structure.QuestScheduleId;
            res.QuestProgressResult = 0;

            Logger.Debug($"KeyId={packet.Structure.KeyId} ProgressCharacterId={packet.Structure.ProgressCharacterId}, QuestScheduleId={packet.Structure.QuestScheduleId}, ProcessNo={packet.Structure.ProcessNo}\n");

            var partyQuestState = client.Party.QuestState;

            ushort processNo = packet.Structure.ProcessNo;
            QuestId questId = (QuestId) packet.Structure.QuestScheduleId;

            if (!partyQuestState.HasQuest(questId))
            {
                // Hack for making the random quests from static packets go away
                List<CDataQuestCommand> ResultCommandList = new List<CDataQuestCommand>();
                ResultCommandList.Add(new CDataQuestCommand()
                {
                    Command = (ushort)QuestCommandCheckType.IsEndTimer,
                    Param01 = 0x173
                });

                res.QuestScheduleId = 0x32f00;
                res.QuestProcessState.Add(new CDataQuestProcessState()
                {
                    ProcessNo = 0x1b,
                    SequenceNo = 0x1,
                    BlockNo = 0x2,
                    ResultCommandList = ResultCommandList
                });
            }
            else
            {
                var processState = partyQuestState.GetProcessState(questId, processNo);
                
                var quest = QuestManager.GetQuest(questId);
                res.QuestProcessState = quest.StateMachineExecute(processState, out questProgressState);

                partyQuestState.UpdateProcessState(questId, res.QuestProcessState);

                if (questProgressState == QuestProgressState.Complete)
                {
                    SendRewards(client, client.Character, quest);

                    S2CQuestCompleteNtc completeNtc = new S2CQuestCompleteNtc()
                    {
                        QuestScheduleId = (uint) questId,
                        RandomRewardNum = quest.RandomRewardNum(),
                        ChargeRewardNum = quest.RewardParams.ChargeRewardNum,
                        ProgressBonusNum = quest.FixedRewardsNum(),
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
                            memberClient.Character.QuestRewards.Add(quest.GetBoxRewards());
                        }
                    }

                    // Remove the quest data from the party object
                    partyQuestState.CompleteQuest(questId);
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
                _WalletManager.AddToWallet(character, walletReward.Type, walletReward.Value);

                updateCharacterItemNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint()
                {
                    Type = walletReward.Type,
                    Value = _WalletManager.GetWalletAmount(character, walletReward.Type),
                    AddPoint = (int)walletReward.Value
                });
            }

            if (updateCharacterItemNtc.UpdateWalletList.Count > 0)
            {
                client.Send(updateCharacterItemNtc);
            }

            foreach (var expPoint in quest.ExpRewards)
            {
                _ExpManager.AddExp(client, character, expPoint.Reward, 0, 2); // I think type 2 means quest
            }
        }
    }
}
