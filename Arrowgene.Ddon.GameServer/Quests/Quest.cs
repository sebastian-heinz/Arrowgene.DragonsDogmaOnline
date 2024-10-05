using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Context;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Arrowgene.Ddon.GameServer.Quests
{
    public class QuestRewardParams
    {
        public byte ChargeRewardNum { get; set; }
        public byte ProgressBonusNum { get; set; }
        public bool IsRepeatReward { get; set; }
        public bool IsUndiscoveredReward { get; set; }
        public bool IsHelpReward { get; set; }
        public bool IsPartyBonus { get; set; }
    }

    public class QuestLocation
    {
        public StageId StageId { get; set; }
        public ushort SubGroupId { get; set; }

        public uint QuestLayoutFlag {  get; set; }

        public bool ContainsStageId(StageId stageId, ushort subGroupId)
        {
            return (stageId.Id == StageId.Id) && (stageId.GroupId == StageId.GroupId) && (stageId.LayerNo == StageId.LayerNo) && (subGroupId == SubGroupId);
        }
    }

    public class QuestDeliveryItem
    {
        public uint ItemId { get; set; }
        public uint Amount {  get; set; }
    }

    public abstract class Quest
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(Quest));

        protected List<QuestProcess> Processes { get; set; }
        public readonly QuestId QuestId;
        public readonly bool IsDiscoverable;
        public readonly QuestType QuestType;
        public readonly QuestId QuestScheduleId;
        public QuestAreaId QuestAreaId { get; set; }
        public StageId StageId {  get; set; }
        public uint NewsImageId { get; set; }
        public uint BaseLevel { get; set; }
        public ushort MinimumItemRank { get; set; }
        public QuestId NextQuestId { get; protected set; }
        public bool ResetPlayerAfterQuest { get; protected set; }
        public List<QuestOrderCondition> OrderConditions { get; protected set; }
        public QuestRewardParams RewardParams { get; protected set; }
        public List<CDataWalletPoint> WalletRewards { get; protected set; }
        public List<CDataQuestExp> ExpRewards { get; protected set; }
        public List<QuestRewardItem> ItemRewards { get; protected set; }
        public List<QuestRewardItem> SelectableRewards { get; protected set; }
        public List<CDataCharacterReleaseElement> ContentsReleaseRewards { get; protected set; }
        public List<QuestLocation> Locations { get; protected set; }
        public List<QuestDeliveryItem> DeliveryItems { get; protected set; }
        public List<QuestLayoutFlagSetInfo> QuestLayoutFlagSetInfo;
        public List<QuestLayoutFlag> QuestLayoutFlags;
        public QuestMissionParams MissionParams { get; protected set; }
        public Dictionary<uint, QuestEnemyGroup> EnemyGroups { get; set; }
        public HashSet<StageId> UniqueEnemyGroups { get; protected set; }
        public List<QuestServerAction> ServerActions { get; protected set; }
        public bool IsVariantQuest { get; set; }
        public uint VariantId { get; set; }
        public bool Enabled { get; protected set; }

        public Quest(QuestId questId, QuestId questScheduleId, QuestType questType, bool isDiscoverable = false)
        {
            QuestId = questId;
            QuestType = questType;
            QuestScheduleId = questScheduleId;
            IsDiscoverable = isDiscoverable;

            OrderConditions = new List<QuestOrderCondition>();
            RewardParams = new QuestRewardParams();
            WalletRewards = new List<CDataWalletPoint>();
            ExpRewards = new List<CDataQuestExp>();
            ItemRewards = new List<QuestRewardItem>();
            SelectableRewards = new List<QuestRewardItem>();
            ContentsReleaseRewards = new List<CDataCharacterReleaseElement>();
            Locations = new List<QuestLocation>();
            DeliveryItems = new List<QuestDeliveryItem>();
            QuestLayoutFlagSetInfo = new List<QuestLayoutFlagSetInfo>();
            QuestLayoutFlags = new List<QuestLayoutFlag>();
            EnemyGroups = new Dictionary<uint, QuestEnemyGroup>();
            UniqueEnemyGroups = new HashSet<StageId>();
            IsVariantQuest = false;
            VariantId = 0;
            MissionParams = new QuestMissionParams();
            ServerActions = new List<QuestServerAction>();
            Processes = new List<QuestProcess>();
        }

        private List<CDataQuestProcessState> GetProcessState(uint step, out uint announceNoCount)
        {
            Dictionary<QuestFlagType, Dictionary<int, QuestFlag>> questFlags = new Dictionary<QuestFlagType, Dictionary<int, QuestFlag>>();
            List<CDataQuestProcessState> result = new List<CDataQuestProcessState>();

            int i = 0;
            uint stepsFound = 0;

            announceNoCount = 0;
            for (; i < Processes[0].Blocks.Count && stepsFound < step; i++)
            {
                var block = Processes[0].Blocks[i];
                if (block.IsCheckpoint || block.AnnounceType == QuestAnnounceType.Accept)
                {
                    stepsFound += 1;
                }

                if (block.AnnounceType != QuestAnnounceType.None)
                {
                    announceNoCount += 1;
                }

                foreach (var flag in block.QuestFlags)
                {
                    if (flag.Action == QuestFlagAction.Set || flag.Action == QuestFlagAction.Clear)
                    {
                        if (!questFlags.ContainsKey(flag.Type))
                        {
                            questFlags[flag.Type] = new Dictionary<int, QuestFlag>();
                        }

                        questFlags[flag.Type][flag.Value] = flag;
                    }
                }

                foreach (var flag in block.CheckpointQuestFlags)
                {
                    if (flag.Action == QuestFlagAction.Set || flag.Action == QuestFlagAction.Clear)
                    {
                        if (!questFlags.ContainsKey(flag.Type))
                        {
                            questFlags[flag.Type] = new Dictionary<int, QuestFlag>();
                        }

                        questFlags[flag.Type][flag.Value] = flag;
                    }
                }

                if (step == stepsFound)
                {
                    break;
                }
            }

            if (step != stepsFound)
            {
                throw new QuestRestoreProgressFailedException(QuestId, step, stepsFound);
            }

            result.Add(new CDataQuestProcessState(Processes[0].Blocks[i].QuestProcessState));

            for (int j = 1; j < Processes.Count; j++)
            {
                var process = Processes[j];
                if (process.Blocks.Count > 0)
                {
                    result.Add(new CDataQuestProcessState(process.Blocks[0].QuestProcessState));
                }
            }

            // Eliminate any announce or free item steps when resuming a quest.
            foreach (var processState in result)
            {
                // Make copy of the result commands
                processState.ResultCommandList = processState.ResultCommandList
                    .Where(x => x.Command != (ushort)QuestResultCommand.UpdateAnnounce &&
                                x.Command != (ushort)QuestResultCommand.SetAnnounce &&
                                x.Command != (ushort)QuestResultCommand.HandItem &&
                                x.Command != (ushort)QuestResultCommand.PushImteToPlBag)
                    .ToList();
            }

            // Generate a block that replays all the flags that got set and cleared
            if (questFlags.Count > 0)
            {

                var flags = new List<QuestFlag>();
                foreach (var flag in questFlags)
                {
                    flags.AddRange(flag.Value.Values.ToList());
                }
                

                var questBlock = new QuestBlock()
                {
                    ProcessNo = (ushort)Processes.Count,
                    SequenceNo = 1,
                    BlockNo = 1,
                    QuestFlags = flags
                };

                result.Add(Quest.BlockAsCDataQuestProcessState(questBlock));
            }

            return result;
        }

        public virtual CDataQuestList ToCDataQuestList(uint step)
        {
            var quest = new CDataQuestList()
            {
                QuestId = (uint)QuestId,
                QuestScheduleId = (uint)QuestScheduleId,
                BaseLevel = BaseLevel,
                ContentJoinItemRank = MinimumItemRank,
                IsClientOrder = step > 0,
                BaseExp = ExpRewards,
                BaseWalletPoints = WalletRewards,
                FixedRewardItemList = GetQuestFixedRewards(),
                FixedRewardSelectItemList = GetQuestSelectableRewards(),
                QuestOrderConditionParamList = GetQuestOrderConditions(),
            };

            quest.QuestProcessStateList = GetProcessState(step, out uint announceNoCount);

            for (uint i = 0; i < announceNoCount; i++)
            {
                quest.QuestAnnounceList.Add(new CDataQuestAnnounce() { AnnounceNo = i });
            }

            foreach (var questLayoutFlag in QuestLayoutFlags)
            {
                quest.QuestLayoutFlagList.Add(questLayoutFlag.AsCDataQuestLayoutFlag());
            }

            foreach (var questLayoutFlagSet in QuestLayoutFlagSetInfo)
            {
                quest.QuestLayoutFlagSetInfoList.Add(questLayoutFlagSet.AsCDataQuestLayoutFlagSetInfo());
            }

            return quest;
        }

        public virtual CDataQuestOrderList ToCDataQuestOrderList(uint step)
        {
            var quest = new CDataQuestOrderList()
            {
                QuestId = (uint)QuestId,
                QuestScheduleId = (uint)QuestScheduleId,
                BaseLevel = BaseLevel,
                ContentJoinItemRank = MinimumItemRank,
                IsClientOrder = step > 0,
                IsEnable = true,
                CanProgress = true,
                BaseExp = ExpRewards,
                BaseWalletPoints = WalletRewards,
                FixedRewardItem = GetQuestFixedRewards(),
                FixedRewardSelectItem = GetQuestSelectableRewards(),
                QuestOrderConditionParam = GetQuestOrderConditions(),
            };

            quest.QuestProcessStateList = GetProcessState(step, out uint announceNoCount);

            for (uint i = 0; i < announceNoCount; i++)
            {
                quest.QuestLog.QuestAnnounceList.Add(new CDataQuestAnnounce() { AnnounceNo = i });
            }

            foreach (var questLayoutFlag in QuestLayoutFlags)
            {
                quest.QuestLayoutFlagList.Add(questLayoutFlag.AsCDataQuestLayoutFlag());
            }

            foreach (var questLayoutFlagSet in QuestLayoutFlagSetInfo)
            {
                quest.QuestLayoutFlagSetInfoList.Add(questLayoutFlagSet.AsCDataQuestLayoutFlagSetInfo());
            }

            return quest;
        }

        public virtual CDataTutorialQuestOrderList ToCDataTutorialQuestOrderList(uint step, bool enableCancel = false)
        {
            return new CDataTutorialQuestOrderList()
            {
                Param = ToCDataQuestOrderList(step),
                EnableCancel = enableCancel
            };
        }

        public virtual CDataTutorialQuestList ToCDataTutorialQuestList(uint step, bool enableCancel = false)
        {
            return new CDataTutorialQuestList()
            {
                Param = ToCDataQuestList(step),
                EnableCancel = enableCancel
            };
        }

        public virtual CDataPriorityQuest ToCDataPriorityQuest(uint step)
        {
            var questProcessStateList = GetProcessState(step, out uint announceNoCount);

            var result = new CDataPriorityQuest()
            {
                QuestId = (uint) QuestId,
                QuestScheduleId = (uint) QuestScheduleId,

            };

            for (uint i = 0; i < announceNoCount; i++)
            {
                result.QuestAnnounceList.Add(new CDataQuestAnnounce() { AnnounceNo = i });
            }

            return result;
        }

        public virtual CDataMainQuestList ToCDataMainQuestList(uint step)
        {
            return new CDataMainQuestList()
            {
                Param = ToCDataQuestList(step)
            };
        }

        public virtual CDataLightQuestList ToCDataLightQuestList(uint step)
        {
            var result = new CDataLightQuestList()
            {
                Param = ToCDataQuestList(step),
                Contents = new CDataQuestContents()
                {
                    Type = 2,
                    Param01 = 0x011200,
                    Param02 = 2,
                    Param03 = 2,
                    Param04 = 0,
                    Unk0 = 6,
                    Unk1 = 6,
                },
                Detail = new CDataLightQuestDetail()
                {
                    AreaId = (uint)QuestAreaId.HidellPlains,
                    BaseAreaPoint = 25,
                    BoardType = 1, // 1 == Normal, 2 == Clan?
                    OrderLimit = 25,
                    ClearNum = 0,
                    GetCP = 25 // This is reward area points
                }
            };

            return result;
        }

        public virtual CDataTimeGainQuestList ToCDataTimeGainQuestList(uint step)
        {
            var result = new CDataTimeGainQuestList()
            {
                Param = ToCDataQuestList(step),
                PlayTimeInSec = MissionParams.PlaytimeInSeconds,
                IsNoTimeup = (MissionParams.PlaytimeInSeconds == 0),
                Unk0 = false, // Could also be IsNoTimeUp?
                IsJoinCharacter = !MissionParams.IsSolo,
                IsJoinPawn = MissionParams.MaxPawns > 0,
                Unk1 = false,
                JoinPawnNum = (byte) MissionParams.MaxPawns,
            };
            result.Restrictions.RestrictArmor = !MissionParams.ArmorAllowed;
            result.Restrictions.RestrictJewlery = !MissionParams.JewelryAllowed;

#if false
            // Figure out what these fields do
            result.Restrictions.Unk0 = 2;
            result.Restrictions.Unk1List.Add(new CDataCommonU32() { Value = 1 });
            result.Restrictions.Unk2List.Add(new CDataTimeGainQuestUnk1Unk2() { Unk0 = 3, Unk1 = true });
            result.Restrictions.Unk5List.Add(new CDataCommonU8() { Value = 2 });
#endif

            HashSet<uint> items = new HashSet<uint>();
            List<QuestRewardItem> rewards = this.ItemRewards.Concat(this.SelectableRewards).ToList();
            // Rewards for EXM seem to show up independently
            foreach (var rewardData in rewards)
            {
                foreach (var reward in rewardData.LootPool)
                {
                    if (rewardData.RewardType == QuestRewardType.Fixed || rewardData.RewardType == QuestRewardType.Select)
                    {
                        result.RewardItemDetailList.Add(new CDataRewardItemDetail()
                        {
                            ItemId = reward.ItemId,
                            Num = reward.Num,
                            Type = 11
                        });
                    }
                    else if (rewardData.RewardType == QuestRewardType.Random && !items.Contains(reward.ItemId))
                    {
                        items.Add(reward.ItemId);
                        result.RewardItemDetailList.Add(new CDataRewardItemDetail()
                        {
                            ItemId = reward.ItemId,
                            Num = 1,
                            Type = 12
                        });
                    }
                }
            }

            return result;
        }

        public virtual CDataSetQuestInfoList ToCDataSetQuestInfoList()
        {
            var result = new CDataSetQuestInfoList()
            {
                QuestScheduleId = (uint)QuestScheduleId,
                QuestId = (uint)QuestId,
                ImageId = NewsImageId, // Optional, client has its own defaults if you fail to provide one.
                BaseLevel = BaseLevel,
                IsDiscovery = true, // If false, hides quest details from the news report.
                EndDistributionDate = uint.MaxValue, // ulong.MaxValue causes some math on the client to overflow and report it as ending soon, so we use uint here.
                ContentJoinItemRank = (ushort)(OrderConditions.Find(x => x.Type == QuestOrderConditionType.ItemRank)?.Param01 ?? 0),
                RandomRewardNum = RandomRewardNum(),
                SelectRewardItemIdList = GetQuestSelectableRewards().Select(x => new CDataCommonU32(x.ItemId)).ToList(),
                //DiscoverRewardWalletPoint = WalletRewards, // These are not the same as the regular rewards?
                //DiscoverRewardExp = ExpRewards, // These are not the same as the regular rewards?
                QuestLayoutFlagSetInfoList = QuestLayoutFlagSetInfo.Select(x => x.AsCDataQuestLayoutFlagSetInfo()).ToList(),
                QuestEnemyInfoList = EnemyGroups.Values.SelectMany(group => group.Enemies.Select(enemy => new CDataQuestEnemyInfo()
                {
                    GroupId = enemy.UINameId,
                    Unk0 = 0, // Seemingly always 0 in the pcaps
                    Lv = enemy.Lv,
                    IsPartyRecommend = enemy.IsBossGauge
                }))
                .ToList(),
                QuestOrderConditionParamList = GetQuestOrderConditions(),
                DeliveryItemList = DeliveryItems.Select(x => new CDataDeliveryItem()
                {
                    ItemId = x.ItemId,
                    Unk0 = (ushort)x.Amount
                })
                .ToList()
            };

            return result;
        }

        public virtual CDataContentsPlayStartData ToCDataContentsPlayStartData(uint step = 0)
        {
            return new CDataContentsPlayStartData()
            {
                QuestId = (uint) QuestId,
                QuestScheudleId = (uint) QuestScheduleId,
                BaseLevel = BaseLevel,
                StartPos = MissionParams.StartPos,
                QuestEnemyInfoList = EnemyGroups.Values.SelectMany(group => group.Enemies.Select(enemy => new CDataQuestEnemyInfo()
                {
                    GroupId = enemy.UINameId,
                    Unk0 = 0, // Seemingly always 0 in the pcaps
                    Lv = enemy.Lv,
                    IsPartyRecommend = enemy.IsBossGauge
                }))
                .ToList(),
                QuestLayoutFlagSetInfoList = QuestLayoutFlagSetInfo.Select(x => x.AsCDataQuestLayoutFlagSetInfo()).ToList(),
                QuestProcessStateList = GetProcessState(step, out uint announceNoCount),
                Unk0 = true,
                Unk1 = false,
                Unk2 = true
            };
        }

        public virtual CDataRaidBossPlayStartData ToCDataRaidBossPlayStartData(uint step = 0)
        {
            return new CDataRaidBossPlayStartData()
            {
                CommonData = ToCDataContentsPlayStartData(step),
                ClearTimePointBonusList = new List<CDataClearTimePointBonus>()
                {
                    new CDataClearTimePointBonus() {Ratio = 1, Seconds = 100}
                },
                RaidBossEnemyParam = new CDataRaidBossEnemyParam()
                {
                    RaidBossId = 1
                }
            };
        }

        public abstract List<CDataQuestProcessState> StateMachineExecute(DdonGameServer server, GameClient client, QuestProcessState processState, out QuestProgressState questProgressState);

        public virtual void SendProgressWorkNotices(GameClient client, StageId stageId, uint subGroupId)
        {
            client.Party.SendToAll(new S2CQuestQuestProgressWorkSaveNtc());
        }

        public virtual void ResetEnemiesForBlock(GameClient client, QuestBlock questBlock)
        {
            foreach (var groupId in questBlock.EnemyGroupIds)
            {
                var enemyGroup = EnemyGroups[groupId];

                // Cleanup old contexts if we are replacing monsters with new ones
                foreach (var enemy in enemyGroup.Enemies)
                {
                    var uid = ContextManager.CreateEnemyUID(enemy.Index, enemyGroup.StageId.ToStageLayoutId());
                    client.Party.Contexts.Remove(uid);
                }

                S2CInstanceEnemyGroupResetNtc resetNtc = new S2CInstanceEnemyGroupResetNtc()
                {
                    LayoutId = enemyGroup.StageId.ToStageLayoutId()
                };

                client.Party.InstanceEnemyManager.ResetEnemyNode(enemyGroup.StageId);
                client.Party.SendToAll(resetNtc);
            }
        }

        public virtual void ResetEnemiesForStage(GameClient client, StageId stageId)
        {
            foreach (var (groupId, group) in EnemyGroups)
            {
                if (group.StageId.Id == stageId.Id)
                {
                    // Cleanup old contexts if we are replacing monsters with new ones
                    foreach (var enemy in group.Enemies)
                    {
                        var uid = ContextManager.CreateEnemyUID(enemy.Index, group.StageId.ToStageLayoutId());
                        client.Party.Contexts.Remove(uid);
                    }

                    S2CInstanceEnemyGroupResetNtc resetNtc = new S2CInstanceEnemyGroupResetNtc()
                    {
                        LayoutId = group.StageId.ToStageLayoutId()
                    };

                    client.Party.InstanceEnemyManager.ResetEnemyNode(group.StageId);
                    client.Party.SendToAll(resetNtc);
                }
            }
        }

        public virtual void HandleAreaChange(GameClient client, StageId stageId)
        {
            ResetEnemiesForStage(client, stageId);

            // TODO: Figure out what these do
            client.Party.SendToAll(new S2C_63_0_16_NTC() { Unk0 = 2 });
            client.Party.SendToAll(new S2C_63_11_16_NTC() { StageNo = (uint) StageManager.ConvertIdToStageNo(stageId) });
#if false
            // S2C_63_2_16_NTC appears to have objective data inside of it
            var pcap2 = new S2CSituationDataUpdateObjectivesNtc.Serializer().Read(pcap2_data);
            client.Party.SendToAll(pcap2);
#endif
        }

        public virtual void DestroyEnemiesForBlock(GameClient client, QuestBlock questBlock)
        {
            foreach (var groupId in questBlock.EnemyGroupIds)
            {
                var enemyGroup = EnemyGroups[groupId];

                S2CInstanceEnemyGroupDestroyNtc destroyNtc = new S2CInstanceEnemyGroupDestroyNtc()
                {
                    LayoutId = enemyGroup.StageId.ToStageLayoutId()
                };

                client.Party.SendToAll(destroyNtc);
            }
        }

        public bool HasEnemiesInInCurrentStage(StageId stageId)
        {
            return UniqueEnemyGroups.Contains(stageId);
        }

        public virtual void PopulateStartingEnemyData(PartyQuestState partyQuestState)
        {
            var questState = partyQuestState.GetQuestState(this.QuestId);
            foreach (var processState in questState.ProcessState.Values)
            {
                if (processState.ProcessNo >= Processes.Count)
                {
                    continue;
                }

                var process = Processes[processState.ProcessNo];
                if (processState.BlockNo > process.Blocks.Count)
                {
                    // @note BlockNo counts from 1
                    continue;
                }

                foreach (var groupId in process.Blocks[processState.BlockNo - 1].EnemyGroupIds)
                {
                    var enemyGroup = EnemyGroups[groupId];
                    partyQuestState.SetInstanceEnemies(this, enemyGroup.StageId, (ushort)enemyGroup.SubGroupId, enemyGroup.CreateNewInstance());
                }
            }
        }

        public virtual void HandleOmInstantValue(GameClient client, ulong key, uint value)
        {
            // Remove the valid bit (that way json doesn't need to provide it)
            key = key & 0x7fffffffffffffff;
            foreach (var action in ServerActions)
            {
                if (action.Key == key && action.Value == value && action.ActionType == QuestSeverActionType.OmSetInstantValue)
                {
                    switch (action.OmInstantValueAction)
                    {
                        case OmInstantValueAction.ResetGroup:
                            client.Party.SendToAll(new S2CInstanceEnemyGroupResetNtc()
                            {
                                LayoutId = new CDataStageLayoutId()
                                {
                                    StageId = action.StageId.Id,
                                    GroupId = action.StageId.GroupId,
                                    LayerNo = action.StageId.LayerNo
                                }
                            });
                            break;
                    }
                }
            }
        }

        public List<QuestRewardItem> GetQuestRewards()
        {
            List<QuestRewardItem> rewards = new List<QuestRewardItem>();

            foreach (var reward in ItemRewards)
            {
                rewards.Add(reward);
            }

            foreach (var reward in SelectableRewards)
            {
                rewards.Add(reward);
            }

            return rewards;
        }

        public byte RandomRewardNum()
        {
            byte count = 0;
            foreach (var reward in ItemRewards)
            {
                if (reward.RewardType == QuestRewardType.Random)
                {
                    count += 1;
                }
            }

            return count;
        }

        public byte FixedRewardsNum()
        {
            byte count = 0;
            foreach (var reward in ItemRewards)
            {
                switch (reward.RewardType)
                {
                    case QuestRewardType.Fixed:
                    case QuestRewardType.Select:
                        count += 1;
                        break;
                }
            }
            return count;
        }

        public static List<CDataRewardBoxItem> AsCDataRewardBoxItems(QuestBoxRewards rewards)
        {
            List<CDataRewardBoxItem> results = new List<CDataRewardBoxItem>();

            Quest quest = QuestManager.GetRewardQuest(rewards.QuestId, rewards.VariantId);
            if (quest == null)
            {
                return new List<CDataRewardBoxItem>();
            }

            foreach (var reward in quest.SelectableRewards)
            {
                results.AddRange(reward.AsCDataRewardBoxItems());
            }

            List<QuestRandomRewardItem> randomRewards = new List<QuestRandomRewardItem>();
            foreach (var reward in quest.ItemRewards)
            {
                if (reward.RewardType != QuestRewardType.Random)
                {
                    results.AddRange(reward.AsCDataRewardBoxItems());
                }
                else
                {
                    randomRewards.Add((QuestRandomRewardItem)reward);
                }
            }

            foreach (var randomReward in rewards.RandomRewardIndices.Zip(randomRewards, Tuple.Create))
            {
                results.Add(randomReward.Item2.AsCDataRewardBoxItem(randomReward.Item1));
            }

            return results;
        }

        public QuestBoxRewards GenerateBoxRewards()
        {
            QuestBoxRewards obj = new QuestBoxRewards()
            {
                QuestId = QuestId,
                VariantId = VariantId
            };

            foreach (var reward in ItemRewards)
            {
                if (reward.RewardType == QuestRewardType.Random)
                {
                    var randomReward = (QuestRandomRewardItem)reward;
                    obj.RandomRewardIndices.Add(randomReward.Roll());
                }
            }
            obj.NumRandomRewards = obj.RandomRewardIndices.Count;

            return obj;
        }

        public List<CDataRewardItem> GetQuestFixedRewards()
        {
            List<CDataRewardItem> rewards = new List<CDataRewardItem>();

            foreach (var reward in ItemRewards)
            {
                rewards.AddRange(reward.AsCDataRewardItems());
            }

            return rewards;
        }

        public List<CDataRewardItem> GetQuestSelectableRewards()
        {
            List<CDataRewardItem> rewards = new List<CDataRewardItem>();

            foreach (var reward in SelectableRewards)
            {
                rewards.AddRange(reward.AsCDataRewardItems());
            }

            return rewards;
        }

        public List<CDataQuestOrderConditionParam> GetQuestOrderConditions()
        {
            List<CDataQuestOrderConditionParam> orderConditions = new List<CDataQuestOrderConditionParam>();

            foreach (var orderCondition in OrderConditions)
            {
                orderConditions.Add(orderCondition.ToCDataQuestOrderConditionParam());
            }

            return orderConditions;
        }

        public bool HasRewards()
        {
            return (ItemRewards.Count > 0) || (SelectableRewards.Count > 0);
        }

        public static void ParseQuestFlags(List<QuestFlag> questFlags, List<CDataQuestCommand> resultFlags, List<CDataQuestCommand> checkFlags)
        {
            foreach (var questFlag in questFlags)
            {
                switch (questFlag.Type)
                {
                    case QuestFlagType.QstLayout:
                        switch (questFlag.Action)
                        {
                            case QuestFlagAction.Set:
                                resultFlags.Add(QuestManager.ResultCommand.QstLayoutFlagOn(questFlag.Value));
                                break;
                            case QuestFlagAction.Clear:
                                resultFlags.Add(QuestManager.ResultCommand.QstLayoutFlagOff(questFlag.Value));
                                break;
                            case QuestFlagAction.CheckOn:
                            case QuestFlagAction.CheckOff:
                                /* Invalid for Layout flags */
                                return;
                        }
                        break;
                    case QuestFlagType.WorldManageLayout:
                        switch (questFlag.Action)
                        {
                            case QuestFlagAction.Set:
                                resultFlags.Add(QuestManager.ResultCommand.WorldManageLayoutFlagOn(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.Clear:
                                resultFlags.Add(QuestManager.ResultCommand.WorldManageLayoutFlagOff(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.CheckOn:
                            case QuestFlagAction.CheckOff:
                                /* Invalid for Layout flags */
                                return;
                        }
                        break;
                    case QuestFlagType.MyQst:
                        switch (questFlag.Action)
                        {
                            case QuestFlagAction.Set:
                                resultFlags.Add(QuestManager.ResultCommand.MyQstFlagOn(questFlag.Value));
                                break;
                            case QuestFlagAction.Clear:
                                resultFlags.Add(QuestManager.ResultCommand.MyQstFlagOff(questFlag.Value));
                                break;
                            case QuestFlagAction.CheckOn:
                                checkFlags.Add(QuestManager.CheckCommand.MyQstFlagOn(questFlag.Value));
                                break;
                            case QuestFlagAction.CheckOff:
                                checkFlags.Add(QuestManager.CheckCommand.MyQstFlagOff(questFlag.Value));
                                break;
                            case QuestFlagAction.CheckSetFromFsm:
                                checkFlags.Add(QuestManager.CheckCommand.MyQstFlagOnFromFsm(questFlag.Value));
                                break;
                        }
                        break;
                    case QuestFlagType.WorldManageQuest:
                        switch (questFlag.Action)
                        {
                            case QuestFlagAction.Set:
                                resultFlags.Add(QuestManager.ResultCommand.WorldManageQuestFlagOn(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.Clear:
                                resultFlags.Add(QuestManager.ResultCommand.WorldManageQuestFlagOff(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.CheckOn:
                                checkFlags.Add(QuestManager.CheckCommand.WorldManageQuestFlagOn(questFlag.Value, questFlag.QuestId));
                                break;
                            case QuestFlagAction.CheckOff:
                                checkFlags.Add(QuestManager.CheckCommand.WorldManageQuestFlagOn(questFlag.Value, questFlag.QuestId));
                                break;
                        }
                        break;
                }
            }
        }

        private static CDataQuestProcessState BlockAsCDataQuestProcessState(QuestBlock questBlock)
        {
            CDataQuestProcessState result = new CDataQuestProcessState()
            {
                ProcessNo = questBlock.ProcessNo,
                SequenceNo = questBlock.SequenceNo,
                BlockNo = questBlock.BlockNo,
            };

            List<CDataQuestCommand> resultCommands = new List<CDataQuestCommand>();
            List<CDataQuestCommand> checkCommands = new List<CDataQuestCommand>();

            ParseQuestFlags(questBlock.QuestFlags, resultCommands, checkCommands);

            result.ResultCommandList = resultCommands;
            if (checkCommands.Count > 0)
            {
                result.CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(checkCommands);
            }

            return result;
        }
    }

    public class QuestDoesNotExistException : ResponseErrorException
    {
        public QuestDoesNotExistException(QuestId questId) : base(ErrorCode.ERROR_CODE_QUEST_INTERNAL_ERROR, $"The quest ${questId} does not exist")
        {
        }
    }

    public class QuestRestoreProgressFailedException : ResponseErrorException
    {
        public QuestRestoreProgressFailedException(QuestId questId, uint step, uint stepsFound) : 
            base(ErrorCode.ERROR_CODE_QUEST_DIFFERENT_PROGRESS, $"Failed to restore progress for {questId} (Step({step}) != StepsFound({stepsFound}))")
        {
        }
    }
}
