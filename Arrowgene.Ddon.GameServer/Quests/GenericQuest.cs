using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.GameServer.Quests.MainQuests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using static System.Reflection.Metadata.BlobBuilder;

namespace Arrowgene.Ddon.GameServer.Quests
{
    public class GenericQuest : Quest
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GenericQuest));

        private List<QuestProcess> Processes;

        public static GenericQuest FromAsset(QuestAssetData questAsset)
        {
            var quest = new GenericQuest(questAsset.QuestId, questAsset.Type, questAsset.Discoverable);

            quest.BaseLevel = questAsset.BaseLevel;
            quest.MinimumItemRank = questAsset.MinimumItemRank;
            quest.NextQuestId = questAsset.NextQuestId;
            quest.QuestLayoutFlagSetInfo = questAsset.QuestLayoutSetInfoFlags;
            quest.QuestLayoutFlags = questAsset.QuestLayoutFlags;
            quest.Processes = questAsset.Processes;

            quest.ExpRewards.Add(new CDataQuestExp()
            {
                ExpMode = 1,
                Reward = questAsset.ExpReward
            });

            foreach (var walletReward in questAsset.RewardCurrency)
            {
                quest.WalletRewards.Add(new CDataWalletPoint()
                {
                    Type = walletReward.WalletType,
                    Value = walletReward.Amount
                });
            }

            foreach (var rewardItem in questAsset.RewardItems)
            {
                switch (rewardItem.RewardType)
                {
                    case QuestRewardType.Random:
                    case QuestRewardType.Fixed:
                        quest.ItemRewards.Add(rewardItem);
                        break;
                    case QuestRewardType.Select:
                        quest.SelectedableItemRewards.Add(rewardItem);
                        break;
                }
            }

            foreach (var process in quest.Processes)
            {
                foreach (var block in process.Blocks)
                {
                    switch (block.BlockType)
                    {
                        case QuestBlockType.KillGroup:
                            quest.Locations.Add(new QuestLocation() { StageId = block.StageId, SubGroupId = block.SubGroupId });
                            break;
                        case QuestBlockType.DeliverItems:
                            foreach (var request in block.DeliveryRequests)
                            {
                                quest.DeliveryItems.Add(new QuestDeliveryItem()
                                {
                                    ItemId = request.ItemId,
                                    Amount = request.Amount,
                                });
                            }
                            break;
                    }

                    block.QuestProcessState = BlockAsCDataQuestProcessState(block, quest.Locations);
                }
            }

            return quest;
        }

        public GenericQuest(QuestId questId, QuestType questType, bool discoverable) : base(questId, questType, discoverable)
        {
            Processes = new List<QuestProcess>();
            QuestLayoutFlagSetInfo = new List<QuestLayoutFlagSetInfo>();
        }

        public override bool HasEnemiesInCurrentStageGroup(QuestState questState, StageId stageId, uint subGroupId)
        {
            // Search to see if a target is required for the current process
            foreach (var process in Processes)
            {
                foreach (var block in process.Blocks)
                {
                    if (!questState.ProcessState.ContainsKey(block.ProcessNo))
                    {
                        continue;
                    }

                    var processState = questState.ProcessState[block.ProcessNo];
                    if (processState.BlockNo != block.BlockNo)
                    {
                        continue;
                    }

                    if ((stageId.Id == block.StageId.Id) && (stageId.GroupId == block.StageId.GroupId))
                    {
                        return true;
                    }
                    // Keep looking
                }
            }
            return false;
        }

        public override List<InstancedEnemy> GetEnemiesInStageGroup(StageId stageId, uint subGroupId)
        {
            List<InstancedEnemy> enemies = new List<InstancedEnemy>();

            foreach (var process in Processes)
            {
                foreach (var block in process.Blocks)
                {
                    if (block.BlockType != QuestBlockType.KillGroup)
                    {
                        continue;
                    }

                    if (block.StageId.Id != stageId.Id || block.StageId.GroupId != stageId.GroupId)
                    {
                        continue;
                    }

                    byte index = 0;
                    foreach (var enemy in block.Enemies)
                    {
                        enemies.Add(new InstancedEnemy(enemy)
                        {
                            Index = (byte)(index + block.StartingIndex)
                        });

                        index += 1;
                    }
                }
            }

            return enemies;
        }

        public override void SendProgressWorkNotices(GameClient client, StageId stageId, uint subGroupId)
        {
            QuestLocation questLocation = null;
            foreach (var location in Locations)
            {
                if (location.ContainsStageId(stageId, (ushort) subGroupId))
                {
                    questLocation = location;
                    break;
                }
            }

            if (questLocation == null)
            {
                client.Party.SendToAll(new S2CQuestQuestProgressWorkSaveNtc());
            }
            else
            {
                var proccessState = client.Party.QuestState.GetProcessState(QuestId, 0);
                // We need to signal the current block
                client.Party.SendToAll(new S2CQuestQuestProgressWorkSaveNtc()
                {
                    QuestScheduleId = (uint)QuestId,
                    ProcessNo = proccessState.ProcessNo,
                    SequenceNo = 0,
                    BlockNo = proccessState.BlockNo,
                    WorkList = new List<CDataQuestProgressWork>()
                    {
                        QuestManager.NotifyCommand.KilledTargetEnemySetGroup((int) questLocation.QuestLayoutFlag, StageManager.ConvertIdToStageNo(stageId), (int) stageId.GroupId)
                    }
                });
            }
        }

        public override CDataQuestList ToCDataQuestList()
        {
            var quest = base.ToCDataQuestList();

            foreach (var process in Processes)
            {
                if (process.Blocks.Count > 0)
                {
                    quest.QuestProcessStateList.Add(process.Blocks[0].QuestProcessState);
                }
            }

            return quest;
        }

        public override CDataQuestOrderList ToCDataQuestOrderList()
        {
            var quest = base.ToCDataQuestOrderList();

            foreach (var process in Processes)
            {
                if (process.Blocks.Count > 0)
                {
                    quest.QuestProcessStateList.Add(process.Blocks[0].QuestProcessState);
                }
            }

            return quest;
        }

        public override List<CDataQuestProcessState> StateMachineExecute(QuestProcessState processState, out QuestProgressState questProgressState)
        {
            if (processState.ProcessNo >= Processes.Count)
            {
                questProgressState = QuestProgressState.Unknown;
                return new List<CDataQuestProcessState>();
            }

            var process = Processes[processState.ProcessNo];
            if ((processState.BlockNo) >= process.Blocks.Count)
            {
                questProgressState = QuestProgressState.Unknown;
                return new List<CDataQuestProcessState>();
            }

            var questBlock = process.Blocks[processState.BlockNo];
            if (processState.ProcessNo == 0 && questBlock.SequenceNo == 1)
            {
                questProgressState = QuestProgressState.Complete;
            }
            else
            {
                questProgressState = QuestProgressState.InProgress;
            }

            return new List<CDataQuestProcessState>()
            {
                questBlock.QuestProcessState
            };
        }

        private static CDataQuestProcessState BlockAsCDataQuestProcessState(QuestBlock questBlock, List<QuestLocation> locations)
        {
            CDataQuestProcessState result = new CDataQuestProcessState()
            {
                ProcessNo = questBlock.ProcessNo,
                SequenceNo = questBlock.SequenceNo,
                BlockNo = questBlock.BlockNo,
            };

            if (questBlock.SequenceNo != 1)
            {                
                switch (questBlock.AnnounceType)
                {
                    case QuestAnnounceType.Accept:
                        result.ResultCommandList.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept, 1));
                        break;
                    case QuestAnnounceType.Update:
                        result.ResultCommandList.Add(QuestManager.ResultCommand.UpdateAnnounce());
                        break;
                }
            }

            foreach (var layoutFlag in questBlock.QuestLayoutFlagsOn)
            {
                result.ResultCommandList.Add(QuestManager.ResultCommand.QstLayoutFlagOn((int) layoutFlag));
            }

            foreach (var layoutFlag in questBlock.QuestLayoutFlagsOff)
            {
                result.ResultCommandList.Add(QuestManager.ResultCommand.QstLayoutFlagOff((int)layoutFlag));
            }

            switch (questBlock.BlockType)
            {
                case QuestBlockType.NpcTalkAndOrder:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.NpcTalkAndOrderUi(StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId), questBlock.NpcOrderDetails[0].NpcId, 0)
                    });
                    result.ResultCommandList.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    break;
                case QuestBlockType.DiscoverEnemy:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsEnemyFoundForOrder(StageManager.ConvertIdToStageNo(questBlock.StageId), (int) questBlock.StageId.GroupId, -1)
                    });
                    break;
                case QuestBlockType.SeekOutEnemiesAtMarkedLocation:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsEnemyFoundForOrder(StageManager.ConvertIdToStageNo(questBlock.StageId), (int) questBlock.StageId.GroupId, -1)
                    });
                    break;
                case QuestBlockType.End:
                    result.ResultCommandList.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear));
                    result.ResultCommandList.Add(QuestManager.ResultCommand.EndEndQuest());
                    break;
                case QuestBlockType.KillGroup:
                    {
                        result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                        {
                            QuestManager.CheckCommand.DieEnemy(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, -1)
                        });
                    }
                    break;
                case QuestBlockType.CollectItem:
                    {
                        var questCommand = questBlock.ShowMarker ?
                            QuestManager.CheckCommand.QuestOmSetTouch(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, 1) :
                            QuestManager.CheckCommand.QuestOmSetTouchWithoutMarker(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, 1);
                        result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                        {
                            questCommand
                        });
                    }
                    break;
                case QuestBlockType.DeliverItems:
                    {
                        List<CDataQuestCommand> deliveryRequests = new List<CDataQuestCommand>();
                        foreach (var item in questBlock.DeliveryRequests)
                        {
                            deliveryRequests.Add(QuestManager.CheckCommand.DeliverItem((int)item.ItemId, (int)item.Amount));
                        }
                        result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(deliveryRequests);
                        result.ResultCommandList.Add(QuestManager.ResultCommand.SetDeliverInfoQuest(StageManager.ConvertIdToStageNo(questBlock.StageId), (int) questBlock.StageId.GroupId, 1, 0));
                    }
                    break;
                case QuestBlockType.CheckMyQstFlags:
                    {
                        List<CDataQuestCommand> cmds = new List<CDataQuestCommand>();
                        foreach (var flag in questBlock.MyQstFlags)
                        {
                            cmds.Add(QuestManager.CheckCommand.MyQstFlagOn((int) flag));
                        }
                        result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(cmds);
                    }
                    break;
                case QuestBlockType.SetMyQstFlags:
                    {
                        List<CDataQuestCommand> cmds = new List<CDataQuestCommand>();
                        foreach (var flag in questBlock.MyQstFlags)
                        {
                            cmds.Add(QuestManager.ResultCommand.MyQstFlagOn((int)flag));
                        }
                        result.ResultCommandList.AddRange(cmds);
                    }
                    break;
                case QuestBlockType.TalkToNpc:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.TalkNpc(StageManager.ConvertIdToStageNo(questBlock.NpcOrderDetails[0].StageId), questBlock.NpcOrderDetails[0].NpcId)
                    });
                    result.ResultCommandList.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails[0].NpcId, questBlock.NpcOrderDetails[0].MsgId));
                    break;
                case QuestBlockType.IsQuestOrdered:
                    {
                        CDataQuestCommand questCommand = null;
                        switch (questBlock.QuestOrderDetails.QuestType)
                        {
                            case QuestType.Main:
                                questCommand = QuestManager.CheckCommand.IsMainQuestOrder((int) questBlock.QuestOrderDetails.QuestId);
                                break;
                            case QuestType.World:
                                questCommand = QuestManager.CheckCommand.IsOrderWorldQuest((int) questBlock.QuestOrderDetails.QuestId);
                                break;
                            case QuestType.PersonalQuest:
                                questCommand = QuestManager.CheckCommand.IsOrderLightQuest((int) questBlock.QuestOrderDetails.QuestId);
                                break;
                            case QuestType.PawnExpedition:
                            case QuestType.PartnerPawnPersonalQuests:
                                // TODO: Needs different arguments
                                // questCommand = QuestManager.CheckCommand.IsOrderPawnQuest((int)questBlock.QuestOrderDetails.QuestId);
                                break;
                            case QuestType.Board:
                            case QuestType.Hidden:
                            case QuestType.ExtremeMissions:
                                break;
                        }
                        result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>() { questCommand });
                    }
                    break;
                case QuestBlockType.IsStageNo:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.StageNo(StageManager.ConvertIdToStageNo(questBlock.StageId))
                    });
                    break;
                case QuestBlockType.Raw:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(questBlock.CheckCommands);
                    result.ResultCommandList.AddRange(questBlock.ResultCommands);
                    break;
                case QuestBlockType.DummyBlock:
                    /* Filler block which might do some meta things like announce or set flags */
                    break;
                case QuestBlockType.None:
                    /* Place holder for terminal blocks in process other than 0 */
                    break;
                default:
                    Logger.Error($"Unexpected block found '{questBlock.BlockType}'. Unable to translate.");
                    break;
            }

            return result;
        }
    }
}
