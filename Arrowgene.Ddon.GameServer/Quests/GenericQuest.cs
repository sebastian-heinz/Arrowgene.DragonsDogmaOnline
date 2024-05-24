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

namespace Arrowgene.Ddon.GameServer.Quests
{
    public class GenericQuest : Quest
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GenericQuest));

        private List<QuestBlock> Blocks;
        private HashSet<uint> QuestLayoutFlags;

        public static GenericQuest FromAsset(QuestAssetData questAsset)
        {
            var quest = new GenericQuest(questAsset.QuestId, questAsset.Discoverable);

            quest.BaseLevel = questAsset.BaseLevel;
            quest.MinimumItemRank = questAsset.MinimumItemRank;

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

            foreach (var block in questAsset.Blocks)
            {
                // This is used for replacing enemies in a quest
                // So only report these for blocks which have mobs
                if (block.BlockType != QuestBlockType.KillGroup)
                {
                    continue;
                }

                quest.Locations.Add(new QuestLocation() { StageId = block.StageId, SubGroupId = block.SubGroupId});
            }

            quest.Blocks = questAsset.Blocks;

            foreach (var block in quest.Blocks)
            {
                block.QuestProcessState = BlockAsCDataQuestProcessState(block, quest.Locations);
                
                foreach (var layoutFlag in block.QuestLayoutFlagsOn)
                {
                    quest.QuestLayoutFlags.Add(layoutFlag);
                }
            }

            return quest;
        }

        public GenericQuest(QuestId questId, bool discoverable) : base(questId, QuestType.World, discoverable)
        {
            Blocks = new List<QuestBlock>();
            QuestLayoutFlags = new HashSet<uint>();
        }

        public override bool HasEnemiesInCurrentStageGroup(QuestState questState, StageId stageId, uint subGroupId)
        {
            // Search to see if a target is required for the current process
            foreach (var block in Blocks)
            {
                if (!questState.ProcessState.ContainsKey(block.ProcessNo))
                {
                    continue;
                }

                var processState = questState.ProcessState[block.ProcessNo];
                if (processState.BlockNo < block.BlockNo)
                {
                    continue;
                }

                if ((stageId.Id == block.StageId.Id) && (stageId.GroupId == block.StageId.GroupId))
                {
                    return true;
                }

                // Keep looking
            }

            return false;
        }

        public override List<InstancedEnemy> GetEnemiesInStageGroup(StageId stageId, uint subGroupId)
        {
            List<InstancedEnemy> enemies = new List<InstancedEnemy>();

            foreach (var block in Blocks)
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
                    ProcessNo = 0,
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
            var firstBlock = Blocks[0];

            quest.QuestProcessStateList = new List<CDataQuestProcessState>()
            {
                firstBlock.QuestProcessState
            };

            foreach (var layoutFlag in QuestLayoutFlags)
            {
                quest.QuestLayoutFlagList.Add(new CDataQuestLayoutFlag() { FlagId = layoutFlag });
            }

            foreach (var location in Locations)
            {
                StageNo stageNo = StageManager.ConvertIdToStageNo(location.StageId);

                if (location.QuestLayoutFlag != 0)
                {
                    quest.QuestLayoutFlagSetInfoList.Add(QuestManager.LayoutFlag.Create(location.QuestLayoutFlag, stageNo, location.StageId.GroupId));
                }
            }

            return quest;
        }

        public override CDataQuestOrderList ToCDataQuestOrderList()
        {
            var quest = base.ToCDataQuestOrderList();
            var firstBlock = Blocks[0];

            quest.QuestProcessStateList = new List<CDataQuestProcessState>()
            {
                firstBlock.QuestProcessState
            };

            return quest;
        }

        public override List<CDataQuestProcessState> StateMachineExecute(QuestProcessState processState, out QuestProgressState questProgressState)
        {
            if ((processState.BlockNo) >= Blocks.Count)
            {
                questProgressState = QuestProgressState.Unknown;
                return new List<CDataQuestProcessState>();
            }

            var questBlock = Blocks[processState.BlockNo];
            if (questBlock.SequenceNo == 1)
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
                case QuestBlockType.DummyBlock:
                    /* Used to do things like advance quest state or turn some flags on/off without checking for any additional conditions */
                    // TODO: Add a StageNo check?
                    break;
                case QuestBlockType.NpcTalkAndOrder:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.NpcTalkAndOrderUi(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.NpcOrderDetails.NpcId, 0)
                    });
                    result.ResultCommandList.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails.NpcId, questBlock.NpcOrderDetails.MsgId));
                    break;
                case QuestBlockType.DiscoverEnemy:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        // QuestManager.CheckCommand.IsEnemyFoundForOrder(StageManager.ConvertIdToStageNo(questBlock.StageId), (int) questBlock.StageId.GroupId, 0)
                        QuestManager.CheckCommand.IsEnemyFound(StageManager.ConvertIdToStageNo(questBlock.StageId), (int) questBlock.StageId.GroupId, -1)
                    });
                    break;
                case QuestBlockType.SeekOutEnemiesAtMarkedLocation:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsEnemyFound(StageManager.ConvertIdToStageNo(questBlock.StageId), (int) questBlock.StageId.GroupId, -1)
                    });
                    break;
                case QuestBlockType.Accept:
                    /* For quests which accept needs to be seperate from other events */
                    break;
                case QuestBlockType.End:
                    result.ResultCommandList.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear));
                    result.ResultCommandList.Add(QuestManager.ResultCommand.EndEndQuest());
                    break;
                case QuestBlockType.KillGroup:
                    {
                        var checkCommands = new List<CDataQuestCommand>();
                        for (int i = 0; i < questBlock.Enemies.Count; i++)
                        {
                            checkCommands.Add(QuestManager.CheckCommand.DieEnemy(
                                StageManager.ConvertIdToStageNo(questBlock.StageId),
                                (int)questBlock.StageId.GroupId,
                                (int)(i + questBlock.StartingIndex)));
                        }
                        result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(checkCommands);
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
                    }
                    break;
                case QuestBlockType.TalkToNpc:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.TalkNpc(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.NpcOrderDetails.NpcId)
                    });
                    result.ResultCommandList.Add(QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails.NpcId, questBlock.NpcOrderDetails.MsgId));
                    break;
                case QuestBlockType.IsStageNo:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.StageNo(StageManager.ConvertIdToStageNo(questBlock.StageId))
                    });
                    break;
                default:
                    Logger.Error($"Unexpected block found '{questBlock.BlockType}'. Unable to translate.");
                    break;
            }

            return result;
        }
    }
}
