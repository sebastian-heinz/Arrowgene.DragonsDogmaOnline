using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Asset;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class EpitaphPartyRewards
    {
        public EpitaphPartyRewards()
        {
            ItemRewards = new List<CDataGatheringItemElement>();
            BuffRewards = new List<CDataSeasonDungeonBuffEffectReward>();
        }

        public uint TrialId { get; set; }
        public List<CDataGatheringItemElement> ItemRewards { get; set; }
        public List<CDataSeasonDungeonBuffEffectReward> BuffRewards { get; set; }
    }

    public class EpitaphMysteriousDoorState
    {
        public EpitaphMysteriousDoorState()
        {
            GatheringPoint = new EpitaphGatheringPoint();
        }

        public SoulOrdealOmState State { get; set; }
        public EpitaphGatheringPoint GatheringPoint { get; set; }
    }

    public class EpitaphPartyState
    {
        public EpitaphPartyState()
        {
            Objectives = new Dictionary<SoulOrdealObjective, EpitaphObjective>();
            Trial = new EpitaphTrialOption();
            AbnormalStatus = new Dictionary<(StageLayoutId StageId, uint PosId), bool>();
        }

        public uint PartyId { get; set; }
        public uint TimerId { get; set; }
        public SoulOrdealObjective PrimaryObjective;
        public Dictionary<SoulOrdealObjective, EpitaphObjective> Objectives { get; set; }
        public EpitaphTrialOption Trial { get; set; }
        public Dictionary<(StageLayoutId StageId, uint PosId), bool> AbnormalStatus { get; set; }

        public List<CDataSoulOrdealObjective> GetObjectiveList()
        {
            var results = new List<CDataSoulOrdealObjective>();
            foreach (var objective in Objectives.Values)
            {
                results.Add(objective.AsCDataSoulOrdealObjective());
            }
            return results;
        }
    }

    public class EpitaphRoadManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(EpitaphRoadManager));

        private DdonGameServer _Server;
        private EpitaphTrialAsset _TrialAssets;
        private EpitaphRoadAsset _EpitaphAssets;

        private Dictionary<uint, EpitaphPartyState> _TrialsInProgress;
        private Dictionary<uint, Dictionary<(StageLayoutId StageId, uint PosId), EpitaphPartyRewards>> _TrialHasRewards;
        private Dictionary<uint, Dictionary<uint, Dictionary<uint, EpitaphBuff>>> _PartyBuffs;
        private Dictionary<uint, HashSet<(StageLayoutId StageId, uint PosId)>> _CompletedTrials;
        private Dictionary<uint, Dictionary<(StageLayoutId stageId, uint PosId), EpitaphMysteriousDoorState>> _DoorState;

        public EpitaphRoadManager(DdonGameServer server)
        {
            _Server = server;
            _TrialAssets = server.AssetRepository.EpitaphTrialAssets;
            _EpitaphAssets = server.AssetRepository.EpitaphRoadAssets;
            _TrialsInProgress = new Dictionary<uint, EpitaphPartyState>();
            _TrialHasRewards = new Dictionary<uint, Dictionary<(StageLayoutId StageId, uint PosId), EpitaphPartyRewards>>();
            _PartyBuffs = new Dictionary<uint, Dictionary<uint, Dictionary<uint, EpitaphBuff>>>();
            _CompletedTrials = new Dictionary<uint, HashSet<(StageLayoutId StageId, uint PosId)>>();
            _DoorState = new Dictionary<uint, Dictionary<(StageLayoutId stageId, uint PosId), EpitaphMysteriousDoorState>>();
        }

        public void ResetInstance(PartyGroup party)
        {
            lock (_TrialsInProgress)
            {
                _TrialsInProgress.Remove(party.Id);
                _CompletedTrials.Remove(party.Id);
                _PartyBuffs.Remove(party.Id);
                _DoorState.Remove(party.Id);

                // TODO: How to manage this (should it reset or stay?)
                // _TrialHasRewards.Remove(party.Id);
            }
        }

        public void StartTrial(PartyGroup party, uint trialId)
        {
            lock (_TrialsInProgress)
            {
                if (_TrialsInProgress.ContainsKey(party.Id))
                {
                    Logger.Error($"Party={party.Id} is trying to start a new trial while one is already in progress");
                    return;
                }

                if (!_CompletedTrials.ContainsKey(party.Id))
                {
                    _CompletedTrials[party.Id] = new HashSet<(StageLayoutId, uint)>();
                }

                var trial = GetTrialOptionInfo(trialId);
                
                var partyState = new EpitaphPartyState()
                {
                    Trial = trial,
                    PrimaryObjective = trial.Objectives.Values.Where(x => x.Priority == SoulOrdealObjectivePriority.Primary).Select(x => x.Type).First(),
                    Objectives = trial.CreateNewObjectiveState()
                };
                _TrialsInProgress[party.Id] = partyState;

                var ntc = new S2CSeasonDungeonExecuteSoulOrdealNtc()
                {
                    TrialId = trial.EpitaphId,
                    TrialName = trial.TrialName,
                    ObjectiveList = partyState.GetObjectiveList(),
                };
                party.SendToAll(ntc);

                foreach (var enemyGroup in trial.EnemyGroups.Values)
                {
                    party.InstanceEnemyManager.ResetEnemyNode(enemyGroup.StageLayoutId);
                    party.SendToAll(new S2CInstanceEnemyGroupResetNtc() { LayoutId = enemyGroup.StageLayoutId.ToCDataStageLayoutId() });
                }

                if (partyState.Objectives.ContainsKey(SoulOrdealObjective.CompleteConditionsWithinTimeLimit))
                {
                    partyState.TimerId = _Server.TimerManager.CreateTimer(partyState.Objectives[SoulOrdealObjective.CompleteConditionsWithinTimeLimit].Param1, () =>
                    {
                        Logger.Info($"(SoulOrdeal) Timer expired for Id={party.Id}");
                        EndTrial(party, partyState, SoulOrdealEndState.Failed);
                    });
                    _Server.TimerManager.StartTimer(partyState.TimerId);
                    Logger.Info($"Starting {partyState.Objectives[SoulOrdealObjective.CompleteConditionsWithinTimeLimit].Param1} second timer for PartyId={party.Id}");
                }
            }
        }

        public void InterruptTrialInProgress(PartyGroup party)
        {
            lock (_TrialsInProgress)
            {
                var partyState = GetPartyState(party);
                if (partyState == null)
                {
                    return;
                }

                EndTrial(party, partyState, SoulOrdealEndState.Failed);
            }
        }

        public uint GetTrialIdInProgress(PartyGroup party)
        {
            lock (_TrialsInProgress)
            {
                if (!_TrialsInProgress.ContainsKey(party.Id))
                {
                    return 0;
                }
                return _TrialsInProgress[party.Id].Trial.EpitaphId;
            }
        }

        public bool TrialInProgress(PartyGroup party)
        {
            lock (_TrialsInProgress)
            {
                return _TrialsInProgress.ContainsKey(party.Id);
            }
        }

        public bool TrialHasRewards(GameClient client, StageLayoutId stageId, uint posId)
        {
            lock (_TrialsInProgress)
            {
                if (!_TrialHasRewards.ContainsKey(client.Character.CharacterId))
                {
                    return false;
                }

                return _TrialHasRewards[client.Character.CharacterId].ContainsKey((stageId, posId));
            }
        }

        public EpitaphPartyRewards GetRewards(GameClient client, StageLayoutId stageId, uint posId)
        {
            lock (_TrialsInProgress)
            {
                if (!_TrialHasRewards.ContainsKey(client.Character.CharacterId))
                {
                    return null;
                }

                if (!_TrialHasRewards[client.Character.CharacterId].ContainsKey((stageId, posId)))
                {
                    return null;
                }

                return _TrialHasRewards[client.Character.CharacterId][(stageId, posId)];
            }
        }

        public void CollectTrialRewards(GameClient client, StageLayoutId stageId, uint posId)
        {
            lock (_TrialsInProgress)
            {
                if (!_TrialHasRewards.ContainsKey(client.Character.CharacterId))
                {
                    return;
                }
                _TrialHasRewards[client.Character.CharacterId].Remove((stageId, posId));
            }
        }

        public bool TrialHasEnemies(PartyGroup party, StageLayoutId stageId, byte subGroupId)
        {
            lock (_TrialsInProgress)
            {
                if (!_TrialsInProgress.ContainsKey(party.Id))
                {
                    return false;
                }

                var trial = _TrialsInProgress[party.Id].Trial;
                if (trial == null)
                {
                    return false;
                }

                return trial.EnemyGroupsByStageId.ContainsKey((stageId, subGroupId));
            }
        }

        public List<InstancedEnemy> GetInstancedEnemies(PartyGroup party, StageLayoutId stageId, byte subGroupId)
        {
            lock (_TrialsInProgress)
            {
                if (!_TrialsInProgress.ContainsKey(party.Id))
                {
                    return new List<InstancedEnemy>();
                }

                var trial = _TrialsInProgress[party.Id].Trial;
                if (trial == null)
                {
                    return new List<InstancedEnemy>();
                }

                return trial.EnemyGroupsByStageId[(stageId, subGroupId)].CreateNewInstance();
            }
        }

        public void EvaluatePlayerAbnormalStatus(PartyGroup party, uint statusId)
        {
            lock (_TrialsInProgress)
            {
                var partyState = GetPartyState(party);
                if (partyState.Objectives.ContainsKey(SoulOrdealObjective.CannotBeAffectedByAbnormalStatus))
                {
                    var objective = partyState.Objectives[SoulOrdealObjective.CannotBeAffectedByAbnormalStatus];

                    objective.Param2 += 1;
                    if (objective.Param2 > objective.Param1)
                    {
                        objective.Param2 = objective.Param1;
                    }

                    party.SendToAll(new S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc()
                    {
                        Objectives = partyState.GetObjectiveList()
                    });

                    if (objective.Param1 == objective.Param2)
                    {
                        EndTrial(party, partyState, SoulOrdealEndState.Failed);
                    }
                }
            }
        }

        public void EvaluateItemUsed(PartyGroup party, uint itemId)
        {
            lock (_TrialsInProgress)
            {
                var partyState = GetPartyState(party);
                if (partyState.Objectives.ContainsKey(SoulOrdealObjective.ItemNoteUsedMoreThanOnce))
                {
                    partyState.Objectives[SoulOrdealObjective.ItemNoteUsedMoreThanOnce].Param2 = 0;
                    party.SendToAll(new S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc()
                    {
                        Objectives = partyState.GetObjectiveList()
                    });

                    EndTrial(party, partyState, SoulOrdealEndState.Failed);
                }
            }
        }

        public void EvaluateDeath(PartyGroup party)
        {
            lock (_TrialsInProgress)
            {
                var partyState = GetPartyState(party);
                if (partyState.Objectives.ContainsKey(SoulOrdealObjective.CannotDieMoreThanOnce))
                {
                    partyState.Objectives[SoulOrdealObjective.ItemNoteUsedMoreThanOnce].Param2 = 0;
                    party.SendToAll(new S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc()
                    {
                        Objectives = partyState.GetObjectiveList()
                    });

                    EndTrial(party, partyState, SoulOrdealEndState.Failed);
                }
            }
        }

        private EpitaphBuff GetBuff(List<EpitaphBuff> buffs, EpitaphBuffId buffId)
        {
            return buffs.Where(x => (EpitaphBuffId)x.BuffId == buffId).FirstOrDefault();
        }

        public uint CalculateBloodOrbBonus(PartyGroup party, InstancedEnemy enemy)
        {
            var buff = GetBuff(GetPartyBuffs(party), EpitaphBuffId.EnemyBoDropIncrease);
            if (buff != null)
            {
                if (0.5 >= Random.Shared.NextDouble())
                {
                    return buff.Increment * 25;
                }
            }
            return 0;
        }

        public void EvaluateEnemyAbnormalStatusEffectStart(PartyGroup party, StageLayoutId stageId, uint posId)
        {
            lock (_TrialsInProgress)
            {
                if (!TrialHasEnemies(party, stageId, 0))
                {
                    return;
                }

                var partyState = GetPartyState(party);
                if (partyState == null)
                {
                    return;
                }
                partyState.AbnormalStatus[(stageId, posId)] = true;

                if (partyState.Objectives.ContainsKey(SoulOrdealObjective.InflictAbnormalStatusCount))
                {
                    var objective = partyState.Objectives[SoulOrdealObjective.InflictAbnormalStatusCount];

                    objective.Param2 += 1;
                    if (objective.Param2 > objective.Param1)
                    {
                        objective.Param2 = objective.Param1;
                    }

                    party.SendToAll(new S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc()
                    {
                        Objectives = partyState.GetObjectiveList()
                    });
                }
            }
        }

        public void EvaluateEnemyAbnormalStatusEffectEnd(PartyGroup party, StageLayoutId stageId, uint posId)
        {
            lock (_TrialsInProgress)
            {
                if (!TrialHasEnemies(party, stageId, 0))
                {
                    return;
                }

                var partyState = GetPartyState(party);
                if (partyState == null)
                {
                    return;
                }
                partyState.AbnormalStatus[(stageId, posId)] = false;
            }
        }

        private void UpdateObjective(EpitaphPartyState partyState, SoulOrdealObjective objectiveId, uint incrementAmount)
        {
            switch (objectiveId)
            {
                case SoulOrdealObjective.DefeatEnemyCount:
                case SoulOrdealObjective.CannotBeAffectedByAbnormalStatus:
                case SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount:
                    partyState.Objectives[objectiveId].Param2 += incrementAmount;
                    if (partyState.Objectives[objectiveId].Param2 > partyState.Objectives[objectiveId].Param1)
                    {
                        partyState.Objectives[objectiveId].Param2 = partyState.Objectives[objectiveId].Param1;
                    }
                    break;
                default:
                    Logger.Error($"Attempting to update '{objectiveId}' but there is no handler.");
                    break;
            }
        }

        private bool HasObjective(EpitaphPartyState partyState, SoulOrdealObjective objectiveId)
        {
            return partyState.Objectives.ContainsKey(objectiveId);
        }

        private bool IsObjectiveMet(EpitaphPartyState partyState, SoulOrdealObjective objectiveId)
        {
            if (!partyState.Objectives.ContainsKey(objectiveId))
            {
                return false;
            }
            return partyState.Objectives[objectiveId].IsObjectiveMet();
        }

        public void EvaluateEnemyKilled(PartyGroup party, StageLayoutId stageId, uint posId, InstancedEnemy enemy)
        {
            lock (_TrialsInProgress)
            {
                if (!TrialHasEnemies(party, stageId, 0))
                {
                    return;
                }

                var partyState = GetPartyState(party);
                if (partyState == null)
                {
                    // Just killed a random enemy
                    // nothing else to do
                    return;
                }

                var trialEndState = SoulOrdealEndState.Cancel;
                if (HasObjective(partyState, SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount) &&
                    partyState.AbnormalStatus.ContainsKey((stageId, posId)))
                {
                    UpdateObjective(partyState, SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount, 1);
                    partyState.AbnormalStatus[(stageId, posId)] = false;
                }

                bool trialCompleted = false;
                if (partyState.PrimaryObjective == SoulOrdealObjective.EliminateTheEnemy)
                {
                    var enemyGroup = party.InstanceEnemyManager.GetInstancedEnemies(stageId);
                    var groupDestroyed = enemyGroup.Where(x => x.IsRequired).All(x => x.IsKilled);
                    if (groupDestroyed)
                    {
                        trialCompleted = true;
                        if (HasObjective(partyState, SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount))
                        {
                            trialEndState = IsObjectiveMet(partyState, SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount) ? SoulOrdealEndState.Success : SoulOrdealEndState.Failed;
                        }
                        else if (HasObjective(partyState, SoulOrdealObjective.InflictAbnormalStatusCount))
                        {
                            trialEndState = IsObjectiveMet(partyState, SoulOrdealObjective.InflictAbnormalStatusCount) ? SoulOrdealEndState.Success : SoulOrdealEndState.Failed;
                        }
                        else
                        {
                            trialEndState = SoulOrdealEndState.Success;
                        }
                    }
                }
                else if (partyState.PrimaryObjective == SoulOrdealObjective.DefeatEnemyCount)
                {
                    partyState.Objectives[partyState.PrimaryObjective].Param2 += 1;
                    if (partyState.Objectives[partyState.PrimaryObjective].Param2 >= partyState.Objectives[partyState.PrimaryObjective].Param1)
                    {
                        if (HasObjective(partyState, SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount))
                        {
                            trialCompleted = IsObjectiveMet(partyState, SoulOrdealObjective.DefeatEnemyWithAbnormalStatusCount);
                        }
                        else if (HasObjective(partyState, SoulOrdealObjective.InflictAbnormalStatusCount))
                        {
                            trialCompleted = IsObjectiveMet(partyState, SoulOrdealObjective.InflictAbnormalStatusCount);
                        }
                        else
                        {
                            trialCompleted = true;
                        }
                    }

                    if (trialCompleted)
                    {
                        trialEndState = SoulOrdealEndState.Success;
                    }
                }

                party.SendToAll(new S2CSeasonDungeonUpdateSoulOrdealObjectivesNtc()
                {
                    Objectives = partyState.GetObjectiveList()
                });

                if (trialCompleted)
                {
                    EndTrial(party, partyState, trialEndState);
                }
            }
        }

        private void EndTrial(PartyGroup party, EpitaphPartyState partyState, SoulOrdealEndState endState)
        {
            if (partyState.TimerId > 0)
            {
                _Server.TimerManager.CancelTimer(partyState.TimerId);
            }

            // End Trial
            party.SendToAll(new S2CSeasonDungeonEndSoulOrdealNtc() { EndState = endState,
                    LayoutId = partyState.Trial.StageId.ToCDataStageLayoutId(), PosId = partyState.Trial.PosId,
                    EpitaphState = ((endState == SoulOrdealEndState.Success) ? SoulOrdealOmState.TrialComplete : SoulOrdealOmState.TrialAvailable)});

            if (endState == SoulOrdealEndState.Success)
            {
                AddRewards(party, partyState);

                _CompletedTrials[party.Id].Add((partyState.Trial.StageId, partyState.Trial.PosId));

                foreach (var unlock in partyState.Trial.Parent.Unlocks)
                {
                    party.SendToAll(new S2CSeasonDungeonSetOmStateNtc()
                    {
                        LayoutId = unlock.StageId.ToCDataStageLayoutId(),
                        PosId = unlock.PosId,
                        State = SoulOrdealOmState.AreaUnlocked
                    });
                }
            }

            if (_TrialsInProgress.ContainsKey(party.Id))
            {
                _TrialsInProgress.Remove(party.Id);
            }

            foreach (var enemyGroup in partyState.Trial.EnemyGroups.Values)
            {
                party.InstanceEnemyManager.ResetEnemyNode(enemyGroup.StageLayoutId);
                party.SendToAll(new S2CInstanceEnemyGroupResetNtc() { LayoutId = enemyGroup.StageLayoutId.ToCDataStageLayoutId() });
            }
        }

        private void AddRewards(PartyGroup party, EpitaphPartyState partyState)
        {
            lock (_TrialsInProgress)
            {
                foreach (var client in party.Clients)
                {
                    if (!_TrialHasRewards.ContainsKey(client.Character.CharacterId))
                    {
                        _TrialHasRewards[client.Character.CharacterId] = new Dictionary<(StageLayoutId StageId, uint PosId), EpitaphPartyRewards>();
                    }

                    var rewards = new EpitaphPartyRewards()
                    {
                        TrialId = partyState.Trial.EpitaphId,
                    };

                    var dungeonInfo = GetDungeonInfoByStageId(party.Leader.Client.Character.Stage);
                    if (dungeonInfo.RewardBuffs)
                    {
                        // Older dungeons don't normally give buff rewards, although they can be configured to do so
                        foreach (var type in Enum.GetValues<SoulOrdealBuffType>())
                        {
                            var reward = _EpitaphAssets.BuffsByType[type][Random.Shared.Next(_EpitaphAssets.BuffsByType[type].Count)];
                            rewards.BuffRewards.Add(reward.AsCDataSeasonDungeonBuffEffectReward());
                        }
                    }

                    uint slotNo = 0;
                    foreach (var item in partyState.Trial.ItemRewards)
                    {
                        var rolledItems = item.AsCDataGatheringItemElement();
                        foreach (var rolledItem in rolledItems)
                        {
                            if (rolledItem.ItemNum > 0)
                            {
                                rolledItem.SlotNo = slotNo++;
                                rewards.ItemRewards.Add(rolledItem);
                            }
                        }
                    }
                    _TrialHasRewards[client.Character.CharacterId][(partyState.Trial.StageId, partyState.Trial.PosId)] = rewards;
                }
            }
        }

        private EpitaphPartyState GetPartyState(PartyGroup party)
        {
            lock (_TrialsInProgress)
            {
                if (!_TrialsInProgress.ContainsKey(party.Id))
                {
                    return null;
                }
                return _TrialsInProgress[party.Id];
            }
        }

        public T GetEpitahObject<T>(uint epitaphId)
        {
            if (_EpitaphAssets.EpitaphObjects.ContainsKey(epitaphId))
            {
                return (T) Convert.ChangeType(_EpitaphAssets.EpitaphObjects[epitaphId], typeof(T));
            }
            else if (_TrialAssets.EpitahObjects.ContainsKey(epitaphId))
            {
                return (T) Convert.ChangeType(_TrialAssets.EpitahObjects[epitaphId], typeof(T));
            }
            return default;
        }

        public EpitaphTrialOption GetTrialOptionInfo(uint epitaphId)
        {
            return GetEpitahObject<EpitaphTrialOption>(epitaphId);
        }

        public bool IsEpitaphId(uint epitaphId)
        {
            return _EpitaphAssets.EpitaphObjects.ContainsKey(epitaphId) || _TrialAssets.EpitahObjects.ContainsKey(epitaphId);
        }

        public EpitaphTrialOption GetTrialOption(PartyGroup party)
        {
            lock (_TrialsInProgress)
            {
                if (!_TrialsInProgress.ContainsKey(party.Id))
                {
                    return null;
                }

                return _TrialsInProgress[party.Id].Trial;
            }
        }

        public EpitaphTrial GetTrial(StageLayoutId stageId, uint posId)
        {
            if (_TrialAssets == null || !_TrialAssets.Trials.ContainsKey(stageId))
            {
                return null;
            }
            return _TrialAssets.Trials[stageId].Where(x => x.PosId == posId).FirstOrDefault();
        }

        public EpitaphTrial GetTrial(uint epitaphId)
        {
            return GetEpitahObject<EpitaphTrial>(epitaphId);
        }

        public EpitaphBarrier GetBarrier(uint epitaphId)
        {
            return GetEpitahObject<EpitaphBarrier>(epitaphId);
        }

        public EpitaphBarrier GetBarrier(NpcId npcId)
        {
            if (!_Server.AssetRepository.EpitaphRoadAssets.BarriersByNpcId.ContainsKey(npcId))
            {
                return null;
            }
            return _Server.AssetRepository.EpitaphRoadAssets.BarriersByNpcId[npcId];
        }

        public void AddPlayerBuff(GameClient client, PartyGroup party, uint buffId, uint increment)
        {
            lock (_PartyBuffs)
            {
                if (!_PartyBuffs.ContainsKey(party.Id))
                {
                    _PartyBuffs[party.Id] = new Dictionary<uint, Dictionary<uint, EpitaphBuff>>();
                }

                var partybuffs = _PartyBuffs[party.Id];
                if (!partybuffs.ContainsKey(client.Character.CharacterId))
                {
                    partybuffs[client.Character.CharacterId] = new Dictionary<uint, EpitaphBuff>();
                }

                var buffs = partybuffs[client.Character.CharacterId];
                if (!buffs.ContainsKey(buffId) && buffs.Count >= 6)
                {
                    return;
                }

                if (!buffs.ContainsKey(buffId))
                {
                    buffs[buffId] = new EpitaphBuff(_EpitaphAssets.BuffsById[buffId])
                    {
                        Increment = 0
                    };
                }

                buffs[buffId].Increment += increment;
                if (buffs[buffId].Increment > 4)
                {
                    buffs[buffId].Increment = 4;
                }
            }
        }

        public List<EpitaphBuff> GetPartyBuffs(PartyGroup party)
        {
            lock (_PartyBuffs)
            {
                if (!_PartyBuffs.ContainsKey(party.Id))
                {
                    return new List<EpitaphBuff>();
                }

                var results = new Dictionary<uint, EpitaphBuff>();
                foreach (var playerBuffs in _PartyBuffs[party.Id].Values)
                {
                    foreach (var buff in playerBuffs.Values)
                    {
                        if (!results.ContainsKey(buff.BuffId))
                        {
                            results[buff.BuffId] = new EpitaphBuff()
                            {
                                BuffId = buff.BuffId,
                                BuffEffect = buff.BuffEffect,
                                Name = buff.Name,
                                Type = buff.Type,
                                Increment = 0
                            };
                        }
                        results[buff.BuffId].Increment += buff.Increment;
                        if (results[buff.BuffId].Increment > 4)
                        {
                            results[buff.BuffId].Increment = 4;
                        }
                    }
                }
                return results.Values.ToList();
            }
        }

        public List<EpitaphBuff> GetPlayerBuffs(GameClient client, PartyGroup party)
        {
            lock (_PartyBuffs)
            {
                if (!_PartyBuffs.ContainsKey(party.Id))
                {
                    return new List<EpitaphBuff>();
                }

                if (!_PartyBuffs[party.Id].ContainsKey(client.Character.CharacterId))
                {
                    return new List<EpitaphBuff>();
                }

                return _PartyBuffs[party.Id][client.Character.CharacterId].Values.ToList();
            }
        }

        public SoulOrdealOmState GetEpitaphState(GameClient client, StageLayoutId stageId, uint posId)
        {
            lock (_TrialsInProgress)
            {
                if (!_TrialAssets.Trials.ContainsKey(stageId))
                {
                    return SoulOrdealOmState.Locked;
                }

                var trial = GetTrial(stageId, posId);
                if (trial == null)
                {
                    return SoulOrdealOmState.Locked;
                }
                
                if (!IsTrialUnlocked(client.Party, trial))
                {
                    return SoulOrdealOmState.Locked;
                }

                if (TrialHasRewards(client, stageId, posId))
                {
                    return SoulOrdealOmState.RewardAvailable;
                }

                if (!TrialCompleted(client.Party, stageId, posId))
                {
                    return StageManager.IsLegacyEpitaphRoadStageId(stageId) ? SoulOrdealOmState.LegacyTrialAvailable : SoulOrdealOmState.TrialAvailable;
                }

                return SoulOrdealOmState.RewardReceived;
            }
        }

        public bool TrialCompleted(PartyGroup party, StageLayoutId stageId, uint posId)
        {
            lock (_TrialsInProgress)
            {
                if (!_CompletedTrials.ContainsKey(party.Id))
                {
                    return false;
                }
                return _CompletedTrials[party.Id].Contains((stageId, posId));
            }
        }

        public void AreaChange(GameClient client, uint stageId, PacketQueue queue)
        {
            client.Send(new S2CSeasonDungeonAreaBuffEffectNtc());

            lock (_TrialsInProgress)
            {
                if (TrialInProgress(client.Party) && client.Party.Leader?.Client == client)
                {
                    EndTrial(client.Party, GetPartyState(client.Party), SoulOrdealEndState.Failed);
                }
            }

            //  Buff effects sent to other party members?
            client.Send(new S2C_SEASON_62_39_16_NTC()
            {
                CharacterId = client.Character.CharacterId
            });

            var dungeonInfo = _Server.EpitaphRoadManager.GetDungeonInfoByStageId(stageId);
            if (dungeonInfo != null)
            {
                foreach (var section in dungeonInfo.Sections)
                {
                    if (!client.Character.EpitaphRoadState.UnlockedContent.Contains(section.EpitaphId))
                    {
                        foreach (var omId in section.BarrierOmIds)
                        {
                            client.Send(new S2CSeasonDungeonSetOmStateNtc()
                            {
                                LayoutId = new CDataStageLayoutId()
                                {
                                    StageId = section.StageId,
                                    GroupId = omId
                                },
                                // TODO: This probably needs a PosId value
                                State = SoulOrdealOmState.Locked
                            });
                        }
                    }
                }

                foreach (var barrier in dungeonInfo.Barriers)
                {
                    if (!client.Character.EpitaphRoadState.UnlockedContent.Contains(barrier.EpitaphId))
                    {
                        client.Send(new S2CSeasonDungeonSetOmStateNtc()
                        {
                            LayoutId = barrier.StageId.ToCDataStageLayoutId(),
                            PosId = barrier.PosId,
                            State = SoulOrdealOmState.Locked
                        });
                    }
                }

                if (_CompletedTrials.ContainsKey(client.Party.Id))
                {
                    foreach (var completedTrial in _CompletedTrials[client.Party.Id])
                    {
                        if (completedTrial.StageId.Id != stageId)
                        {
                            continue;
                        }

                        var trial = GetTrial(completedTrial.StageId, completedTrial.PosId);
                        foreach (var unlock in trial.Unlocks)
                        {
                            client.Send(new S2CSeasonDungeonSetOmStateNtc()
                            {
                                LayoutId = unlock.StageId.ToCDataStageLayoutId(),
                                PosId = unlock.PosId,
                                State = SoulOrdealOmState.AreaUnlocked
                            });
                        }
                    }
                }

                _Server.EpitaphRoadManager.UpdateAllMysteriousDoorOmState(client, stageId, queue);
            }
        }

        public List<CDataSoulOrdealItem> GetCostByEpitahId(uint epitahId)
        {
            var result = new List<CDataSoulOrdealItem>();

            if (_Server.AssetRepository.EpitaphTrialAssets.EpitahObjects.ContainsKey(epitahId))
            {
                result = GetEpitahObject<EpitaphTrial>(epitahId).UnlockCost;
            }
            else if (_Server.AssetRepository.EpitaphRoadAssets.EpitaphObjects.ContainsKey(epitahId))
            {
                var epitaphObject = _Server.AssetRepository.EpitaphRoadAssets.EpitaphObjects[epitahId];
                if (epitaphObject is EpitaphSection section)
                {
                    result = section.UnlockCost;
                }
                else if (epitaphObject is EpitaphBarrier barrier)
                {
                    result = barrier.UnlockCost;
                }
            }
            return result;
        }

        public bool IsSectionUnlocked(Character character, uint epitaphId)
        {
            var sectionInfo = GetSectionById(epitaphId);
            if (sectionInfo == null)
            {
                return false;
            }

            return (sectionInfo.UnlockCost.Count == 0 && sectionInfo.BarrierDependencies.Count == 0) ||
                   character.EpitaphRoadState.UnlockedContent.Contains(epitaphId);
        }

        public EpitaphPath GetDungeonInfo(NpcId npcId)
        {
            return _Server.AssetRepository.EpitaphRoadAssets.Paths.Values.Where(x => x.NpcId == npcId).FirstOrDefault();
        }

        public EpitaphPath GetDungeonInfo(uint dungeonId)
        {
            return _Server.AssetRepository.EpitaphRoadAssets.Paths.Values.Where(x => x.DungeonId == dungeonId).FirstOrDefault();
        }

        public EpitaphPath GetDungeonInfoByStageId(uint stageId)
        {
            return _Server.AssetRepository.EpitaphRoadAssets.Paths.Values.Where(x => x.StageIds.Contains(stageId)).FirstOrDefault();
        }

        public EpitaphPath GetDungeonInfoByStageId(StageLayoutId stageId)
        {
            return GetDungeonInfoByStageId(stageId.Id);
        }

        public EpitaphPath GetDungeonInfoByHubStageId(uint stageId)
        {
            return _Server.AssetRepository.EpitaphRoadAssets.Paths.Values.Where(x => x.HubStageId == stageId).FirstOrDefault();
        }

        public EpitaphPath GetDungeonInfoByHubStageId(StageLayoutId stageId)
        {
            return GetDungeonInfoByHubStageId(stageId.Id);
        }

        public EpitaphSection GetSectionById(uint epitaphId)
        {
            if (!_Server.AssetRepository.EpitaphRoadAssets.EpitaphObjects.ContainsKey(epitaphId))
            {
                return null;
            }
            return (EpitaphSection) _Server.AssetRepository.EpitaphRoadAssets.EpitaphObjects[epitaphId];
        }

        public bool IsTrialUnlocked(PartyGroup party, EpitaphTrial trial)
        {
            if (trial == null)
            {
                return false;
            }

            if (trial.UnlockCost.Count == 0)
            {
                return true;
            }

            return party.Leader.Client.Character.EpitaphRoadState.UnlockedContent.Contains(trial.EpitaphId);
        }

        public bool IsTrialUnlocked(PartyGroup party, uint epitaphId)
        {
            if (!_Server.AssetRepository.EpitaphTrialAssets.EpitahObjects.ContainsKey(epitaphId))
            {
                return false;
            }

            return IsTrialUnlocked(party, GetEpitahObject<EpitaphTrial>(epitaphId));
        }

        public bool IsTrialUnlocked(PartyGroup party, StageLayoutId stageId, uint posId)
        {
            if (!_Server.AssetRepository.EpitaphTrialAssets.Trials.ContainsKey(stageId))
            {
                return false;
            }

            var trial = GetTrial(stageId, posId);
            if (trial == null)
            {
                return false;
            }

            return IsTrialUnlocked(party, trial);
        }

        public EpitaphStatue GetState(uint epitaphId)
        {
            return GetEpitahObject<EpitaphStatue>(epitaphId);
        }

        public EpitaphStatue GetStatue(StageLayoutId stageId, uint posId)
        {
            if (!_Server.AssetRepository.EpitaphRoadAssets.StatuesByOmId.ContainsKey((stageId, posId)))
            {
                return null;
            }
            return _Server.AssetRepository.EpitaphRoadAssets.StatuesByOmId[(stageId, posId)];
        }

        public bool IsStatueUnlocked(GameClient client, StageLayoutId stageId, uint posId)
        {
            var statue = GetStatue(stageId, posId);
            if (statue == null)
            {
                return false;
            }

            return client.Character.EpitaphRoadState.UnlockedContent.Contains(statue.EpitaphId);
        }

        public void HandleStatueUnlock(GameClient client, StageLayoutId stageId, uint posId)
        {
            Logger.Info($"EpitaphStatueOm: StageId={stageId}, PosId={posId}");

            var statue = GetStatue(stageId, posId);
            if (statue != null)
            {
                _Server.Database.InsertEpitaphRoadUnlock(client.Character.CharacterId, statue.EpitaphId);
                client.Character.EpitaphRoadState.UnlockedContent.Add(statue.EpitaphId);
            }

            // We need to send back a packet still, otherwise the player will get soft locked.
            client.Send(new S2CSeasonDungeonSetOmStateNtc()
            {
                LayoutId = stageId.ToCDataStageLayoutId(),
                PosId = posId,
                State = SoulOrdealOmState.AreaUnlocked
            });
        }

        public EpitaphDoor GetMysteriousDoor(StageLayoutId stageId, uint posId)
        {
            if (!_Server.AssetRepository.EpitaphRoadAssets.DoorsByOmId.ContainsKey((stageId, posId)))
            {
                return null;
            }
            return _Server.AssetRepository.EpitaphRoadAssets.DoorsByOmId[(stageId, posId)];
        }

        public EpitaphDoor GetMysteriousDoor(uint epitaphId)
        {
            return GetEpitahObject<EpitaphDoor>(epitaphId);
        }

        public EpitaphMysteriousDoorState GetMysteriousDoorState(PartyGroup party, StageLayoutId stageId, uint posId)
        {
            lock (_DoorState)
            {
                if (!_DoorState.ContainsKey(party.Id) || !_DoorState[party.Id].ContainsKey((stageId, posId)))
                {
                    if (!_DoorState.ContainsKey(party.Id))
                    {
                        _DoorState[party.Id] = new Dictionary<(StageLayoutId stageId, uint PosId), EpitaphMysteriousDoorState>();
                    }

                    if (!_DoorState[party.Id].ContainsKey((stageId, posId)))
                    {
                        _DoorState[party.Id][(stageId, posId)] = new EpitaphMysteriousDoorState()
                        {
                            State = SoulOrdealOmState.DoorLocked
                        };
                    }
                }
                return _DoorState[party.Id][(stageId, posId)];
            }
        }

        public void SetMysteriousDoorState(PartyGroup party, StageLayoutId stageId, uint posId, SoulOrdealOmState state)
        {
            lock (_DoorState)
            {
                if (!_DoorState.ContainsKey(party.Id))
                {
                    _DoorState[party.Id] = new Dictionary<(StageLayoutId stageId, uint PosId), EpitaphMysteriousDoorState>();
                    _DoorState[party.Id][(stageId, posId)] = new EpitaphMysteriousDoorState();
                }
                _DoorState[party.Id][(stageId, posId)].State = state;
            }
        }

        public void UpdateAllMysteriousDoorOmState(GameClient client, StageLayoutId stageId, PacketQueue queue)
        {
            UpdateAllMysteriousDoorOmState(client, stageId.Id, queue);
        }

        public void UpdateAllMysteriousDoorOmState(GameClient client, uint stageId, PacketQueue queue)
        {
            lock (_DoorState)
            {
                var dungeonInfo = _Server.EpitaphRoadManager.GetDungeonInfoByStageId(stageId);
                if (dungeonInfo == null)
                {
                    return;
                }

                foreach (var door in dungeonInfo.Doors)
                {
                    if (door.StageId.Id != stageId)
                    {
                        continue;
                    }

                    client.Enqueue(new S2CSeasonDungeonSetOmStateNtc()
                    {
                        LayoutId = door.StageId.ToCDataStageLayoutId(),
                        PosId = door.PosId,
                        State = GetMysteriousDoorState(client.Party, door.StageId, door.PosId).State
                    }, queue);
                }
            }
        }

        public void SpreadMysteriousPowers(PartyGroup party, StageLayoutId stageId, uint posId)
        {
            lock (_DoorState)
            {
                var doorState = GetMysteriousDoorState(party, stageId, posId);

                var door = GetMysteriousDoor(stageId, posId);
                doorState.GatheringPoint = door.GatheringPoints[Random.Shared.Next(door.GatheringPoints.Count)];

                party.SendToAll(new S2CSeasonDungeonSetOmStateNtc()
                {
                    LayoutId = doorState.GatheringPoint.StageId.ToCDataStageLayoutId(),
                    State = SoulOrdealOmState.GatheringPointSpawned
                });
            }
        }

        private void HandleMysteriousPowerCollection(GameClient client, EpitaphGatheringPoint gatheringPoint)
        {
            lock (_DoorState)
            {
                var state = GetMysteriousDoorState(client.Party, gatheringPoint.Door.StageId, gatheringPoint.Door.PosId);
                if (state == null)
                {
                    Logger.Error($"Mysterious door state requested but state doesn't exist for StageId={gatheringPoint.Door.StageId}, PosId={gatheringPoint.Door.PosId}, EpitaphId={gatheringPoint.Door.EpitaphId}");
                    return;
                }

                if (!state.GatheringPoint.Equals(gatheringPoint))
                {
                    return;
                }

                SetMysteriousDoorState(client.Party, gatheringPoint.Door.StageId, gatheringPoint.Door.PosId, SoulOrdealOmState.DoorUnlocked);

                client.Party.SendToAll(new S2C_SEASON_62_28_16_NTC()
                {
                    Message = "The mysterious door can be unsealed",
                    LayoutId = gatheringPoint.Door.StageId.ToCDataStageLayoutId(),
                    PosId = gatheringPoint.Door.PosId
                });
            }
        }

        public EpitaphGatheringPoint GetGatheringPoint(StageLayoutId stageId, uint posId)
        {
            if (!_Server.AssetRepository.EpitaphRoadAssets.GatheringPointsByOmId.ContainsKey((stageId, posId)))
            {
                return null;
            }
            return _Server.AssetRepository.EpitaphRoadAssets.GatheringPointsByOmId[(stageId, posId)];
        }

        public EpitaphGatheringPoint GetGatheringPoint(uint epitaphId)
        {
            return GetEpitahObject<EpitaphGatheringPoint>(epitaphId);
        }

        private EpitaphWeeklyReward GetReward(StageLayoutId stageId, uint posId)
        {
            if (!_Server.AssetRepository.EpitaphRoadAssets.ChestsByOmId.ContainsKey((stageId, posId)))
            {
                return null;
            }
            return _Server.AssetRepository.EpitaphRoadAssets.ChestsByOmId[(stageId, posId)];
        }

        public List<EpitaphItemReward> GetRandomLootForStageId(uint stageId)
        {
            if (!_Server.AssetRepository.EpitaphRoadAssets.RandomLootByStageId.ContainsKey(stageId))
            {
                return new();
            }
            return _Server.AssetRepository.EpitaphRoadAssets.RandomLootByStageId[stageId];
        }

        public List<EpitaphItemReward> GetRandomLootForStageId(StageLayoutId stageId)
        {
            return GetRandomLootForStageId(stageId.Id);
        }

        public List<InstancedGatheringItem> RollInstancedGatheringItem(EpitaphItemReward item)
        {
            var results = new List<InstancedGatheringItem>();

            var rolledItems = item.AsCDataGatheringItemElement();
            foreach (var rolledItem in rolledItems)
            {
                if (rolledItem.ItemNum > 0)
                {
                    results.Add(new InstancedGatheringItem()
                    {
                        ItemId = (ItemId) rolledItem.ItemId,
                        ItemNum = rolledItem.ItemNum,
                        IsHidden = rolledItem.IsHidden,
                        Quality = rolledItem.Quality,
                    });
                }
            }

            return results;
        }

        private List<InstancedGatheringItem> RollWeeklyChestReward(EpitaphPath dungeonInfo, EpitaphWeeklyReward reward)
        {
            var rewards = new List<InstancedGatheringItem>();
            foreach (var tableId in reward.DropTablesIds)
            {
                if (!dungeonInfo.DropTables.ContainsKey(tableId))
                {
                    Logger.Error($"The drop table id '{tableId}' doesn't exist for '{dungeonInfo.Name}'. Unable to generate items. Skipping.");
                    continue;
                }

                foreach (var item in dungeonInfo.DropTables[tableId].Items)
                {
                    rewards.AddRange(RollInstancedGatheringItem(item));
                }
            }
            return rewards;
        }

        public List<InstancedGatheringItem> RollGatheringLoot(GameClient client, Character character, StageLayoutId stageId, uint posId)
        {
            var results = new List<InstancedGatheringItem>();

            Logger.Info($"Epitaph gathering point StageId={stageId}, PosId={posId}");

            var dungeonInfo = GetDungeonInfoByStageId(stageId);
            if (dungeonInfo == null)
            {
                return new();
            }

            // For for mysterious door green light
            var gatheringPoint = GetGatheringPoint(stageId, posId);
            if (gatheringPoint != null)
            {
                HandleMysteriousPowerCollection(client, gatheringPoint);
            }

            // Check for weekly reward
            var reward = GetReward(stageId, posId);
            if (reward != null && !character.EpitaphRoadState.WeeklyRewardsClaimed.Contains(reward.EpitaphId))
            {
                results.AddRange(RollWeeklyChestReward(dungeonInfo, reward));

                if (_Server.GameSettings.GameServerSettings.EnableEpitaphWeeklyRewards)
                {
                    character.EpitaphRoadState.WeeklyRewardsClaimed.Add(reward.EpitaphId);
                    _Server.Database.InsertEpitaphWeeklyReward(character.CharacterId, reward.EpitaphId);
                }
            }

            // If it wasn't a special reward, roll some random dungeon items
            if (results.Count == 0)
            {
                foreach (var item in GetRandomLootForStageId(stageId))
                {
                    results.AddRange(RollInstancedGatheringItem(item));
                }

                if (results.Count == 0)
                {
                    // If the player was very unlucky,
                    // put some souls or water flasks into the
                    // result as seen in player videos
                    // TODO: Make this configurable
                    results.Add(new InstancedGatheringItem()
                    {
                        ItemId = (gatheringPoint == null) ? ItemId.WaterFlask : (ItemId) dungeonInfo.SoulItemId,
                        ItemNum = (uint) Random.Shared.Next(1, 4)
                    });
                }
            }

            return results;
        }

        public bool CheckUnlockConditions(GameClient client, EpitaphBarrier barrier)
        {
            foreach (var sectionId in barrier.DependentSectionIds)
            {
                var sectionInfo = _Server.EpitaphRoadManager.GetSectionById(sectionId);
                foreach (var unlockId in sectionInfo.UnlockDependencies)
                {
                    if (!client.Character.EpitaphRoadState.UnlockedContent.Contains(unlockId))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Called by the task manager. The main task will signal all channels
        /// to flush the cached information queried by the player when first
        /// logging in and send a notification to all players that the action
        /// occurred.
        /// </summary>
        public void PerformWeeklyReset()
        {
            _Server.ChatManager.BroadcastMessage(LobbyChatMsgType.ManagementAlertN, "Epitaph Road Weekly Rewards Reset");

            // Clear out cached data related to epitaph weekly rewards
            foreach (var client in _Server.ClientLookup.GetAll())
            {
                client.Character.EpitaphRoadState.WeeklyRewardsClaimed.Clear();
            }
        }
    }
}
