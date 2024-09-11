using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Party
{
    public class QuestProcessState
    {
        public ushort ProcessNo { get; set; }
        public ushort SequenceNo { get; set; }
        public ushort BlockNo { get; set; }
    }

    public class QuestDeliveryRecord
    {
        public uint ItemId { get; set; }
        public uint AmountDelivered { get; set; }
        public uint AmountRequired { get; set; }
    }

    public class QuestEnemyHuntRecord
    {
        public ushort ProcessNo { get; set; }
        public ushort SequenceNo { get; set; }
        public ushort BlockNo { get; set; }
        public uint EnemyId { get; set; }
        public uint MinimumLevel { get; set; }
        public uint AmountHunted { get; set; }
        public uint AmountRequired { get; set; }
    }

    public class QuestState
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestState));

        public QuestId QuestId { get; set; }
        public QuestType QuestType { get; set; }
        public QuestProgressState State { get; set; }
        public bool HasStarted { get; set; }
        public uint Step { get; set; }

        public Dictionary<ushort, QuestProcessState> ProcessState {  get; set; }
        public Dictionary<StageId, Dictionary<uint, List<InstancedEnemy>>> QuestEnemies {  get; set; }
        public Dictionary<uint, QuestDeliveryRecord> DeliveryRecords {  get; set; }
        public Dictionary<uint, QuestEnemyHuntRecord> HuntRecords { get; set; }

        public QuestState()
        {
            ProcessState = new Dictionary<ushort, QuestProcessState>();
            QuestEnemies = new Dictionary<StageId, Dictionary<uint, List<InstancedEnemy>>>();
            DeliveryRecords = new Dictionary<uint, QuestDeliveryRecord>();
            HuntRecords = new Dictionary<uint, QuestEnemyHuntRecord>();
        }

        public uint UpdateDeliveryRequest(uint itemId, uint amount)
        {
            lock (DeliveryRecords)
            {
                if (!DeliveryRecords.ContainsKey(itemId))
                {
                    Logger.Error($"Missing delivery record {itemId} for quest {QuestId}");
                    return uint.MaxValue;
                }

                var deliveryRecord = DeliveryRecords[itemId];

                deliveryRecord.AmountDelivered += amount;

                if (deliveryRecord.AmountDelivered > deliveryRecord.AmountRequired)
                {
                    Logger.Error($"Delivery overage {itemId} for quest {QuestId}");
                    return uint.MaxValue;
                }

                return deliveryRecord.AmountRequired - deliveryRecord.AmountDelivered;
            }
        }

        public void AddDeliveryRequest(uint itemId, uint amountRequired)
        {
            lock (DeliveryRecords)
            {
                DeliveryRecords[itemId] = new QuestDeliveryRecord()
                {
                    ItemId = itemId,
                    AmountRequired = amountRequired,
                    AmountDelivered = 0
                };
            }
        }

        public bool DeliveryRequestComplete()
        {
            lock (DeliveryRecords)
            {
                foreach (var record in DeliveryRecords.Values)
                {
                    if (record.AmountDelivered != record.AmountRequired)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public void AddHuntRequest(QuestEnemyHunt hunt)
        {
            lock (HuntRecords)
            {
                HuntRecords[hunt.EnemyId] = new QuestEnemyHuntRecord()
                {
                    ProcessNo = hunt.ProcessNo,
                    SequenceNo = hunt.SequenceNo,
                    BlockNo = hunt.BlockNo,

                    EnemyId = hunt.EnemyId,
                    MinimumLevel = hunt.MinimumLevel,
                    AmountRequired = hunt.Amount,
                    AmountHunted = 0
                };
            }
        }

        public QuestEnemyHuntRecord? UpdateHuntRequest(Enemy enemy)
        {
            var enemyId = enemy.UINameId;
            lock (HuntRecords)
            {
                if (!HuntRecords.ContainsKey(enemyId))
                {
                    return null;
                }

                var huntRecord = HuntRecords[enemyId];

                if (enemy.Lv < huntRecord.MinimumLevel)
                {
                    return null;
                }

                huntRecord.AmountHunted++;
                return huntRecord;
            }
        }
    }

    public class PartyQuestState
    {

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyQuestState));

        private Dictionary<QuestId, QuestState> ActiveQuests { get; set; }
        private Dictionary<StageId, List<QuestId>> QuestLookupTable { get; set; }
        private Dictionary<QuestId, uint> ActiveVariantQuests { get; set; }
        // For the purposes of each party quest state knowing the possible variant quests
        private HashSet<QuestId> VariantQuests { get; set; }
        public HashSet<QuestId> CompletedWorldQuests { get; set; }

        public PartyQuestState()
        {
            ActiveQuests = new Dictionary<QuestId, QuestState>();
            QuestLookupTable = new Dictionary<StageId, List<QuestId>>();
            CompletedWorldQuests = new HashSet<QuestId>();
            ActiveVariantQuests = new Dictionary<QuestId, uint>();
            VariantQuests = QuestManager.GetAllVariantQuestIds();
        }

        public void SetHasStarted(QuestId questId, bool activeState)
        {
            ActiveQuests[questId].HasStarted = activeState;
        }

        public Quest GetQuest(QuestId questId)
        {
            if (ActiveVariantQuests.ContainsKey(questId))
            {
                // Look inside the ActiveVariantQuests and get the quest variant id to be used to get back the specific quest.
                return QuestManager.GetQuest(questId, ActiveVariantQuests[questId]);
            }

            return QuestManager.GetQuest(questId);
        }

        public void AddNewQuest(Quest quest, uint step = 0, bool questStarted = false)
        {
            lock (ActiveQuests)
            {
                ActiveQuests[quest.QuestId] = new QuestState()
                {
                    QuestId = quest.QuestId,
                    QuestType = quest.QuestType,
                    Step = step,
                    HasStarted = questStarted
                };

                foreach (var location in quest.Locations)
                {
                    if (!QuestLookupTable.ContainsKey(location.StageId))
                    {
                        QuestLookupTable[location.StageId] = new List<QuestId>();
                    }

                    QuestLookupTable[location.StageId].Add(quest.QuestId);

                    // Populate data structures for Instance Enemy Data
                    if (!ActiveQuests[quest.QuestId].QuestEnemies.ContainsKey(location.StageId))
                    {
                        ActiveQuests[quest.QuestId].QuestEnemies[location.StageId] = new Dictionary<uint, List<InstancedEnemy>>();
                    }
                }

                foreach (var request in quest.DeliveryItems)
                {
                    ActiveQuests[quest.QuestId].AddDeliveryRequest(request.ItemId, request.Amount);
                }
                foreach (var request in quest.EnemyHunts)
                {
                    ActiveQuests[quest.QuestId].AddHuntRequest(request);
                }

                // Initialize Process State Table
                UpdateProcessState(quest.QuestId, quest.ToCDataQuestList(step).QuestProcessStateList);

                // Initialize enemy data are the current point
                quest.PopulateStartingEnemyData(this);
            }
        }

        public bool HasEnemiesInCurrentStageGroup(Quest quest, StageId stageId)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[quest.QuestId];
                return questState.QuestEnemies.ContainsKey(stageId);
            }
        }

        public void SetInstanceEnemies(Quest quest, StageId stageId, ushort subGroupId, List<InstancedEnemy> enemies)
        {
            lock (ActiveQuests)
            {
                ActiveQuests[quest.QuestId].QuestEnemies[stageId][subGroupId] = enemies;
            }
        }

        public List<InstancedEnemy> GetInstancedEnemies(Quest quest, StageId stageId, ushort subGroupId)
        {
            lock (ActiveQuests)
            {
                if (!ActiveQuests.ContainsKey(quest.QuestId))
                {
                    Logger.Error($"No state for '{quest.QuestId}' present. Returning empty enemy list.");
                    return new List<InstancedEnemy>();
                }

                if (!ActiveQuests[quest.QuestId].QuestEnemies.ContainsKey(stageId))
                {
                    return new List<InstancedEnemy>();
                }

                if (!ActiveQuests[quest.QuestId].QuestEnemies[stageId].ContainsKey(subGroupId))
                {
                    return new List<InstancedEnemy>();
                }

                return ActiveQuests[quest.QuestId].QuestEnemies[stageId][subGroupId];
            }
        }

        public List<InstancedEnemy> GetInstancedEnemies(QuestId questId, StageId stageId, ushort subGroupId)
        {
            var quest = GetQuest(questId);
            return GetInstancedEnemies(quest, stageId, subGroupId);
        }

        public InstancedEnemy GetInstancedEnemy(Quest quest, StageId stageId, ushort subGroupId, uint index)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[quest.QuestId];
                foreach (var enemy in questState.QuestEnemies[stageId][subGroupId])
                {
                    if (enemy.Index == index)
                    {
                        return enemy;
                    }
                }

                return null;
            }
        }

        public InstancedEnemy GetInstancedEnemy(QuestId questId, StageId stageId, ushort subGroupId, uint index)
        {
            var quest = GetQuest(questId);
            return GetInstancedEnemy(quest, stageId, subGroupId, index);
        }

        public void AddNewQuest(QuestId questId, uint step, bool questStarted, uint variantId)
        {
            Quest quest = QuestManager.GetQuest(questId, variantId);

            if (VariantQuests.Contains(questId))
            {
                ActiveVariantQuests[questId] = (uint)quest.VariantId;
                AddNewQuest(quest, step, questStarted);
            }
        }

        public void AddNewQuest(QuestId questId, uint step, bool questStarted)
        {
            // Trying to add a new variant quest before properly removing it will cause an exception.

            if (ActiveVariantQuests.ContainsKey(questId))
            {
                return;
            }

            Quest quest;

            // If the quest we are trying to add is a variant quest, then roll and get a random version.
            if (VariantQuests.Contains(questId))
            {
                quest = QuestManager.GetQuest(questId, QuestManager.GetRandomVariantId(questId));
            }
            else
            {
                quest = GetQuest(questId);
            }

            if (quest == null)
            {
                // Might be progress from removed quest (or one in development).
                Logger.Error($"Unable to locate quest data for {questId}");
                return;
            }

            // If we are adding a new variant quest, then log the variant id for further reference
            if (quest.IsVariantQuest)
            {
                ActiveVariantQuests.Add(quest.QuestId, quest.VariantId);
            }

            AddNewQuest(quest, step, questStarted);
        }

        public void RemoveQuest(QuestId questId)
        {
            var quest = GetQuest(questId);
            lock (ActiveQuests)
            {
                ActiveQuests.Remove(questId);

                if (ActiveVariantQuests.ContainsKey(questId))
                {
                    ActiveVariantQuests.Remove(questId);
                }

                foreach (var location in quest.Locations)
                {
                    if (QuestLookupTable.ContainsKey(location.StageId))
                    {
                        QuestLookupTable[location.StageId].Remove(questId);
                    }
                }
            }
        }

        public void RemoveInactiveWorldQuests()
        {
            lock (ActiveQuests)
            {
                var questsToRemove = new List<QuestId>();
                foreach (var (questId, quest) in ActiveQuests)
                {
                    if (quest.QuestType == QuestType.World && quest.Step == 0)
                    {
                        questsToRemove.Add(questId);
                    }
                }

                foreach (var questId in questsToRemove)
                {
                    RemoveQuest(questId);
                }
            }
        }

        public void CancelQuest(QuestId questId)
        {
            var quest = GetQuest(questId);
            RemoveQuest(questId);
        }

        public void CompleteQuest(QuestId questId)
        {
            var quest = GetQuest(questId);
            RemoveQuest(questId);

            if (QuestManager.IsWorldQuest(questId))
            {
                CompletedWorldQuests.Add(questId);
            }

            if (quest.NextQuestId != 0)
            {
                AddNewQuest(quest.NextQuestId, 0, false);
            }
        }

        public bool IsCompletedWorldQuest(QuestId questId)
        {
            return CompletedWorldQuests.Contains(questId);
        }

        public List<QuestId> GetActiveQuestIds()
        {
            lock (ActiveQuests)
            {
                return ActiveQuests.Keys.ToList();
            }
        }

        public bool HasActiveQuest(QuestId questId)
        {
            lock (ActiveQuests)
            {
                return ActiveQuests.ContainsKey(questId);
            }
        }

        public List<QuestId> StageQuests(StageId stageId)
        {
            lock (ActiveQuests)
            {
                if (QuestLookupTable.ContainsKey(stageId))
                {
                    return QuestLookupTable[stageId];
                }
            }
            return new List<QuestId>();
        }

        public bool HasQuest(QuestId questId)
        {
            lock (ActiveQuests)
            {
                return ActiveQuests.ContainsKey(questId);
            }
        }

        public QuestState GetQuestState(Quest quest)
        {
            lock (ActiveQuests)
            {
                if (!ActiveQuests.ContainsKey(quest.QuestId))
                {
                    // Throw an exception?
                    return null;
                }

                return ActiveQuests[quest.QuestId];
            }
        }

        public QuestState GetQuestState(QuestId questId)
        {
            var quest = GetQuest(questId);
            return GetQuestState(quest);
        }

        public void UpdateProcessState(QuestId questId, ushort processNo, ushort sequenceNo, ushort blockNo)
        {
            lock (ActiveQuests)
            {
                if (!ActiveQuests.ContainsKey(questId))
                {
                    return;
                }

                if (!ActiveQuests[questId].ProcessState.ContainsKey(processNo))
                {
                    ActiveQuests[questId].ProcessState[processNo] = new QuestProcessState();
                }

                var processState = ActiveQuests[questId].ProcessState[processNo];
                processState.ProcessNo = processNo;
                processState.SequenceNo = sequenceNo;
                processState.BlockNo = blockNo;
            }
        }

        public void UpdateProcessState(QuestId questId, List<CDataQuestProcessState> questProcessState)
        {
            foreach (var process in questProcessState)
            {
                UpdateProcessState(questId, process.ProcessNo, process.SequenceNo, process.BlockNo);
            }
        }

        public QuestProcessState GetProcessState(QuestId questId, ushort processNo)
        {
            lock (ActiveQuests)
            {
                if (!ActiveQuests.ContainsKey(questId))
                {
                    ActiveQuests[questId] = new QuestState();
                }

                if (!ActiveQuests[questId].ProcessState.ContainsKey(processNo))
                {
                    ActiveQuests[questId].ProcessState[processNo] = new QuestProcessState() { ProcessNo = processNo, BlockNo = 1 };
                }

                return ActiveQuests[questId].ProcessState[processNo];
            }
        }

        private void RerollUnfoundAltQuests()
        {
            // 1. Check all Active variant quests and see if they have started.

            foreach (var variantQuest in VariantQuests)
            {
                // 1. Check if the variant quest is not in either active quest lists
                if (!ActiveVariantQuests.ContainsKey(variantQuest) && !ActiveQuests.ContainsKey(variantQuest))
                {
                    // 2. Add a new variant quest if none were found.
                    AddNewQuest(variantQuest, 0, false);
                    continue;
                }

                // 3. Check if the quest exists within the larger active quest list
                if (ActiveQuests.ContainsKey(variantQuest))
                    switch (ActiveQuests[variantQuest].HasStarted)
                    {
                        // 4. If the quest is started, leave it alone, if not remove and add a new random quest.
                        case true:
                            continue;
                        case false:
                            RemoveQuest(variantQuest);
                            AddNewQuest(variantQuest, 0, false);
                            continue;
                    }
            }
        }

        public bool UpdatePartyQuestProgress(DdonGameServer server, PartyGroup party, QuestId questId)
        {
            Quest quest = GetQuest(questId);

            var questState = party.QuestState.GetQuestState(quest);
            foreach (var memberClient in party.Clients)
            {
                var result = server.Database.GetQuestProgressById(memberClient.Character.CommonId, questId);
                if (result == null)
                {
                    continue;
                }

                if (result.Step != questState.Step)
                {
                    continue;
                }

                server.Database.UpdateQuestProgress(memberClient.Character.CommonId, quest.QuestId, quest.QuestType, questState.Step + 1);
            }

            questState.Step += 1;

            return true;
        }

        public bool CompletePartyQuestProgress(DdonGameServer server, PartyGroup party, QuestId questId)
        {
            Quest quest = GetQuest(questId);

            var questState = party.QuestState.GetQuestState(quest);
            foreach (var memberClient in party.Clients)
            {
                // Special case for Exteme Missions where there is no state saved
                // Tracking completion matters for progress and weekly reward limits
                if (quest.QuestType == QuestType.ExtremeMission)
                {
                    var completedQuests = memberClient.Character.CompletedQuests;
                    if (!completedQuests.ContainsKey(quest.QuestId))
                    {
                        completedQuests.Add(quest.QuestId, new CompletedQuest()
                        {
                            QuestId = quest.QuestId,
                            QuestType = quest.QuestType,
                            ClearCount = 1,
                        });
                    }
                    else
                    {
                        completedQuests[quest.QuestId].ClearCount += 1;
                    }
                        
                    server.Database.ReplaceCompletedQuest(memberClient.Character.CommonId, quest.QuestId, quest.QuestType, completedQuests[quest.QuestId].ClearCount);
                    continue;
                }

                var result = server.Database.GetQuestProgressById(memberClient.Character.CommonId, quest.QuestId);
                if (result == null)
                {
                    continue;
                }

                if (result.Step != questState.Step)
                {
                    continue;
                }

                server.Database.DeletePriorityQuest(memberClient.Character.CommonId, quest.QuestId);
                server.Database.RemoveQuestProgress(memberClient.Character.CommonId, quest.QuestId, quest.QuestType);
                if (quest.NextQuestId != QuestId.None)
                {
                    var nextQuest = GetQuest(quest.NextQuestId);
                    server.Database.InsertQuestProgress(memberClient.Character.CommonId, nextQuest.QuestId, nextQuest.QuestType, 0);
                }

                if (!memberClient.Character.CompletedQuests.ContainsKey(quest.QuestId))
                {
                    memberClient.Character.CompletedQuests.Add(questId, new CompletedQuest()
                    {
                        QuestId = quest.QuestId,
                        QuestType = quest.QuestType,
                        ClearCount = 1,
                    });
                    server.Database.InsertIfNotExistCompletedQuest(memberClient.Character.CommonId, quest.QuestId, quest.QuestType);
                }
                else
                {
                    uint clearCount = ++memberClient.Character.CompletedQuests[quest.QuestId].ClearCount;
                    server.Database.ReplaceCompletedQuest(memberClient.Character.CommonId, quest.QuestId, quest.QuestType, clearCount);
                }
            }

            // Remove the quest data from the party object
            CompleteQuest(quest.QuestId);

            return true;
        }

        public bool DistributePartyQuestRewards(DdonGameServer server, PartyGroup party, QuestId questId)
        {
            Quest quest = GetQuest(questId);

            var questState = party.QuestState.GetQuestState(quest);
            foreach (var memberClient in party.Clients)
            {
                // If this is a main quest, check to see that the member is currently on this quest, otherwise don't reward
                if (quest.QuestType == QuestType.Main)
                {
                    var result = server.Database.GetQuestProgressById(memberClient.Character.CommonId, questId);
                    if (result == null)
                    {
                        continue;
                    }

                    if (result.Step != questState.Step)
                    {
                        continue;
                    }
                }

                // Check for Item Rewards
                if (quest.HasRewards())
                {
                    server.RewardManager.AddQuestRewards(memberClient, quest);
                }

                // Check for Exp, Rift and Gold Rewards
                SendWalletRewards(server, memberClient, quest);
            }

            return true;
        }

        private void SendWalletRewards(DdonGameServer server, GameClient client, Quest quest)
        {
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.Quest
            };

            foreach (var walletReward in quest.WalletRewards)
            {
                server.WalletManager.AddToWallet(client.Character, walletReward.Type, walletReward.Value);

                updateCharacterItemNtc.UpdateWalletList.Add(new CDataUpdateWalletPoint()
                {
                    Type = walletReward.Type,
                    Value = server.WalletManager.GetWalletAmount(client.Character, walletReward.Type),
                    AddPoint = (int)walletReward.Value
                });
            }

            if (updateCharacterItemNtc.UpdateWalletList.Count > 0)
            {
                client.Send(updateCharacterItemNtc);
            }

            foreach (var expPoint in quest.ExpRewards)
            {
                if (expPoint.Type == ExpType.ExperiencePoints)
                {
                    server.ExpManager.AddExp(client, client.Character, expPoint.Reward, RewardSource.Quest, quest.QuestType);
                }
                else if (expPoint.Type == ExpType.JobPoints)
                {
                    server.ExpManager.AddJp(client, client.Character, expPoint.Reward, RewardSource.Quest, quest.QuestType);
                }
                else if (expPoint.Type == ExpType.PlayPoints)
                {
                    server.PPManager.AddPlayPoint(client, expPoint.Reward, type: 2);
                }
            }
        }

        public void UpdatePriorityQuestList(DdonGameServer server, GameClient requestingClient, PartyGroup party)
        {
            if (party.Leader is null || requestingClient != party.Leader.Client)
            {
                return;
            }

            var leaderClient = party.Leader.Client;

            S2CQuestSetPriorityQuestNtc prioNtc = new S2CQuestSetPriorityQuestNtc()
            {
                CharacterId = leaderClient.Character.CharacterId
            };

            var priorityQuests = server.Database.GetPriorityQuests(leaderClient.Character.CommonId);
            foreach (var priorityQuestId in priorityQuests)
            {
                var priorityQuest = GetQuest(priorityQuestId);
                var questState = party.QuestState.GetQuestState(priorityQuest.QuestId);
                prioNtc.PriorityQuestList.Add(priorityQuest.ToCDataPriorityQuest(questState.Step));
            }
            party.SendToAll(prioNtc);
        }

        public void ResetInstance()
        {
            lock (ActiveQuests)
            {
                CompletedWorldQuests.Clear();

                RerollUnfoundAltQuests();
            }
        }
    
        public void HandleEnemyHuntRequests(GameClient client, Enemy enemy)
        {
            foreach (var quest in ActiveQuests)
            {
                QuestEnemyHuntRecord huntRecord = quest.Value.UpdateHuntRequest(enemy);

                if (huntRecord != null)
                {
                    S2CQuestQuestProgressWorkSaveNtc ntc = new()
                    {
                        QuestScheduleId = (uint)QuestManager.GetQuest(quest.Key).QuestScheduleId,
                        ProcessNo = huntRecord.ProcessNo,
                        SequenceNo = huntRecord.SequenceNo,
                        BlockNo = huntRecord.BlockNo
                    };
                    ntc.WorkList.Add(new CDataQuestProgressWork()
                    {
                        CommandNo = (uint)QuestNotifyCommand.KilledEnemyLight,
                        Work01 = (int)huntRecord.EnemyId,
                        Work02 = (int)huntRecord.MinimumLevel,
                        Work03 = (int)huntRecord.AmountHunted
                    });

                    client.Send(ntc);
                }
            }
        }
    }
}
