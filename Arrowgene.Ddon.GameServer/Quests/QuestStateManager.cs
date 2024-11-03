using Arrowgene.Ddon.Database.Model;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Quests
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
        public uint QuestScheduleId {  get; set; }
        public QuestType QuestType { get; set; }
        public QuestProgressState State { get; set; }
        public uint Step { get; set; }

        public Dictionary<ushort, QuestProcessState> ProcessState { get; set; }
        public Dictionary<StageId, Dictionary<uint, List<InstancedEnemy>>> QuestEnemies { get; set; }
        public Dictionary<uint, QuestDeliveryRecord> DeliveryRecords { get; set; }
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

        public void AddHuntRequest(QuestEnemyHunt hunt, uint countOverride = 0)
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
                    AmountHunted = countOverride
                };
            }
        }

        public QuestEnemyHuntRecord UpdateHuntRequest(Enemy enemy)
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

    public abstract class QuestStateManager
    {

        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestStateManager));

        protected Dictionary<uint, QuestState> ActiveQuests { get; set; }
        private Dictionary<StageId, HashSet<uint>> QuestLookupTable { get; set; }
        private List<QuestId> CompletedWorldQuests { get; set; }
        private Dictionary<QuestAreaId, HashSet<uint>> RolledInstanceWorldQuests { get; set; }

        public QuestStateManager()
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
                foreach (var request in quest.EnemyHunts)
                {
                    uint storedHunts = (quest.SaveWorkAsStep && step >= 1) ? (step - 1) : 0;
                    ActiveQuests[quest.QuestScheduleId].AddHuntRequest(request, storedHunts);
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

        public abstract bool UpdateQuestProgress(uint questScheduleId, DbConnection? connectionIn = null);
        public abstract bool CompleteQuestProgress(uint questScheduleId, DbConnection? connectionIn = null);
        public abstract bool DistributeQuestRewards(uint questScheduleId, DbConnection? connectionIn = null);
        public abstract void UpdatePriorityQuestList(GameClient requestingClient, DbConnection? connectionIn = null);

        protected void SendWalletRewards(DdonGameServer server, GameClient client, Quest quest, DbConnection? connectionIn = null)
        {
            S2CItemUpdateCharacterItemNtc updateCharacterItemNtc = new S2CItemUpdateCharacterItemNtc()
            {
                UpdateType = ItemNoticeType.Quest
            };

            foreach (var walletReward in quest.WalletRewards)
            {
                updateCharacterItemNtc.UpdateWalletList.Add(server.WalletManager.AddToWallet(
                    client.Character, 
                    walletReward.Type, 
                    walletReward.Value,
                    connectionIn: connectionIn
                ));
            }

            if (updateCharacterItemNtc.UpdateWalletList.Count > 0)
            {
                client.Send(updateCharacterItemNtc);
            }

            foreach (var expPoint in quest.ExpRewards)
            {
                if (expPoint.Type == ExpType.ExperiencePoints)
                {
                    server.ExpManager.AddExp(client, client.Character, expPoint.Reward, RewardSource.Quest, quest.QuestType, connectionIn);
                }
                else if (expPoint.Type == ExpType.JobPoints)
                {
                    server.ExpManager.AddJp(client, client.Character, expPoint.Reward, RewardSource.Quest, quest.QuestType, connectionIn);
                }
                else if (expPoint.Type == ExpType.PlayPoints)
                {
                    server.PPManager.AddPlayPoint(client, expPoint.Reward, type: 1, connectionIn: connectionIn);
                }
            }
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

    public class SharedQuestStateManager : QuestStateManager
    {
        private readonly PartyGroup Party;
        private readonly DdonGameServer Server;

        public SharedQuestStateManager(PartyGroup party, DdonGameServer server)
        {
            this.Party = party;
            this.Server = server;
        }

        public override bool CompleteQuestProgress(uint questScheduleId, DbConnection? connectionIn = null)
        {
            Quest quest = GetQuest(questScheduleId);

            var questState = GetQuestState(quest);
            foreach (var memberClient in Party.Clients)
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

                    Server.Database.ReplaceCompletedQuest(memberClient.Character.CommonId, quest.QuestId, quest.QuestType, completedQuests[quest.QuestId].ClearCount, connectionIn);
                    continue;
                }

                var result = Server.Database.GetQuestProgressByScheduleId(memberClient.Character.CommonId, questScheduleId, connectionIn);
                if (result == null)
                {
                    continue;
                }

                if (result.Step != questState.Step && !quest.SaveWorkAsStep)
                {
                    continue;
                }

                Server.Database.DeletePriorityQuest(memberClient.Character.CommonId, questScheduleId, connectionIn);
                Server.Database.RemoveQuestProgress(memberClient.Character.CommonId, questScheduleId, quest.QuestType, connectionIn);
                if (quest.NextQuestId != QuestId.None)
                {
                    var nextQuest = GetQuest((uint)quest.NextQuestId);
                    Server.Database.InsertQuestProgress(memberClient.Character.CommonId, nextQuest.QuestScheduleId, nextQuest.QuestType, 0, connectionIn);
                }

                if (!memberClient.Character.CompletedQuests.ContainsKey(quest.QuestId))
                {
                    memberClient.Character.CompletedQuests.Add(quest.QuestId, new CompletedQuest()
                    {
                        QuestId = quest.QuestId,
                        QuestType = quest.QuestType,
                        ClearCount = 1,
                    });
                    Server.Database.InsertIfNotExistCompletedQuest(memberClient.Character.CommonId, quest.QuestId, quest.QuestType, connectionIn);
                }
                else
                {
                    uint clearCount = ++memberClient.Character.CompletedQuests[quest.QuestId].ClearCount;
                    Server.Database.ReplaceCompletedQuest(memberClient.Character.CommonId, quest.QuestId, quest.QuestType, clearCount, connectionIn);
                }
            }

            // Remove the quest data from the party object
            CompleteQuest(quest.QuestScheduleId);

            return true;
        }

        public override bool DistributeQuestRewards(uint questScheduleId, DbConnection? connectionIn = null)
        {
            Quest quest = GetQuest(questScheduleId);

            var questState = GetQuestState(quest);
            foreach (var memberClient in Party.Clients)
            {
                // If this is a main quest, check to see that the member is currently on this quest, otherwise don't reward
                if (quest.QuestType == QuestType.Main)
                {
                    var result = Server.Database.GetQuestProgressByScheduleId(memberClient.Character.CommonId, quest.QuestScheduleId, connectionIn);
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
                    Server.RewardManager.AddQuestRewards(memberClient, quest, connectionIn);
                }

                // Check for Exp, Rift and Gold Rewards
                SendWalletRewards(Server, memberClient, quest, connectionIn);
            }

            return true;
        }

        public override void UpdatePriorityQuestList(GameClient requestingClient, DbConnection? connectionIn = null)
        {
            if (Party.Leader is null || requestingClient != Party.Leader.Client)
            {
                return;
            }

            var leaderClient = Party.Leader.Client;

            S2CQuestSetPriorityQuestNtc prioNtc = new S2CQuestSetPriorityQuestNtc()
            {
                CharacterId = leaderClient.Character.CharacterId
            };

            var priorityQuestScheduleIds = Server.Database.GetPriorityQuestScheduleIds(leaderClient.Character.CommonId, connectionIn);
            foreach (var priorityQuestScheduleId in priorityQuestScheduleIds)
            {
                var quest = QuestManager.GetQuestByScheduleId(priorityQuestScheduleId);
                var questStateManager = quest.IsPersonal ? leaderClient.QuestState : leaderClient.Party.QuestState;
                var questState = questStateManager.GetQuestState(priorityQuestScheduleId);
                prioNtc.PriorityQuestList.Add(quest.ToCDataPriorityQuest(questState.Step));
            }
            Party.SendToAll(prioNtc);
        }

        public override bool UpdateQuestProgress( uint questScheduleId, DbConnection? connectionIn = null)
        {
            var questState = GetQuestState(questScheduleId);
            var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
            foreach (var memberClient in Party.Clients)
            {
                var result = Server.Database.GetQuestProgressByScheduleId(memberClient.Character.CommonId, questState.QuestScheduleId, connectionIn);
                if (result == null)
                {
                    continue;
                }

                if (result.Step != questState.Step && !quest.SaveWorkAsStep)
                {
                    continue;
                }

                Server.Database.UpdateQuestProgress(memberClient.Character.CommonId, questState.QuestScheduleId, questState.QuestType, questState.Step + 1, connectionIn);
            }

            questState.Step += 1;

            return true;
        }
    }

    public class SoloQuestStateManager : QuestStateManager
    {
        private readonly PlayerPartyMember Member;
        private readonly DdonGameServer Server;

        public SoloQuestStateManager(PlayerPartyMember member, DdonGameServer server)
        {
            this.Member = member;
            this.Server = server;
        }

        public override bool UpdateQuestProgress(uint questScheduleId, DbConnection? connectionIn = null)
        {
            var questState = GetQuestState(questScheduleId);
            var quest = QuestManager.GetQuestByScheduleId(questScheduleId);

            var result = Server.Database.GetQuestProgressByScheduleId(Member.Client.Character.CommonId, questState.QuestScheduleId);
            if (result == null)
            {
                return false;
            }

            if (result.Step != questState.Step && !quest.SaveWorkAsStep)
            {
                return false;
            }

            Server.Database.UpdateQuestProgress(Member.Client.Character.CommonId, questState.QuestScheduleId, questState.QuestType, questState.Step + 1);

            questState.Step += 1;

            return true;
        }

        public override bool CompleteQuestProgress(uint questScheduleId, DbConnection? connectionIn = null)
        {
            Quest quest = GetQuest(questScheduleId);
            var questState = GetQuestState(quest);

            var result = Server.Database.GetQuestProgressByScheduleId(Member.Client.Character.CommonId, questScheduleId, connectionIn);
            if (result == null)
            {
                return false;
            }

            if (result.Step != questState.Step && !quest.SaveWorkAsStep)
            {
                return false;
            }

            Server.Database.DeletePriorityQuest(Member.Client.Character.CommonId, questScheduleId, connectionIn);
            Server.Database.RemoveQuestProgress(Member.Client.Character.CommonId, questScheduleId, quest.QuestType, connectionIn);
            if (quest.NextQuestId != QuestId.None)
            {
                var nextQuest = GetQuest((uint)quest.NextQuestId);
                Server.Database.InsertQuestProgress(Member.Client.Character.CommonId, nextQuest.QuestScheduleId, nextQuest.QuestType, 0, connectionIn);
            }

            if (!Member.Client.Character.CompletedQuests.ContainsKey(quest.QuestId))
            {
                Member.Client.Character.CompletedQuests.Add(quest.QuestId, new CompletedQuest()
                {
                    QuestId = quest.QuestId,
                    QuestType = quest.QuestType,
                    ClearCount = 1,
                });
                Server.Database.InsertIfNotExistCompletedQuest(Member.Client.Character.CommonId, quest.QuestId, quest.QuestType, connectionIn);
            }
            else
            {
                uint clearCount = ++Member.Client.Character.CompletedQuests[quest.QuestId].ClearCount;
                Server.Database.ReplaceCompletedQuest(Member.Client.Character.CommonId, quest.QuestId, quest.QuestType, clearCount, connectionIn);
            }

            CompleteQuest(quest.QuestScheduleId);

            return true;
        }

        public override bool DistributeQuestRewards(uint questScheduleId, DbConnection? connectionIn = null)
        {
            Quest quest = GetQuest(questScheduleId);
            var questState = GetQuestState(quest);
            if (quest.HasRewards())
            {
                Server.RewardManager.AddQuestRewards(Member.Client, quest);
            }

            // Check for Exp, Rift and Gold Rewards
            SendWalletRewards(Server, Member.Client, quest);
            return true;
        }

        public override void UpdatePriorityQuestList(GameClient requestingClient, DbConnection? connectionIn = null)
        {
            throw new NotImplementedException();
        }

        public void HandleEnemyHuntRequests(Enemy enemy)
        {
            if (Member.Client.Character.GameMode != GameMode.Normal)
            {
                return;
            }

            lock (ActiveQuests)
            {
                foreach (var quest in ActiveQuests)
                {
                    Quest questObject = QuestManager.GetQuestByScheduleId(quest.Key);
                    QuestState questState = quest.Value;
                    QuestEnemyHuntRecord huntRecord = questState.UpdateHuntRequest(enemy);

                    if (huntRecord != null)
                    {
                        if (questObject.SaveWorkAsStep)
                        {
                            // The quest machinery of the client expects the entire hunt for light quests to be one stage, but that means progress doesn't persist.
                            // Without altering the DB structure, we save the progress in the "stage" field:
                            // Stage 0 -> Not accepted, should never occur.
                            // Stage 1 -> Accepted, but no work done
                            // Stage N+1 -> Accepted, but with N work done. 
                            Server.Database.UpdateQuestProgress(Member.Client.Character.CommonId, questState.QuestScheduleId, questState.QuestType, huntRecord.AmountHunted + 1);
                        }

                        S2CQuestQuestProgressWorkSaveNtc ntc = new()
                        {
                            QuestScheduleId = quest.Key,
                            ProcessNo = huntRecord.ProcessNo,
                            SequenceNo = huntRecord.SequenceNo,
                            BlockNo = huntRecord.BlockNo
                        };
                        ntc.WorkList.Add(new CDataQuestProgressWork()
                        {
                            CommandNo = (uint)QuestNotifyCommand.KilledEnemyLight,
                            Work01 = 0,
                            Work02 = 0,
                            Work03 = (int)huntRecord.AmountHunted
                        });

                        Member.Client.Send(ntc);
                    }
                }
            }
        }
    }
}
