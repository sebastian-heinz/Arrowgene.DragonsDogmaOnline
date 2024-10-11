using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections;
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

    public class QuestState
    {
        public QuestId QuestId { get; set; }
        public uint QuestScheduleId {  get; set; }
        public QuestType QuestType { get; set; }
        public QuestProgressState State { get; set; }
        public uint Step { get; set; }

        public Dictionary<ushort, QuestProcessState> ProcessState {  get; set; }
        public Dictionary<StageId, Dictionary<uint, List<InstancedEnemy>>> QuestEnemies {  get; set; }
        public Dictionary<uint, QuestDeliveryRecord> DeliveryRecords {  get; set; }

        public QuestState()
        {
            ProcessState = new Dictionary<ushort, QuestProcessState>();
            QuestEnemies = new Dictionary<StageId, Dictionary<uint, List<InstancedEnemy>>>();
            DeliveryRecords = new Dictionary<uint, QuestDeliveryRecord>();
        }

        public uint UpdateDeliveryRequest(uint itemId, uint amount)
        {
            lock (DeliveryRecords)
            {
                if (!DeliveryRecords.ContainsKey(itemId))
                {
                    // TODO: throw an exception?
                    return uint.MaxValue;
                }

                var deliveryRecord = DeliveryRecords[itemId];

                deliveryRecord.AmountDelivered += amount;

                if (deliveryRecord.AmountDelivered > deliveryRecord.AmountRequired)
                {
                    // This should never happen
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
    }

    public class PartyQuestState
    {

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(PartyQuestState));

        private Dictionary<uint, QuestState> ActiveQuests { get; set; }
        private Dictionary<StageId, HashSet<uint>> QuestLookupTable { get; set; }
        private List<QuestId> CompletedWorldQuests { get; set; }
        private Dictionary<QuestAreaId, HashSet<uint>> RolledInstanceWorldQuests { get; set; }

        public PartyQuestState()
        {
            ActiveQuests = new Dictionary<uint, QuestState>();
            QuestLookupTable = new Dictionary<StageId, HashSet<uint>>();
            CompletedWorldQuests = new List<QuestId>();
            RolledInstanceWorldQuests = new Dictionary<QuestAreaId, HashSet<uint>>();

            foreach (var areaId in Enum.GetValues<QuestAreaId>())
            {
                if (!RolledInstanceWorldQuests.ContainsKey(areaId))
                {
                    RolledInstanceWorldQuests[areaId] = new HashSet<uint>();
                }

                foreach (var questId in QuestManager.GetWorldQuestIdsByAreaId(areaId))
                {
                    RolledInstanceWorldQuests[areaId].Add(QuestManager.RollQuestForQuestId(questId).QuestScheduleId);
                }
            }
        }

        public Quest GetQuest(uint questScheduleId)
        {
            return QuestManager.GetQuestByScheduleId(questScheduleId);
        }

        public void AddNewQuest(Quest quest, uint step = 0)
        {
            lock (ActiveQuests)
            {
                if (quest == null)
                {
                    // Might be a removed quest (or one in development).
                    Logger.Error($"Unable to locate quest data.");
                    return;
                }

                ActiveQuests[quest.QuestScheduleId] = new QuestState()
                {
                    QuestId = quest.QuestId,
                    QuestScheduleId = quest.QuestScheduleId,
                    QuestType = quest.QuestType,
                    Step = step,
                };

                foreach (var stageId in quest.UniqueEnemyGroups)
                {
                    if (!QuestLookupTable.ContainsKey(stageId))
                    {
                        QuestLookupTable[stageId] = new HashSet<uint>();
                    }
                    QuestLookupTable[stageId].Add(quest.QuestScheduleId);
                }

                foreach (var location in quest.Locations)
                {
                    if (!QuestLookupTable.ContainsKey(location.StageId))
                    {
                        QuestLookupTable[location.StageId] = new HashSet<uint>();
                    }

                    QuestLookupTable[location.StageId].Add(quest.QuestScheduleId);

                    // Populate data structures for Instance Enemy Data
                    if (!ActiveQuests[quest.QuestScheduleId].QuestEnemies.ContainsKey(location.StageId))
                    {
                        ActiveQuests[quest.QuestScheduleId].QuestEnemies[location.StageId] = new Dictionary<uint, List<InstancedEnemy>>();
                    }
                }

                foreach (var request in quest.DeliveryItems)
                {
                    ActiveQuests[quest.QuestScheduleId].AddDeliveryRequest(request.ItemId, request.Amount);
                }

                // Initialize Process State Table
                UpdateProcessState(quest.QuestScheduleId, quest.ToCDataQuestList(step).QuestProcessStateList);

                // Initialize enemy data are the current point
                quest.PopulateStartingEnemyData(this);
            }
        }

        public void AddNewQuest(uint QuestScheduleId, uint step)
        {
            Quest quest = QuestManager.GetQuestByScheduleId(QuestScheduleId);
            AddNewQuest(quest, step);
        }

        public bool HasEnemiesInCurrentStageGroup(Quest quest, StageId stageId)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[quest.QuestScheduleId];
                return questState.QuestEnemies.ContainsKey(stageId);
            }
        }

        public bool HasEnemiesInCurrentStageGroup(Quest quest, StageId stageId, uint subGroupId)
        {
            lock (ActiveQuests)
            {
                if (!ActiveQuests.ContainsKey(quest.QuestScheduleId))
                {
                    return false;
                }

                var questState = ActiveQuests[quest.QuestScheduleId];
                if (!questState.QuestEnemies.ContainsKey(stageId))
                {
                    return false;
                }

                return questState.QuestEnemies[stageId].ContainsKey(subGroupId);
            }
        }

        public void SetInstanceEnemies(Quest quest, StageId stageId, ushort subGroupId, List<InstancedEnemy> enemies)
        {
            lock (ActiveQuests)
            {
                ActiveQuests[quest.QuestScheduleId].QuestEnemies[stageId][subGroupId] = enemies;
            }
        }

        public List<InstancedEnemy> GetInstancedEnemies(Quest quest, StageId stageId, ushort subGroupId)
        {
            lock (ActiveQuests)
            {
                if (!ActiveQuests.ContainsKey(quest.QuestScheduleId))
                {
                    Logger.Error($"No state for '{quest.QuestId}' present. Returning empty enemy list.");
                    return new List<InstancedEnemy>();
                }

                if (!ActiveQuests[quest.QuestScheduleId].QuestEnemies.ContainsKey(stageId))
                {
                    return new List<InstancedEnemy>();
                }

                if (!ActiveQuests[quest.QuestScheduleId].QuestEnemies[stageId].ContainsKey(subGroupId))
                {
                    return new List<InstancedEnemy>();
                }

                return ActiveQuests[quest.QuestScheduleId].QuestEnemies[stageId][subGroupId];
            }
        }

        public InstancedEnemy GetInstancedEnemy(Quest quest, StageId stageId, ushort subGroupId, uint index)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[quest.QuestScheduleId];
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

        public void RemoveQuest(uint questScheduleId)
        {
            var quest = GetQuest(questScheduleId);
            lock (ActiveQuests)
            {
                ActiveQuests.Remove(questScheduleId);
                foreach (var location in quest.Locations)
                {
                    if (QuestLookupTable.ContainsKey(location.StageId))
                    {
                        QuestLookupTable[location.StageId].Remove(questScheduleId);
                    }
                }
            }
        }

        public void RemoveInactiveWorldQuests()
        {
            lock (ActiveQuests)
            {
                var questsToRemove = new List<uint>();
                foreach (var (questScheduleId, questState) in ActiveQuests)
                {
                    if (QuestManager.IsWorldQuest(questState.QuestId) && questState.Step == 0)
                    {
                        questsToRemove.Add(questScheduleId);
                    }
                }

                foreach (var questScheduleId in questsToRemove)
                {
                    RemoveQuest(questScheduleId);
                }
            }
        }

        public void CancelQuest(uint questScheduleId)
        {
            var quest = GetQuest(questScheduleId);
            RemoveQuest(questScheduleId);
        }

        public void CompleteQuest(uint questScheduleId)
        {
            var quest = GetQuest(questScheduleId);
            RemoveQuest(questScheduleId);

            if (QuestManager.IsWorldQuest(quest))
            {
                CompletedWorldQuests.Add(quest.QuestId);
                RolledInstanceWorldQuests[quest.QuestAreaId].Remove(questScheduleId);
            }

            if (quest.NextQuestId != 0)
            {
                var nextQuest = QuestManager.RollQuestForQuestId(quest.NextQuestId);
                AddNewQuest(nextQuest.QuestScheduleId, 0);
            }
        }

        public bool IsCompletedWorldQuest(uint questScheduleId)
        {
            var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
            return IsCompletedWorldQuest(quest.QuestId);
        }

        public bool IsCompletedWorldQuest(QuestId questId)
        {
            return CompletedWorldQuests.Contains(questId);
        }

        public bool IsQuestActive(uint questScheduleId)
        {
            var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
            if (quest == null)
            {
                return false;
            }

            return IsQuestActive(quest.QuestId);
        }

        public bool IsQuestActive(QuestId questId)
        {
            lock (ActiveQuests)
            {
                var questVariants = QuestManager.GetQuestsByQuestId(questId);
                foreach (var questVarient in questVariants)
                {
                    if (ActiveQuests.ContainsKey(questVarient.QuestScheduleId))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public HashSet<uint> GetActiveQuestScheduleIds()
        {
            lock (ActiveQuests)
            {
                return ActiveQuests.Values.Select(x => x.QuestScheduleId).ToHashSet();
            }
        }

        public bool HasActiveQuest(uint questScheduleId)
        {
            lock (ActiveQuests)
            {
                return ActiveQuests.ContainsKey(questScheduleId);
            }
        }

        public HashSet<uint> StageQuests(StageId stageId)
        {
            lock (ActiveQuests)
            {
                if (QuestLookupTable.ContainsKey(stageId))
                {
                    return QuestLookupTable[stageId];
                }
            }
            return new HashSet<uint>();
        }

        public HashSet<uint> AreaQuests(QuestAreaId areaId)
        {
            lock (ActiveQuests)
            {
                if (RolledInstanceWorldQuests.ContainsKey(areaId))
                {
                    return RolledInstanceWorldQuests[areaId];
                }
                return new HashSet<uint>();
            }
        }

        public QuestState GetQuestState(uint questScheduleId)
        {
            lock (ActiveQuests)
            {
                if (!ActiveQuests.ContainsKey(questScheduleId))
                {
                    return null;
                }

                return ActiveQuests[questScheduleId];
            }
        }

        public QuestState GetQuestState(Quest quest)
        {
            return GetQuestState(quest.QuestScheduleId);
        }

        public void UpdateProcessState(uint questScheduleId, ushort processNo, ushort sequenceNo, ushort blockNo)
        {
            lock (ActiveQuests)
            {
                if (!ActiveQuests.ContainsKey(questScheduleId))
                {
                    return;
                }

                if (!ActiveQuests[questScheduleId].ProcessState.ContainsKey(processNo))
                {
                    ActiveQuests[questScheduleId].ProcessState[processNo] = new QuestProcessState();
                }

                var processState = ActiveQuests[questScheduleId].ProcessState[processNo];
                processState.ProcessNo = processNo;
                processState.SequenceNo = sequenceNo;
                processState.BlockNo = blockNo;
            }
        }

        public void UpdateProcessState(uint questScheduleId, List<CDataQuestProcessState> questProcessState)
        {
            foreach (var process in questProcessState)
            {
                UpdateProcessState(questScheduleId, process.ProcessNo, process.SequenceNo, process.BlockNo);
            }
        }

        public QuestProcessState GetProcessState(uint questScheduleId, ushort processNo)
        {
            lock (ActiveQuests)
            {
                if (!ActiveQuests.ContainsKey(questScheduleId))
                {
                    ActiveQuests[questScheduleId] = new QuestState();
                }

                if (!ActiveQuests[questScheduleId].ProcessState.ContainsKey(processNo))
                {
                    ActiveQuests[questScheduleId].ProcessState[processNo] = new QuestProcessState() { ProcessNo = processNo, BlockNo = 1 };
                }

                return ActiveQuests[questScheduleId].ProcessState[processNo];
            }
        }

        public bool UpdatePartyQuestProgress(DdonGameServer server, PartyGroup party, uint questScheduleId)
        {
            var questState = party.QuestState.GetQuestState(questScheduleId);
            foreach (var memberClient in party.Clients)
            {
                var result = server.Database.GetQuestProgressByScheduleId(memberClient.Character.CommonId, questState.QuestScheduleId);
                if (result == null)
                {
                    continue;
                }

                if (result.Step != questState.Step)
                {
                    continue;
                }

                server.Database.UpdateQuestProgress(memberClient.Character.CommonId, questState.QuestScheduleId, questState.QuestType, questState.Step + 1);
            }

            questState.Step += 1;

            return true;
        }

        public bool CompletePartyQuestProgress(DdonGameServer server, PartyGroup party, uint questScheduleId)
        {
            Quest quest = GetQuest(questScheduleId);

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

                var result = server.Database.GetQuestProgressByScheduleId(memberClient.Character.CommonId, questScheduleId);
                if (result == null)
                {
                    continue;
                }

                if (result.Step != questState.Step)
                {
                    continue;
                }

                server.Database.DeletePriorityQuest(memberClient.Character.CommonId, questScheduleId);
                server.Database.RemoveQuestProgress(memberClient.Character.CommonId, questScheduleId, quest.QuestType);
                if (quest.NextQuestId != QuestId.None)
                {
                    var nextQuest = GetQuest((uint) quest.NextQuestId);
                    server.Database.InsertQuestProgress(memberClient.Character.CommonId, nextQuest.QuestScheduleId, nextQuest.QuestType, 0);
                }

                if (!memberClient.Character.CompletedQuests.ContainsKey(quest.QuestId))
                {
                    memberClient.Character.CompletedQuests.Add(quest.QuestId, new CompletedQuest()
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
            CompleteQuest(quest.QuestScheduleId);

            return true;
        }

        public bool DistributePartyQuestRewards(DdonGameServer server, PartyGroup party, uint questScheduleId)
        {
            Quest quest = GetQuest(questScheduleId);

            var questState = party.QuestState.GetQuestState(quest);
            foreach (var memberClient in party.Clients)
            {
                // If this is a main quest, check to see that the member is currently on this quest, otherwise don't reward
                if (quest.QuestType == QuestType.Main)
                {
                    var result = server.Database.GetQuestProgressByScheduleId(memberClient.Character.CommonId, quest.QuestScheduleId);
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

            var priorityQuestScheduleIds = server.Database.GetPriorityQuestScheduleIds(leaderClient.Character.CommonId);
            foreach (var priorityQuestScheduleId in priorityQuestScheduleIds)
            {
                var priorityQuest = GetQuest(priorityQuestScheduleId);
                var questState = party.QuestState.GetQuestState(priorityQuestScheduleId);
                prioNtc.PriorityQuestList.Add(priorityQuest.ToCDataPriorityQuest(questState.Step));
            }
            party.SendToAll(prioNtc);
        }

        public void ResetInstance()
        {
            lock (ActiveQuests)
            {
                foreach (var questId in CompletedWorldQuests)
                {
                    var quest = QuestManager.RollQuestForQuestId(questId);
                    RolledInstanceWorldQuests[quest.QuestAreaId].Add(quest.QuestScheduleId);
                }
                CompletedWorldQuests.Clear();
            }
        }
    }
}
