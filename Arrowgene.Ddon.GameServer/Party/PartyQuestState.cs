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
        public uint AmountRequired {  get; set; }
    }

    public class QuestState
    {
        public QuestId QuestId { get; set; }
        public QuestType QuestType { get; set; }
        public QuestProgressState State { get; set; }
        public uint Step {  get; set; }

        public Dictionary<ushort, QuestProcessState> ProcessState {  get; set; }
        public Dictionary<StageId, Dictionary<uint, List<InstancedEnemy>>> QuestEnemies {  get; set; }
        public Dictionary<StageId, ushort> CurrentSubgroup { get; set; }

        public Dictionary<uint, QuestDeliveryRecord> DeliveryRecords {  get; set; }

        public QuestState()
        {
            ProcessState = new Dictionary<ushort, QuestProcessState>();
            QuestEnemies = new Dictionary<StageId, Dictionary<uint, List<InstancedEnemy>>>();
            DeliveryRecords = new Dictionary<uint, QuestDeliveryRecord>();
            CurrentSubgroup = new Dictionary<StageId, ushort>();
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

        private Dictionary<QuestId, QuestState> ActiveQuests { get; set; }
        private Dictionary<StageId, List<QuestId>> QuestLookupTable { get; set; }
        private List<QuestId> CompletedWorldQuests {  get; set; }

        public PartyQuestState()
        {
            ActiveQuests = new Dictionary<QuestId, QuestState>();
            QuestLookupTable = new Dictionary<StageId, List<QuestId>>();
            CompletedWorldQuests = new List<QuestId>();
        }

        public void AddNewQuest(Quest quest, uint step = 0)
        {
            lock (ActiveQuests) 
            {
                ActiveQuests[quest.QuestId] = new QuestState()
                {
                    QuestId = quest.QuestId,
                    QuestType = quest.QuestType,
                    Step = step
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

                // Initialize Process State Table
                UpdateProcessState(quest.QuestId, quest.ToCDataQuestList(step).QuestProcessStateList);

                // Initialize enemy data are the current point
                quest.PopulateStartingEnemyData(this);
            }
        }

        public bool HasEnemiesInCurrentStageGroup(Quest quest, StageId stageId, uint subGroupId)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[quest.QuestId];
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
                ActiveQuests[quest.QuestId].QuestEnemies[stageId][subGroupId] = enemies;
            }
        }

        public List<InstancedEnemy> GetInstancedEnemies(Quest quest, StageId stageId, ushort subGroupId)
        {
            lock (ActiveQuests)
            {
                return ActiveQuests[quest.QuestId].QuestEnemies[stageId][subGroupId];
            }
        }

        public List<InstancedEnemy> GetInstancedEnemies(QuestId questId, StageId stageId, ushort subGroupId)
        {
            var quest = QuestManager.GetQuest(questId);
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
            var quest = QuestManager.GetQuest(questId);
            return GetInstancedEnemy(quest, stageId, subGroupId, index);
        }

        public void SetInstanceSubgroupId(Quest quest, StageId stageId, ushort subgroupId)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[quest.QuestId];
                questState.CurrentSubgroup[stageId] = subgroupId;
            }
        }

        public ushort GetInstanceSubgroupId(Quest quest, StageId stageId)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[quest.QuestId];
                if (!questState.CurrentSubgroup.ContainsKey(stageId))
                {
                    return 0;
                }
                return questState.CurrentSubgroup[stageId];
            }
        }

        public void AddNewQuest(QuestId questId, uint step)
        {
            var quest = QuestManager.GetQuest(questId);
            AddNewQuest(quest, step);
        }

        public void RemoveQuest(QuestId questId)
        {
            var quest = QuestManager.GetQuest(questId);
            lock (ActiveQuests)
            {
                ActiveQuests.Remove(questId);

                foreach (var location in quest.Locations)
                {
                    if (QuestLookupTable.ContainsKey(location.StageId))
                    {
                        QuestLookupTable[location.StageId].Remove(questId);
                    }
                }
            }
        }

        public void CancelQuest(QuestId questId)
        {
            lock (CompletedWorldQuests)
            {
                var quest = QuestManager.GetQuest(questId);
                RemoveQuest(questId);

                // Save the quest if it was a world quest
                // so we can add it back on instance reset
                if (quest.QuestType == QuestType.World)
                {
                    CompletedWorldQuests.Add(questId);
                }
            }
        }

        public void CompleteQuest(QuestId questId)
        {
            lock (CompletedWorldQuests)
            {
                var quest = QuestManager.GetQuest(questId);
                RemoveQuest(questId);

                // Save the quest if it was a world quest
                // so we can add it back on instance reset
                if (quest.QuestType == QuestType.World)
                {
                    CompletedWorldQuests.Add(questId);
                }

                if (quest.NextQuestId != 0)
                {
                    AddNewQuest(quest.NextQuestId, 0);
                }
            }
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
            lock(ActiveQuests)
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
            var quest = QuestManager.GetQuest(questId);
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

        public void ResetInstanceQuestState()
        {
            // Add all world quests
            foreach (var questId in CompletedWorldQuests)
            {
                AddNewQuest(questId, 0);
            }

            CompletedWorldQuests.Clear();
        }

        public bool UpdatePartyQuestProgress(DdonGameServer server, PartyGroup party, QuestId questId)
        {
            Quest quest = QuestManager.GetQuest(questId);

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
            Quest quest = QuestManager.GetQuest(questId);

            var questState = party.QuestState.GetQuestState(quest);
            foreach (var memberClient in party.Clients)
            {
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
                    var nextQuest = QuestManager.GetQuest(quest.NextQuestId);
                    server.Database.InsertQuestProgress(memberClient.Character.CommonId, nextQuest.QuestId, nextQuest.QuestType, 0);
                }

                server.Database.InsertIfNotExistCompletedQuest(memberClient.Character.CommonId, quest.QuestId, quest.QuestType);
            }

            // Remove the quest data from the party object
            CompleteQuest(quest.QuestId);

            return true;
        }

        public bool DistributePartyQuestRewards(DdonGameServer server, PartyGroup party, QuestId questId)
        {
            Quest quest = QuestManager.GetQuest(questId);

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
                else if (expPoint.Type == ExpType.PlayPoints)
                {
                    server.PPManager.AddPlayPoint(client, expPoint.Reward, 2);
                }
            }
        }

        public void UpdatePriorityQuestList(DdonGameServer server, PartyGroup party)
        {
            var leaderClient = party.Leader.Client;

            S2CQuestSetPriorityQuestNtc prioNtc = new S2CQuestSetPriorityQuestNtc()
            {
                CharacterId = leaderClient.Character.CharacterId
            };

            var priorityQuests = server.Database.GetPriorityQuests(leaderClient.Character.CommonId);
            foreach (var priorityQuestId in priorityQuests)
            {
                var priorityQuest = QuestManager.GetQuest(priorityQuestId);
                var questState = party.QuestState.GetQuestState(priorityQuest.QuestId);
                prioNtc.PriorityQuestList.Add(priorityQuest.ToCDataPriorityQuest(questState.Step));
            }
            party.SendToAll(prioNtc);
        }
    }
}
