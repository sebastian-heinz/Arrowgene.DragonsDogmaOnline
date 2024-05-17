using System.Collections.Generic;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Quests
{
    public class GenericQuest : Quest
    {
        private List<QuestBlock> Blocks;

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

            quest.Blocks = questAsset.Blocks;

            foreach (var block in questAsset.Blocks)
            {
                // This is used for replacing enemies in a quest
                // So only report these for blocks which have mobs
                if (block.BlockType != QuestBlockType.KillGroup || block.StageId.Id == 0)
                {
                    continue;
                }
                quest.Locations.Add(new QuestLocation() { StageId = block.StageId, SubGroupId = block.SubGroupId });
            }

            return quest;
        }

        public GenericQuest(QuestId questId, bool discoverable) : base(questId, QuestType.World, discoverable)
        {
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
                if (block.BlockNo > processState.BlockNo)
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

                foreach (var enemy in block.Enemies)
                {
                    enemies.Add(new InstancedEnemy(enemy));
                }
            }

            return enemies;
        }

        public override CDataQuestList ToCDataQuestList()
        {
            var quest = base.ToCDataQuestList();
            var firstBlock = Blocks[0];

            quest.QuestProcessStateList = new List<CDataQuestProcessState>()
            {
                BlockAsCDataQuestProcessState(firstBlock)
            };

            return quest;
        }

        public override CDataQuestOrderList ToCDataQuestOrderList()
        {
            var quest = base.ToCDataQuestOrderList();
            var firstBlock = Blocks[0];

            quest.QuestProcessStateList = new List<CDataQuestProcessState>()
            {
                BlockAsCDataQuestProcessState(firstBlock)
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
                BlockAsCDataQuestProcessState(questBlock)
            };
        }

        private CDataQuestProcessState BlockAsCDataQuestProcessState(QuestBlock questBlock)
        {
            CDataQuestProcessState result = new CDataQuestProcessState()
            {
                ProcessNo = questBlock.ProcessNo,
                SequenceNo = 0,
                BlockNo = questBlock.BlockNo,
            };

            if (questBlock.BlockNo > 1 && questBlock.SequenceNo != 1)
            {
                if (questBlock.BlockNo == 2)
                {
                    result.ResultCommandList.Add(QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept, 1));
                }
                else if (questBlock.UpdateAnnounce)
                {
                    result.ResultCommandList.Add(QuestManager.ResultCommand.UpdateAnnounce());
                }
            }

            switch (questBlock.BlockType)
            {
                case QuestBlockType.NpcOrder:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.NpcTalkAndOrderUi(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.NpcOrderDetails.NpcId, 0)
                    });
                    result.ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.QstTalkChg(questBlock.NpcOrderDetails.NpcId, questBlock.NpcOrderDetails.MsgId)
                    };
                    break;
                case QuestBlockType.DiscoverEnemy:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsEnemyFoundForOrder(StageManager.ConvertIdToStageNo(questBlock.StageId), (int) questBlock.StageId.GroupId, 0)
                    });
                    break;
                case QuestBlockType.SeekOutEnemiesAtMarkedLocation:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.IsEnemyFound(StageManager.ConvertIdToStageNo(questBlock.StageId), (int) questBlock.StageId.GroupId, -1)
                    });
                    break;
                case QuestBlockType.End:
                    result.ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear),
                        QuestManager.ResultCommand.EndEndQuest(),
                    };
                    break;
                case QuestBlockType.KillGroup:
                    {
                        var checkCommands = new List<CDataQuestCommand>();
                        for (int i = 0; i < questBlock.Enemies.Count; i++)
                        {
                            checkCommands.Add(QuestManager.CheckCommand.DieEnemy(StageManager.ConvertIdToStageNo(questBlock.StageId), (int)questBlock.StageId.GroupId, i));
                        }
                        result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(checkCommands);
                    }
                    break;
                case QuestBlockType.TalkToNpc:
                    result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.TalkNpc(StageManager.ConvertIdToStageNo(questBlock.StageId), questBlock.NpcOrderDetails.NpcId)
                    });
                    break;
            }

            return result;
        }
    }
}
