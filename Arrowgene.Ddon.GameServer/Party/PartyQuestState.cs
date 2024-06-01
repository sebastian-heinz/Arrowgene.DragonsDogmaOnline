using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Dictionary<ushort, QuestProcessState> ProcessState {  get; set; }
        public Dictionary<StageId, List<InstancedEnemy>> QuestEnemies {  get; set; }
        public Dictionary<StageId, bool> ResetEnemyGroup { get; set; }

        public Dictionary<uint, QuestDeliveryRecord> DeliveryRecords {  get; set; }

        public QuestState()
        {
            ProcessState = new Dictionary<ushort, QuestProcessState>();
            QuestEnemies = new Dictionary<StageId, List<InstancedEnemy>>();
            DeliveryRecords = new Dictionary<uint, QuestDeliveryRecord>();
            ResetEnemyGroup = new Dictionary<StageId, bool>();
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
        private Dictionary<QuestId, QuestState> ActiveQuests { get; set; }
        private Dictionary<StageId, List<QuestId>> QuestLookupTable { get; set; }
        private List<QuestId> CompletedWorldQuests {  get; set; }

        public PartyQuestState()
        {
            ActiveQuests = new Dictionary<QuestId, QuestState>();
            QuestLookupTable = new Dictionary<StageId, List<QuestId>>();
            CompletedWorldQuests = new List<QuestId>();

            // Populate world into the completed category so when the instance gets reloaded they are set
            CompletedWorldQuests.AddRange(QuestManager.GetQuestsByType(QuestType.World).Select(x => x.Key));
        }

        public void AddNewQuest(Quest quest)
        {
            lock (ActiveQuests) 
            {
                ActiveQuests[quest.QuestId] = new QuestState()
                {
                    QuestId = quest.QuestId,
                    QuestType = quest.QuestType
                };

                foreach (var location in quest.Locations)
                {
                    if (!QuestLookupTable.ContainsKey(location.StageId))
                    {
                        QuestLookupTable[location.StageId] = new List<QuestId>();
                    }

                    QuestLookupTable[location.StageId].Add(quest.QuestId);

                    // Populate Instance Enemy Data
                    ActiveQuests[quest.QuestId].QuestEnemies[location.StageId] = quest.GetEnemiesInStageGroup(location.StageId, location.SubGroupId);
                }

                foreach (var request in quest.DeliveryItems)
                {
                    ActiveQuests[quest.QuestId].AddDeliveryRequest(request.ItemId, request.Amount);
                }

                // Initialize Process State Table
                UpdateProcessState(quest.QuestId, quest.ToCDataQuestList().QuestProcessStateList);
            }
        }

        public List<InstancedEnemy> GetInstancedEnemies(QuestId questId, StageId stageId, ushort subGroupId)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[questId];
                return questState.QuestEnemies[stageId];
            }
        }

        public InstancedEnemy GetInstancedEnemy(QuestId questId, StageId stageId, ushort subGroupId, uint index)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[questId];
                foreach (var enemy in questState.QuestEnemies[stageId])
                {
                    if (enemy.Index == index)
                    {
                        return enemy;
                    }
                }

                return null;
            }
        }

        public bool ShouldResetInstanceEnemyGroup(QuestId questId, StageId stageId)
        {
            lock (ActiveQuests)
            {
                var questState = ActiveQuests[questId];
                bool ShouldReset = !questState.ResetEnemyGroup.ContainsKey(stageId);
                questState.ResetEnemyGroup[stageId] = false;
                return ShouldReset;
            }
        }

        public void AddNewQuest(QuestId questId)
        {
            var quest = QuestManager.GetQuest(questId);
            AddNewQuest(quest);
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
                    AddNewQuest(quest.NextQuestId);
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
                AddNewQuest(questId);
            }

            CompletedWorldQuests.Clear();
        }
    }
}
