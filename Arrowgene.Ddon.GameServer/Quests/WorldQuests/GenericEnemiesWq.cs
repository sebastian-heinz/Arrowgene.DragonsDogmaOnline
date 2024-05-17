#if false
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Transactions;
using System.Xml.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Enemies;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Quests
{
    public class QuestEnemey
    {
        public uint EnemyId { get; set; }
        public ushort Level {  get; set; }
        public bool IsBoss {  get; set; }
        public uint Exp {  get; set; }
    }

    public class QuestTarget
    {
        public ushort ProcessNo {  get; set; }
        public ushort BlockNo {  get; set; }
        public uint StageNo {  get; set; }
        public uint GroupNo {  get; set; }
        public uint SetNo {  get; set; }

        public List<QuestEnemey> Enemies { get; set; }

        public QuestTarget()
        {
            Enemies = new List<QuestEnemey>();
        }
    }

    public class QuestNpcOrder
    {
        public NpcId NpcId { get; set; }
        public StageNo StageNo { get; set; }
        public uint MsgId {  get; set; }
    }

    public class GenericEnemiesWq : Quest
    {
        private List<QuestTarget> QuestTargets { get; set; }
        private QuestStartCondition StartCondition { get; set; }
        private QuestNpcOrder NpcOrderDetails { get; set; }

        public static GenericEnemiesWq FromAsset(QuestAssetData questAsset)
        {
            List<QuestTarget> questTargets = new List<QuestTarget>();
            foreach (var target in questAsset.Targets)
            {
                questTargets.Add(new QuestTarget()
                {
                    ProcessNo = target.ProcessNo,
                    BlockNo = target.BlockNo,
                    StageNo = target.StageNo,
                    GroupNo = target.GroupNo,
                    SetNo = target.SetNo,
                    Enemies = target.Enemies.Select(x => new QuestEnemey
                    {
                        EnemyId = x.EnemyId,
                        Level = x.Level,
                        IsBoss = x.IsBoss,
                        Exp = x.Exp
                    }).ToList()
                });
            }

            var quest = new GenericEnemiesWq(questAsset.QuestId, questTargets, questAsset.Discoverable);

            quest.BaseLevel = questAsset.BaseLevel;
            quest.MinimumItemRank = questAsset.MinimumItemRank;
            quest.StartCondition = questAsset.StartCondition;

            if (quest.StartCondition == QuestStartCondition.Npc)
            {
                quest.NpcOrderDetails.NpcId = questAsset.NpcOrderDetails.NpcId;
                quest.NpcOrderDetails.StageNo = questAsset.NpcOrderDetails.StageNo;
                quest.NpcOrderDetails.MsgId = questAsset.NpcOrderDetails.MsgId;
            }

            foreach (var location in questAsset.Locations)
            {
                quest.Locations.Add(new QuestLocation()
                {
                    StageId = location.StageId,
                    SubGroupId = location.SubGroupId
                });
            }

            foreach (var rewardItem in questAsset.RewardItems)
            {
                switch (rewardItem.Type)
                {
                    case QuestRewardType.Random:
                        quest.ItemRewards.Add(new QuestRandomReward()
                        {
                            LootPool = rewardItem.LootPool.Select(x => new RandomLootPoolItem()
                            {
                                ItemId = x.ItemId,
                                Chance = x.Chance,
                                Num = x.Num
                            }).ToList<LootPoolItem>()
                        });
                        break;
                    case QuestRewardType.Select:
                        quest.SelectedableItemRewards.Add(new QuestSelectReward()
                        {
                            LootPool = rewardItem.LootPool.Select(x => new SelectLootPoolItem()
                            {
                                ItemId = x.ItemId,
                                Num = x.Num
                            }).ToList<LootPoolItem>()
                        });
                        break;
                }
            }

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

            return quest;
        }

        public GenericEnemiesWq(QuestId questId, List<QuestTarget> questTargets, bool discoverable) : base(questId, QuestType.World, discoverable)
        {
            QuestTargets = questTargets;
            NpcOrderDetails = new QuestNpcOrder();
        }

        public override bool HasEnemiesInCurrentStageGroup(QuestState questState, StageId stageId, uint subGroupId)
        {
            uint stageNo = StageManager.ConvertIdToStageNo(stageId);

            // Search to see if a target is required for the current process
            foreach (var questTarget in QuestTargets)
            {
                if (!questState.ProcessState.ContainsKey(questTarget.ProcessNo))
                {
                    continue;
                }

                var processState = questState.ProcessState[questTarget.ProcessNo];
                // TODO: Handle cases where a list is required for multiple blocks or only between certain blocks?
                if (questTarget.BlockNo > processState.BlockNo)
                {
                    continue;
                }

                if ((stageNo == questTarget.StageNo) && (stageId.GroupId == questTarget.GroupNo))
                {
                    return true;
                }

                // Keep looking
            }

            return false;
        }

        public override List<InstancedEnemy> GetEnemiesInStageGroup(StageId stageId, uint subGroupId)
        {
            uint stageNo = StageManager.ConvertIdToStageNo(stageId);
            List<InstancedEnemy> enemies = new List<InstancedEnemy>();

            foreach (var target in QuestTargets)
            {
                if (target.StageNo != stageNo || target.GroupNo != stageId.GroupId)
                {
                    continue;
                }

                foreach (var enemy in target.Enemies)
                {
                    enemies.Add(new InstancedEnemy()
                    {
                        EnemyId = enemy.EnemyId,
                        Lv = enemy.Level,
                        IsBossBGM = enemy.IsBoss,
                        IsBossGauge = enemy.IsBoss,
                        EnemyTargetTypesId = 4,
                        Experience = enemy.Exp,
                        Scale = 100
                        // TODO: Get rest of properties added as optional in JSON
                    });
                }
                break;
            }

            return enemies;
        }

        public override CDataQuestList ToCDataQuestList()
        {
            var quest = base.ToCDataQuestList();

            switch (StartCondition)
            {
                case QuestStartCondition.Find:
                    quest.QuestProcessStateList = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState()
                        {
                            ProcessNo = 0, SequenceNo = 0, BlockNo = 1,
                            CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                            {
                                QuestManager.CheckCommand.IsEnemyFoundForOrder((StageNo) QuestTargets[0].StageNo, (int) QuestTargets[0].GroupNo, -1)
                            })
                        }
                    };
                    break;
                case QuestStartCondition.Npc:
                    quest.QuestTalkInfoList.Add(new CDataQuestTalkInfo(NpcOrderDetails.NpcId, NpcOrderDetails.MsgId));
                    quest.QuestProcessStateList = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState()
                        {
                            ProcessNo = 0, SequenceNo = 0, BlockNo = 1,
                            ResultCommandList = new List<CDataQuestCommand>()
                            {
                                QuestManager.ResultCommand.QstTalkChg(NpcOrderDetails.NpcId, (int) NpcOrderDetails.MsgId)
                            },
                            CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                            {
                                QuestManager.CheckCommand.NpcTalkAndOrderUi(NpcOrderDetails.StageNo, NpcOrderDetails.NpcId, 0)
                            })
                        }
                    };
                    break;
            }
            return quest;
        }

        public override CDataQuestOrderList ToCDataQuestOrderList()
        {
            var quest = base.ToCDataQuestOrderList();

            switch (StartCondition)
            {
                case QuestStartCondition.Find:
                    quest.QuestProcessStateList = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState()
                        {
                            ProcessNo = 0, SequenceNo = 0, BlockNo = 1,
                            CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                            {
                                QuestManager.CheckCommand.IsEnemyFoundForOrder((StageNo) QuestTargets[0].StageNo, (int) QuestTargets[0].GroupNo, -1)
                            })
                        }
                    };
                    break;
                case QuestStartCondition.Npc:
                    quest.QuestProcessStateList = new List<CDataQuestProcessState>()
                    {
                        new CDataQuestProcessState()
                        {
                            ProcessNo = 0, SequenceNo = 0, BlockNo = 1,
                            ResultCommandList = new List<CDataQuestCommand>()
                            {
                                QuestManager.ResultCommand.QstTalkChg(NpcOrderDetails.NpcId, (int) NpcOrderDetails.MsgId)
                            },
                            CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                            {
                                QuestManager.CheckCommand.NpcTalkAndOrderUi(NpcOrderDetails.StageNo, NpcOrderDetails.NpcId, 0)
                            })
                        }
                    };
                    break;
            }
            return quest;
        }

        public override List<CDataQuestProcessState> StateMachineExecute(QuestProcessState processState, out QuestProgressState questProgressState)
        {
            bool FoundNext = false;
            List<CDataQuestProcessState> result = new List<CDataQuestProcessState>();
            questProgressState = QuestProgressState.InProgress;

            // Attempt to find the next step in this quest
            foreach (var questTarget in QuestTargets)
            {
                if (questTarget.ProcessNo != processState.ProcessNo || questTarget.BlockNo != processState.BlockNo)
                {
                    continue;
                }

                CDataQuestProcessState resultProcessState = new CDataQuestProcessState()
                {
                    ProcessNo = processState.ProcessNo,
                    SequenceNo = 0,
                    BlockNo = (ushort)(processState.BlockNo + 1),
                };

                if (questTarget.BlockNo == 1)
                {
                    resultProcessState.ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Accept, 1)
                    };

                    questProgressState = QuestProgressState.Accepted;
                }
                else
                {
                    resultProcessState.ResultCommandList = new List<CDataQuestCommand>()
                    { 
                        QuestManager.ResultCommand.UpdateAnnounce(),
                    };
                }

                var checkEnemyList = new List<CDataQuestCommand>();
                for (int i = 0; i < questTarget.Enemies.Count; i++)
                {
                    checkEnemyList.Add(QuestManager.CheckCommand.DieEnemy((StageNo)questTarget.StageNo, (int)questTarget.GroupNo, i));
                }

                resultProcessState.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(checkEnemyList);

                FoundNext = true;

                result.Add(resultProcessState);
                break;
            }

            if (!FoundNext && StartCondition == QuestStartCondition.Npc && processState.SequenceNo != 1)
            {
                result.Add(new CDataQuestProcessState()
                {
                    ProcessNo = processState.ProcessNo,
                    SequenceNo = 1,
                    BlockNo = (ushort)(processState.BlockNo + 1),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.UpdateAnnounce(),
                    },
                    CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(new List<CDataQuestCommand>()
                    {
                        QuestManager.CheckCommand.TalkNpc(NpcOrderDetails.StageNo, NpcOrderDetails.NpcId)
                    })
                });

                FoundNext = true;

                questProgressState = QuestProgressState.ReportToNpc;
            }

            if (!FoundNext)
            {

                // We reached the end of the quest?
                result.Add(new CDataQuestProcessState()
                {
                    ProcessNo = processState.ProcessNo, SequenceNo = 1, BlockNo = (ushort)(processState.BlockNo + 1),
                    ResultCommandList = new List<CDataQuestCommand>()
                    {
                        QuestManager.ResultCommand.SetAnnounce(QuestAnnounceType.Clear),
                        QuestManager.ResultCommand.EndEndQuest(),
                    }
                });

                questProgressState = QuestProgressState.Complete;
            }

            return result;
        }
    }
}
#endif
