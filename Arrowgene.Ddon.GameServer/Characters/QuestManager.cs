using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Characters
{
    public class QuestManager
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(QuestManager));

        private QuestManager()
        {
        }

        /**
         * @note gQuests contains a map of <QuestScheduleId:QuestId>.
         * @note gVarientQuests maps QuestId:HashSet<QuestScheduleId>.
         *
         * A QuestScheduleId should always get us back to a unique quest object.
         * A QuestId can return us a list of related QuestScheduleIds which all use the same QuestId.
         */
        private static Dictionary<uint, Quest> gQuests = new();
        private static Dictionary<QuestId, HashSet<uint>> gVariantQuests = new();

        // Prevents two concurrent JSON reloads from interleaving their swaps.
        // Readers never acquire this lock; they are safe because the swap replaces
        // collection references rather than mutating live collections, so any reader
        // that captured a reference before the swap iterates the old, immutable dict.
        private static readonly object _reloadLock = new();

        private static Dictionary<QuestType, Dictionary<uint, HashSet<uint>>> QuestByStageNo = new();
        private static Dictionary<QuestAreaId, HashSet<QuestId>> gWorldQuests = new Dictionary<QuestAreaId, HashSet<QuestId>>();
        private static Dictionary<QuestAdventureGuideCategory, HashSet<uint>> gAdventureGuideCategories = new Dictionary<QuestAdventureGuideCategory, HashSet<uint>>();
        private static Dictionary<QuestAreaId, Dictionary<uint,uint>> gAreaTrialRanks = new Dictionary<QuestAreaId, Dictionary<uint, uint>>();

        private static Dictionary<QuestId, (QuestSubstoryGroupId SubstoryGroupId, uint SeqNo)> gSubstoryLookup = new();

        /// <summary>
        /// QuestScheduleIds that are requested as part of World Manage Quests from pcaps.
        /// We know they can't be found, so don't audibly complain about them.
        /// TODO: Remove this when those quests are handled properly.
        /// </summary>
        private static readonly HashSet<uint> KnownBadQuestScheduleIds = new HashSet<uint>()
        {
            0, 25077, 43645, 43646, 47734, 47735, 47736, 47737, 47738, 47739, 49692, 77644, 151381, 208640, 233576, 259411, 259412, 287378, 315624
        };

        private static void AddQuestToCategory(Quest quest)
        {
            AddQuestToCollections(quest, gVariantQuests, QuestByStageNo, gWorldQuests, gAdventureGuideCategories, gAreaTrialRanks);
        }

        // Populates the supplied index collections with the quest. Callers pass either the
        // live static dicts (initial load / scripted hotload) or freshly allocated locals
        // (JSON hotload build-then-swap path).
        private static void AddQuestToCollections(
            Quest quest,
            Dictionary<QuestId, HashSet<uint>> variantQuests,
            Dictionary<QuestType, Dictionary<uint, HashSet<uint>>> questByStageNo,
            Dictionary<QuestAreaId, HashSet<QuestId>> worldQuests,
            Dictionary<QuestAdventureGuideCategory, HashSet<uint>> adventureGuideCategories,
            Dictionary<QuestAreaId, Dictionary<uint, uint>> areaTrialRanks)
        {
            if (!variantQuests.ContainsKey(quest.QuestId))
                variantQuests[quest.QuestId] = new HashSet<uint>();
            variantQuests[quest.QuestId].Add(quest.QuestScheduleId);

            if (quest.QuestType == QuestType.Tutorial || quest.QuestType == QuestType.Substory)
            {
                uint stageNo = (uint)StageManager.ConvertIdToStageNo(quest.StageId);
                if (!questByStageNo.ContainsKey(quest.QuestType))
                    questByStageNo[quest.QuestType] = new();
                var questDict = questByStageNo[quest.QuestType];
                if (!questDict.ContainsKey(stageNo))
                    questDict[stageNo] = new HashSet<uint>();
                questDict[stageNo].Add(quest.QuestScheduleId);
            }
            else if (quest.QuestType == QuestType.World)
            {
                if (!worldQuests.ContainsKey(quest.QuestAreaId))
                    worldQuests[quest.QuestAreaId] = new HashSet<QuestId>();
                worldQuests[quest.QuestAreaId].Add(quest.QuestId);
            }

            if (!adventureGuideCategories.ContainsKey(quest.AdventureGuideCategory))
                adventureGuideCategories[quest.AdventureGuideCategory] = new HashSet<uint>();
            adventureGuideCategories[quest.AdventureGuideCategory].Add(quest.QuestScheduleId);

            // Build a ranking list for quests for area trials
            // TODO: This should probably be done in quest scripts, but who wants to rewrite 70+ quests?
            if (quest.AdventureGuideCategory == QuestAdventureGuideCategory.AreaTrialOrMission)
            {
                // Search all process states for a CheckAreaRank command rather than assuming
                // it is always the very first command - JSON quests and scripted .csx quests
                // serialize their process state lists differently, so .First() is unreliable.
                var questData = quest.ToCDataQuestList(0);
                uint requiredRank = 0;
                QuestAreaId areaId = quest.QuestAreaId;
                bool found = false;

                foreach (var processState in questData.QuestProcessStateList)
                {
                    foreach (var checkCmdGroup in processState.CheckCommandList)
                    {
                        foreach (var cmd in checkCmdGroup.ResultCommandList)
                        {
                            if (cmd.Command == (ushort)QuestCheckCommand.CheckAreaRank)
                            {
                                requiredRank = (uint)cmd.Param02;
                                if (areaId == QuestAreaId.None)
                                    areaId = (QuestAreaId)cmd.Param01;
                                found = true;
                                break;
                            }
                        }
                        if (found) break;
                    }
                    if (found) break;
                }

                if (found)
                {
                    if (!areaTrialRanks.ContainsKey(areaId))
                        areaTrialRanks[areaId] = new Dictionary<uint, uint>();
                    areaTrialRanks[areaId][quest.QuestScheduleId] = requiredRank;
                }
            }
        }

        public static void LoadScriptedQuest(DdonGameServer server, IQuest questScript)
        {
            var quest = questScript.GenerateQuest(server);
            gQuests[quest.QuestScheduleId] = quest;
            if (quest.Enabled)
            {
                AddQuestToCategory(quest);
            }
        }

        private static void ComputeSubstoryLookups(DdonGameServer server)
        {
            var substoryMissionMap = server.GameSettings.Get<Dictionary<QuestSubstoryGroupId, Dictionary<uint, List<QuestId>>>>("substory", "SubstoryMissionMap") ??
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_SERVER_CONFIG_ERROR);

            gSubstoryLookup.Clear();
            foreach (var (substoryGroupId, data) in substoryMissionMap)
            {
                foreach (var (seqNo, questIds) in data)
                {
                    foreach (var questId in questIds)
                    {
                        gSubstoryLookup[questId] = (substoryGroupId, seqNo);
                    }
                }
            }
            server.GameSettings.Set<bool>("substory", "RecomputeSubstoryLookups", false);
        }

        public static (QuestSubstoryGroupId SubstoryGroupId, uint SeqNo) GetSubstoryQuestProperties(DdonGameServer server, QuestId questId)
        {
            if (server.GameSettings.Get<bool>("substory", "RecomputeSubstoryLookups"))
            {
                ComputeSubstoryLookups(server);
            }

            if (!gSubstoryLookup.ContainsKey(questId))
            {
                return (QuestSubstoryGroupId.Invalid, 0);
            }
            return gSubstoryLookup[questId];
        }

        /// <summary>
        /// Called after a substory quest completes. If all quests in the current sequence are done,
        /// advances the sequence step (or marks the group complete). Persists to DB if changed.
        /// </summary>
        public static void AdvanceSubstoryProgress(DdonGameServer server, Character character, QuestId completedQuestId, DbConnection? connectionIn = null)
        {
            var props = GetSubstoryQuestProperties(server, completedQuestId);
            if (props.SubstoryGroupId == QuestSubstoryGroupId.Invalid) return;

            var substorySequenceSettings = server.GameSettings.Get<Dictionary<QuestSubstoryGroupId, List<uint>>>("substory", "SubstorySequence");
            var substoryMissionMap = server.GameSettings.Get<Dictionary<QuestSubstoryGroupId, Dictionary<uint, List<QuestId>>>>("substory", "SubstoryMissionMap");
            if (substorySequenceSettings == null || substoryMissionMap == null) return;
            if (!substorySequenceSettings.ContainsKey(props.SubstoryGroupId)) return;

            if (!character.SubstoryProgress.ContainsKey(props.SubstoryGroupId))
            {
                character.SubstoryProgress[props.SubstoryGroupId] = new SubstoryProgress
                {
                    SubstoryGroupId = props.SubstoryGroupId,
                    SequenceStep = 0,
                    IsComplete = false
                };
            }

            var progress = character.SubstoryProgress[props.SubstoryGroupId];
            if (progress.IsComplete) return;

            var sequences = substorySequenceSettings[props.SubstoryGroupId];
            if (progress.SequenceStep >= sequences.Count) return;

            var currentSeqNo = sequences[progress.SequenceStep];
            if (!substoryMissionMap[props.SubstoryGroupId].ContainsKey(currentSeqNo)) return;

            // Check if every quest in the current sequence is now completed
            var sequenceQuests = substoryMissionMap[props.SubstoryGroupId][currentSeqNo];
            bool allDone = sequenceQuests.TrueForAll(qid => character.CompletedQuests.ContainsKey(qid));
            if (!allDone) return;

            progress.SequenceStep += 1;
            if (progress.SequenceStep >= sequences.Count)
            {
                progress.IsComplete = true;
            }

            server.Database.UpsertSubstoryProgress(character.CharacterId, progress, connectionIn);
        }

        public static void LoadQuests(DdonGameServer server)
        {
            // Iterate over quests generated from json
            foreach (var questAsset in server.AssetRepository.QuestAssets.Quests)
            {
                var quest = GenericQuest.FromAsset(server, questAsset);
                gQuests[quest.QuestScheduleId] = quest;

                if (quest.Enabled)
                {
                    AddQuestToCategory(quest);
                }
            }

            LoadLightQuests(server);

            ComputeSubstoryLookups(server);
        }

        public static void ReloadJsonQuests(DdonGameServer server)
        {
            Logger.Info($"Hotloading JSON quests...");

            // Build entirely new index collections without touching live state.
            // Scripted quests are preserved by copying them from a snapshot of the
            // current live collections; JSON quests are discarded and rebuilt from
            // the freshly loaded asset data.
            var newQuests = new Dictionary<uint, Quest>();
            var newVariantQuests = new Dictionary<QuestId, HashSet<uint>>();
            var newQuestByStageNo = new Dictionary<QuestType, Dictionary<uint, HashSet<uint>>>();
            var newWorldQuests = new Dictionary<QuestAreaId, HashSet<QuestId>>();
            var newAdventureGuideCategories = new Dictionary<QuestAdventureGuideCategory, HashSet<uint>>();
            var newAreaTrialRanks = new Dictionary<QuestAreaId, Dictionary<uint, uint>>();

            // Snapshot current reference so we iterate a stable collection.
            var currentQuests = gQuests;
            foreach (var (scheduleId, quest) in currentQuests)
            {
                if (quest.QuestSource == QuestSource.Json)
                    continue;
                newQuests[scheduleId] = quest;
                AddQuestToCollections(quest, newVariantQuests, newQuestByStageNo, newWorldQuests, newAdventureGuideCategories, newAreaTrialRanks);
            }

            foreach (var questAsset in server.AssetRepository.QuestAssets.Quests)
            {
                var quest = GenericQuest.FromAsset(server, questAsset);
                newQuests[quest.QuestScheduleId] = quest;
                if (quest.Enabled)
                    AddQuestToCollections(quest, newVariantQuests, newQuestByStageNo, newWorldQuests, newAdventureGuideCategories, newAreaTrialRanks);
            }

            // Atomically swap all references under a lock to prevent two concurrent
            // reloads from interleaving. Readers never acquire this lock; they are
            // safe because swapping the reference (not mutating the dict) means any
            // reader that already holds a reference to the old collection will
            // finish iterating it without a "Collection was modified" exception.
            lock (_reloadLock)
            {
                gQuests = newQuests;
                gVariantQuests = newVariantQuests;
                QuestByStageNo = newQuestByStageNo;
                gWorldQuests = newWorldQuests;
                gAdventureGuideCategories = newAdventureGuideCategories;
                gAreaTrialRanks = newAreaTrialRanks;
            }

            Logger.Info($"JSON quest file reloaded. {server.AssetRepository.QuestAssets.Quests.Count} JSON quests total in memory.");
        }

        public static void LoadLightQuests(DdonGameServer server)
        {
            foreach(var quest in server.LightQuestManager.ReadQuests(true))
            {
                gQuests[quest.QuestScheduleId] = quest;
                AddQuestToCategory(quest);
            }
        }

        public static void AddQuests(DdonGameServer server, IEnumerable<Quest> quests)
        {
            foreach(var quest in quests)
            {
                gQuests[quest.QuestScheduleId] = quest;
                AddQuestToCategory(quest);
            }
        }

        public static HashSet<uint> GetQuestsByType(QuestType type)
        {
            HashSet<uint> results = new HashSet<uint>();

            // TODO: We probably need to optimize this as more quests are added
            foreach (var (scheduleId, quest) in gQuests)
            {
                if (quest.QuestType == type)
                {
                    results.Add(quest.QuestScheduleId);
                }
            }

            return results;
        }

        public static HashSet<QuestId> GetWorldQuestIdsByAreaId(QuestAreaId areaId)
        {
            var snapshot = gWorldQuests;
            return snapshot.TryGetValue(areaId, out var ids) ? ids : new HashSet<QuestId>();
        }

        public static Quest GetQuestByBoardId(ulong boardId)
        {
            uint questId = BoardManager.GetQuestIdFromBoardId(boardId);
            return GetQuestByScheduleId(questId);
        }

        public static HashSet<uint> GetQuestByStageNo(QuestType questType, uint stageNo)
        {
            var snapshot = QuestByStageNo;
            if (!snapshot.TryGetValue(questType, out var byStage))
                return new();
            return byStage.TryGetValue(stageNo, out var ids) ? ids : new();
        }

        public static bool IsVariantQuest(QuestId baseQuestId)
        {
            return gVariantQuests.ContainsKey(baseQuestId);
        }

        public static Quest GetQuestByScheduleId(uint questScheduleId)
        {
            var snapshot = gQuests;
            if (snapshot.TryGetValue(questScheduleId, out var quest))
                return quest;
            if (!KnownBadQuestScheduleIds.Contains(questScheduleId) && !IsBoardQuest(questScheduleId))
                Logger.Error($"GetQuestByScheduleId: Invalid questScheduleId {questScheduleId}");
            return null;
        }

        public static Quest GetQuestByQuestId(QuestId questId)
        {
            var questScheduleIds = GetQuestScheduleIdsForQuestId(questId);
            if (questScheduleIds.Count > 1)
            {
                throw new Exception($"The quest {questId} has multiple implementations. Use GetQuestScheduleIdsForQuestId instead.");
            }

            return questScheduleIds.Count > 0 ? QuestManager.GetQuestByScheduleId(questScheduleIds.ToList()[0]) : null;
        }

        public static HashSet<Quest> GetQuestsByQuestId(QuestId questId)
        {
            var questScheduleIds = GetQuestScheduleIdsForQuestId(questId);
            return questScheduleIds.Select(x => QuestManager.GetQuestByScheduleId(x))
                .Where(x => x is not null)
                .ToHashSet();
        }

        public static HashSet<uint> GetQuestScheduleIdsForQuestId(QuestId questId)
        {
            var snapshot = gVariantQuests;
            return snapshot.TryGetValue(questId, out var ids) ? ids : new HashSet<uint>();
        }

        public static Quest RollQuestForQuestId(QuestId questId)
        {
            var quests = GetQuestScheduleIdsForQuestId(questId);
            var questScheduleId = quests.ElementAt(Random.Shared.Next(0, quests.Count));
            var snapshot = gQuests;
            return snapshot.TryGetValue(questScheduleId, out var quest) ? quest : null;
        }

        public static HashSet<uint> GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory category)
        {
            var snapshot = gAdventureGuideCategories;
            return snapshot.TryGetValue(category, out var ids) ? ids.ToHashSet() : new();
        }

        public static bool IsQuestEnabled(uint questScheduleId)
        {
            var quest = GetQuestByScheduleId(questScheduleId);
            return (quest == null) ? false : quest.Enabled;
        }

        public static QuestStateManager GetQuestStateManager(GameClient client, Quest quest)
        {
            return quest.IsPersonal ? client.QuestState : client.Party.QuestState;
        }

        public static QuestStateManager GetQuestStateManager(GameClient client, uint questScheduleId)
        {
            return GetQuestStateManager(client, QuestManager.GetQuestByScheduleId(questScheduleId));
        }

        public static bool IsClientAlignedForMainQuestProgress(DdonGameServer server, GameClient client, Quest quest, uint step, DbConnection? connectionIn = null)
        {
            if (quest.QuestType != QuestType.Main)
            {
                return true;
            }

            if (client.Character.HasQuestCompleted(quest.QuestId))
            {
                return false;
            }

            var progress = server.Database.GetQuestProgressByScheduleId(client.Character.CommonId, quest.QuestScheduleId, connectionIn);
            return progress != null && (quest.SaveWorkAsStep || progress.Step == step);
        }

        public static HashSet<uint> CollectQuestScheduleIds(GameClient client, StageLayoutId stageId)
        {
            var questScheduleIds = new HashSet<uint>();

            questScheduleIds.UnionWith(client.Party.QuestState.StageQuests(stageId));
            if (client.Party.Leader is not null)
            {
                questScheduleIds.UnionWith(client.Party.Leader.QuestState.StageQuests(stageId));
            }

            return questScheduleIds;
        }

        public static void PurgeUnstartedTutorialQuests(GameClient client)
        {
            var unstartedTutorialQuests = client.QuestState.GetActiveQuestScheduleIds()
                .Select(x => QuestManager.GetQuestByScheduleId(x))
                .Where(x => x != null && x.QuestType == QuestType.Tutorial)
                .Where(x => {
                    var mgr = QuestManager.GetQuestStateManager(client, x);
                    return mgr != null && mgr.GetQuestState(x.QuestScheduleId)?.State == QuestProgressState.Unknown;
                })
                .ToList();

            foreach (var quest in unstartedTutorialQuests)
            {
                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                if (questStateManager != null)
                {
                    questStateManager.RemoveQuest(quest);
                }
            }
        }

        public static HashSet<uint> GetActiveQuestScheduleIds(GameClient client, QuestType questType = QuestType.All)
        {
            var activeQuestScheduleIds = client.Party.QuestState.GetActiveQuestScheduleIds()
                .Where(x => (questType == QuestType.All) || QuestManager.GetQuestByScheduleId(x).QuestType == questType)
                .ToList()
                .ToHashSet();
            activeQuestScheduleIds.UnionWith(
                client.QuestState.GetActiveQuestScheduleIds()
                    .Where(x => (questType == QuestType.All) || QuestManager.GetQuestByScheduleId(x).QuestType == questType)
                    .ToList()
                    .ToHashSet());
            return activeQuestScheduleIds;
        }

        public static Dictionary<uint,uint> GetAreaTrialRankings(QuestAreaId areaId)
        {
            if (!gAreaTrialRanks.ContainsKey(areaId))
            {
                return new();
            }
            return gAreaTrialRanks[areaId];
        }

        public static QuestAreaId GetAreaIdForTrial(uint questScheduleId)
        {
            foreach (var (areaId, rankings) in gAreaTrialRanks)
            {
                if (rankings.ContainsKey(questScheduleId))
                    return areaId;
            }
            return QuestAreaId.None;
        }

        public static uint GetScheduleId(DdonGameServer server, QuestId questId, uint variantNumber)
        {
            if (IsDatabaseManaged(questId, out uint baseScheduleId))
            {
                int bits = 27;
                int maxVariant = 2 << bits;
                if (variantNumber >= maxVariant)
                {
                    throw new Exception($"Invalid variant number {variantNumber} > {maxVariant} for quest {questId}.");
                }
                return baseScheduleId + variantNumber;
            }
            else
            {
                if (variantNumber >= 128)
                {
                    throw new Exception($"Invalid variant number {variantNumber} > 127 for quest {questId}.");
                }

                return server.AssetRepository.QuestScheduleIdAsset[questId] + variantNumber;
            }
        }

        public static uint GetVariantIndex(DdonGameServer server, QuestId questId, uint questScheduleId)
        {
            if (IsDatabaseManaged(questId, out uint baseScheduleId))
            {
                return QuestScheduleId.GetRotatingVariant(questScheduleId);
            }
            else
            {
                return QuestScheduleId.GetVariant(questScheduleId);
            }
        }

        /// <summary>
        /// For quests that are managed from the database.
        /// These quests have schedule IDs that are defined solely by type + offset, and so accept larger offsets.
        /// </summary>
        public static bool IsDatabaseManaged(QuestId questId, out uint baseScheduleId)
        {
            baseScheduleId = 0;

            // Light Quests
            if (IsBoardQuest(questId))
            {
                baseScheduleId = QuestScheduleId.GenerateRotatingId(4, 0);
                return true;
            }

            // TODO: Clan Quests, Wild Hunt

            return false;
        }

        public static List<QuestProgressWork> CollectWorkItems(GameClient client, QuestProgressWorkType workType)
        {
            return client.QuestState.ProgressWork[workType].Concat(client.Party.QuestState.ProgressWork[workType]).ToList();
        }

        public class LayoutFlag
        {
            public static CDataQuestLayoutFlagSetInfo Create(uint layoutFlag, uint stageNo, uint groupId)
            {
                return new CDataQuestLayoutFlagSetInfo()
                {
                    LayoutFlagNo = layoutFlag,
                    SetInfoList = new List<CDataQuestSetInfo>()
                    {
                        new CDataQuestSetInfo()
                        {
                            StageNo = stageNo,
                            GroupId = groupId
                        }
                    }
                };
            }

            public static CDataQuestLayoutFlagSetInfo Create(uint layoutFlag, StageInfo stageInfo, uint groupId)
            {
                return Create(layoutFlag, stageInfo.StageNo, groupId);
            }
        }

        public class AcceptConditions
        {
            public static CDataQuestOrderConditionParam NoRestriction()
            {
                return new CDataQuestOrderConditionParam() { Type = 0x0 };
            }
            public static CDataQuestOrderConditionParam MinimumLevelRestriction(uint level)
            {
                return new CDataQuestOrderConditionParam() { Type = 0x1, Param01 = (int)level };
            }

            public static CDataQuestOrderConditionParam MinimumVocationRestriction(JobId jobId, uint level)
            {
                return new CDataQuestOrderConditionParam() { Type = 0x2, Param01 = (int)jobId, Param02 = (int)level };
            }

            public static CDataQuestOrderConditionParam Solo()
            {
                return new CDataQuestOrderConditionParam() { Type = 0x3 };
            }

            public static CDataQuestOrderConditionParam MainQuestCompletionRestriction(QuestId questId)
            {
                return new CDataQuestOrderConditionParam() { Type = 0x6, Param01 = (int)questId };
            }

            public static CDataQuestOrderConditionParam ClearTutorialQuestRestriction(int param01, int param02 = 0)
            {
                return new CDataQuestOrderConditionParam() { Type = 0x7, Param01 = param01, Param02 = param02 };
            }

            public static CDataQuestOrderConditionParam ClearTutorialQuestRestriction(QuestId questId, int param02 = 0)
            {
                return new CDataQuestOrderConditionParam() { Type = 0x7, Param01 = (int)questId, Param02 = param02 };
            }
        }

        public static CDataQuestProcessState CreateQuestProcessState(ushort processNo, ushort sequenceNo, ushort blockNo, List<CDataQuestCommand> resultCommands, List<CDataQuestCommand> checkCommands)
        {
            return new CDataQuestProcessState()
            {
                ProcessNo = processNo,
                SequenceNo = sequenceNo,
                BlockNo = blockNo,
                ResultCommandList = resultCommands,
                CheckCommandList = QuestManager.CheckCommand.AddCheckCommands(checkCommands)
            };
        }

        public class CheckCommand
        {
            public static List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> AddCheckCommands(List<CDataQuestCommand> commands)
            {
                List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> result = new List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand>();

                // This struct seems to be a list serialized inside of a list
                result.Add(new CDataQuestProcessState.MtTypedArrayCDataQuestCommand());
                result[0].ResultCommandList = commands;

                return result;
            }

            public static List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> AddCheckCommands(List<CDataQuestCommand> commands0, List<CDataQuestCommand> commands1)
            {
                List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> result = new List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand>();
                result.Add(new CDataQuestProcessState.MtTypedArrayCDataQuestCommand());
                result.Add(new CDataQuestProcessState.MtTypedArrayCDataQuestCommand());
                result[0].ResultCommandList = commands0;
                result[1].ResultCommandList = commands1;
                return result;
            }

            public static List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> AddCheckCommands(List<List<CDataQuestCommand>> commands)
            {
                List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> result = new List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand>();
                foreach (List<CDataQuestCommand> commandList in commands)
                {
                    var checkCommands = new CDataQuestProcessState.MtTypedArrayCDataQuestCommand();
                    checkCommands.ResultCommandList = commandList;
                    result.Add(checkCommands);
                }
                return result;
            }

            public static List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> AppendCheckCommand(List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> obj, CDataQuestCommand command)
            {
                obj[0].ResultCommandList.Add(command);
                return obj;
            }

            public static List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> AppendCheckCommands(List<CDataQuestProcessState.MtTypedArrayCDataQuestCommand> obj, List<CDataQuestCommand> commands)
            {
                obj[0].ResultCommandList.AddRange(commands);
                return obj;
            }


            /**
             * @brief
             * @param stageNo
             * @param npcId
             */
            public static CDataQuestCommand TalkNpc(uint stageNo, NpcId npcId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TalkNpc, Param01 = (int)stageNo, Param02 = (int)npcId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand DieEnemy(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DieEnemy, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param sceNo
             */
            public static CDataQuestCommand SceHitIn(uint stageNo, int sceNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SceHitIn, Param01 = (int)stageNo, Param02 = sceNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param itemId
             * @param itemNum
             */
            public static CDataQuestCommand HaveItem(int itemId, int itemNum, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.HaveItem, Param01 = itemId, Param02 = itemNum, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param itemId
             * @param itemNum
             * @param npcId
             * @param msgNo
             */
            public static CDataQuestCommand DeliverItem(int itemId, int itemNum, NpcId npcId = NpcId.None, int msgNo = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DeliverItem, Param01 = itemId, Param02 = itemNum, Param03 = (int)npcId, Param04 = msgNo };
            }

            /**
             * @brief
             * @param enemyNameId (NOT enemyId)
             * @param enemyLv
             * @param enemyNum
             */
            public static CDataQuestCommand EmDieLight(int enemyId, int enemyLv, int enemyNum, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.EmDieLight, Param01 = enemyId, Param02 = enemyLv, Param03 = enemyNum, Param04 = param04 };
            }

            /**
             * @brief
             * @param questId
             * @param flagNo
             */
            public static CDataQuestCommand QstFlagOn(int questId, int flagNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QstFlagOn, Param01 = questId, Param02 = flagNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param questId
             * @param flagNo
             */
            public static CDataQuestCommand QstFlagOff(int questId, int flagNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QstFlagOff, Param01 = questId, Param02 = flagNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand MyQstFlagOn(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.MyQstFlagOn, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand MyQstFlagOff(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.MyQstFlagOff, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand Padding00(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.Padding00, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand Padding01(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.Padding01, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand Padding02(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.Padding02, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             */
            public static CDataQuestCommand StageNo(uint stageNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.StageNo, Param01 = (int)stageNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param eventNo
             */
            public static CDataQuestCommand EventEnd(uint stageNo, int eventNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.EventEnd, Param01 = (int)stageNo, Param02 = eventNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param x
             * @param y
             * @param z
             */
            public static CDataQuestCommand Prt(uint stageNo, int x, int y, int z)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.Prt, Param01 = (int)stageNo, Param02 = x, Param03 = y, Param04 = z };
            }

            /**
             * @brief
             * @param minCount
             * @param maxCount
             */
            public static CDataQuestCommand Clearcount(int minCount, int maxCount, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.Clearcount, Param01 = minCount, Param02 = maxCount, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand SceFlagOn(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SceFlagOn, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand SceFlagOff(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SceFlagOff, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param npcId
             */
            public static CDataQuestCommand TouchActToNpc(uint stageNo, NpcId npcId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TouchActToNpc, Param01 = (int)stageNo, Param02 = (int)npcId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param npcId
             */
            public static CDataQuestCommand OrderDecide(NpcId npcId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OrderDecide, Param01 = (int)npcId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsEndCycle(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEndCycle, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsInterruptCycle(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsInterruptCycle, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsFailedCycle(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsFailedCycle, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsEndResult(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEndResult, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief Used to order a quest from an NPC with multiple talking options.
             * @param stageNo
             * @param npcId
             * @param noOrderGroupSerial
             */
            public static CDataQuestCommand NpcTalkAndOrderUi(uint stageNo, NpcId npcId, int noOrderGroupSerial, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NpcTalkAndOrderUi, Param01 = (int)stageNo, Param02 = (int)npcId, Param03 = noOrderGroupSerial, Param04 = param04 };
            }

            /**
             * @brief Used to order a quest from an NPC with no additional talking options.
             * @param stageNo
             * @param npcId
             * @param noOrderGroupSerial
             */
            public static CDataQuestCommand NpcTouchAndOrderUi(uint stageNo, NpcId npcId, int noOrderGroupSerial, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NpcTouchAndOrderUi, Param01 = (int)stageNo, Param02 = (int)npcId, Param03 = noOrderGroupSerial, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             */
            public static CDataQuestCommand StageNoNotEq(uint stageNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.StageNoNotEq, Param01 = (int)stageNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param warLevel
             */
            public static CDataQuestCommand Warlevel(int warLevel, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.Warlevel, Param01 = warLevel, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param npcId
             */
            public static CDataQuestCommand TalkNpcWithoutMarker(uint stageNo, NpcId npcId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TalkNpcWithoutMarker, Param01 = (int)stageNo, Param02 = (int)npcId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param gold
             * @param type
             */
            public static CDataQuestCommand HaveMoney(int gold, int type, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.HaveMoney, Param01 = gold, Param02 = type, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param clearNum
             * @param areaId
             */
            public static CDataQuestCommand SetQuestClearNum(int clearNum, int areaId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SetQuestClearNum, Param01 = clearNum, Param02 = areaId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand MakeCraft(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.MakeCraft, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand PlayEmotion(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.PlayEmotion, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param timerNo
             */
            public static CDataQuestCommand IsEndTimer(int timerNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEndTimer, Param01 = timerNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsEnemyFound(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEnemyFound, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param randomNo
             * @param value
             */
            public static CDataQuestCommand RandomEq(int randomNo, int value, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.RandomEq, Param01 = randomNo, Param02 = value, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param randomNo
             * @param value
             */
            public static CDataQuestCommand RandomNotEq(int randomNo, int value, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.RandomNotEq, Param01 = randomNo, Param02 = value, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param randomNo
             * @param value
             */
            public static CDataQuestCommand RandomLess(int randomNo, int value, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.RandomLess, Param01 = randomNo, Param02 = value, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param randomNo
             * @param value
             */
            public static CDataQuestCommand RandomNotGreater(int randomNo, int value, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.RandomNotGreater, Param01 = randomNo, Param02 = value, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param randomNo
             * @param value
             */
            public static CDataQuestCommand RandomGreater(int randomNo, int value, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.RandomGreater, Param01 = randomNo, Param02 = value, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param randomNo
             * @param value
             */
            public static CDataQuestCommand RandomNotLess(int randomNo, int value, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.RandomNotLess, Param01 = randomNo, Param02 = value, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param div
             * @param value
             */
            public static CDataQuestCommand Clearcount02(int div, int value, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.Clearcount02, Param01 = div, Param02 = value, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param minTime
             * @param maxTime
             */
            public static CDataQuestCommand IngameTimeRangeEq(int minTime, int maxTime, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IngameTimeRangeEq, Param01 = minTime, Param02 = maxTime, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param minTime
             * @param maxTime
             */
            public static CDataQuestCommand IngameTimeRangeNotEq(int minTime, int maxTime, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IngameTimeRangeNotEq, Param01 = minTime, Param02 = maxTime, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param hpRate
             * @param type
             */
            public static CDataQuestCommand PlHp(int hpRate, int type, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.PlHp, Param01 = hpRate, Param02 = type, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param hpRate
             */
            public static CDataQuestCommand EmHpNotLess(uint stageNo, int groupNo, int setNo, int hpRate)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.EmHpNotLess, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = hpRate };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param hpRate
             */
            public static CDataQuestCommand EmHpLess(uint stageNo, int groupNo, int setNo, int hpRate)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.EmHpLess, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = hpRate };
            }

            /**
             * @brief
             * @param weatherId
             */
            public static CDataQuestCommand WeatherEq(int weatherId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.WeatherEq, Param01 = weatherId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param weatherId
             */
            public static CDataQuestCommand WeatherNotEq(int weatherId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.WeatherNotEq, Param01 = weatherId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param jobId
             */
            public static CDataQuestCommand PlJobEq(int jobId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.PlJobEq, Param01 = jobId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param jobId
             */
            public static CDataQuestCommand PlJobNotEq(int jobId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.PlJobNotEq, Param01 = jobId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param sex
             */
            public static CDataQuestCommand PlSexEq(int sex, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.PlSexEq, Param01 = sex, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param sex
             */
            public static CDataQuestCommand PlSexNotEq(int sex, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.PlSexNotEq, Param01 = sex, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param sceNo
             */
            public static CDataQuestCommand SceHitOut(uint stageNo, int sceNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SceHitOut, Param01 = (int)stageNo, Param02 = sceNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand WaitOrder(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.WaitOrder, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand OmSetTouch(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OmSetTouch, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand OmReleaseTouch(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OmReleaseTouch, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param checkType
             * @param level
             */
            public static CDataQuestCommand JobLevelNotLess(int checkType, int level, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.JobLevelNotLess, Param01 = checkType, Param02 = level, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param checkType
             * @param level
             */
            public static CDataQuestCommand JobLevelLess(int checkType, int level, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.JobLevelLess, Param01 = checkType, Param02 = level, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand MyQstFlagOnFromFsm(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.MyQstFlagOnFromFsm, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param sceNo
             */
            public static CDataQuestCommand SceHitInWithoutMarker(uint stageNo, int sceNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SceHitInWithoutMarker, Param01 = (int)stageNo, Param02 = sceNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param sceNo
             */
            public static CDataQuestCommand SceHitOutWithoutMarker(uint stageNo, int sceNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SceHitOutWithoutMarker, Param01 = (int)stageNo, Param02 = sceNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param idx
             * @param num
             */
            public static CDataQuestCommand KeyItemPoint(int idx, int num, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.KeyItemPoint, Param01 = idx, Param02 = num, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param timerNo
             */
            public static CDataQuestCommand IsNotEndTimer(int timerNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsNotEndTimer, Param01 = timerNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param questId
             */
            public static CDataQuestCommand IsMainQuestClear(int questId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsMainQuestClear, Param01 = questId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand DogmaOrb(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DogmaOrb, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsEnemyFoundForOrder(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEnemyFoundForOrder, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand IsTutorialFlagOn(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsTutorialFlagOn, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand QuestOmSetTouch(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestOmSetTouch, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand QuestOmReleaseTouch(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestOmReleaseTouch, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand NewTalkNpc(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NewTalkNpc, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand NewTalkNpcWithoutMarker(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NewTalkNpcWithoutMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             * @param questId
             */
            public static CDataQuestCommand IsTutorialQuestClear(int questId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsTutorialQuestClear, Param01 = questId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param questId
             */
            public static CDataQuestCommand IsMainQuestOrder(int questId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsMainQuestOrder, Param01 = questId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param questId
             */
            public static CDataQuestCommand IsTutorialQuestOrder(int questId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsTutorialQuestOrder, Param01 = questId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsTouchPawnDungeonOm(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsTouchPawnDungeonOm, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand IsOpenDoorOmQuestSet(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOpenDoorOmQuestSet, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             * @param stageNo
             * @param enemyId
             * @param enemyNum
             */
            public static CDataQuestCommand EmDieForRandomDungeon(uint stageNo, int enemyId, int enemyNum, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.EmDieForRandomDungeon, Param01 = (int)stageNo, Param02 = enemyId, Param03 = enemyNum, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param hpRate
             */
            public static CDataQuestCommand NpcHpNotLess(uint stageNo, int groupNo, int setNo, int hpRate)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NpcHpNotLess, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = hpRate };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param hpRate
             */
            public static CDataQuestCommand NpcHpLess(uint stageNo, int groupNo, int setNo, int hpRate)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NpcHpLess, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = hpRate };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsEnemyFoundWithoutMarker(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEnemyFoundWithoutMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsEventBoardAccepted(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEventBoardAccepted, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             * @param questId
             */
            public static CDataQuestCommand WorldManageQuestFlagOn(int flagNo, int questId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.WorldManageQuestFlagOn, Param01 = flagNo, Param02 = questId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             * @param questId
             */
            public static CDataQuestCommand WorldManageQuestFlagOff(int flagNo, int questId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.WorldManageQuestFlagOff, Param01 = flagNo, Param02 = questId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand TouchEventBoard(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TouchEventBoard, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenEntryRaidBoss(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenEntryRaidBoss, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OepnEntryFortDefense(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OepnEntryFortDefense, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand DiePlayer(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DiePlayer, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param partyMemberNum
             */
            public static CDataQuestCommand PartyNumNotLessWtihoutPawn(int partyMemberNum, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.PartyNumNotLessWtihoutPawn, Param01 = partyMemberNum, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param partyMemberNum
             */
            public static CDataQuestCommand PartyNumNotLessWithPawn(int partyMemberNum, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.PartyNumNotLessWithPawn, Param01 = partyMemberNum, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand LostMainPawn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.LostMainPawn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand SpTalkNpc(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SpTalkNpc, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OepnJobMaster(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OepnJobMaster, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand TouchRimStone(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TouchRimStone, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand GetAchievement(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.GetAchievement, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand DummyNotProgress(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DummyNotProgress, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand DieRaidBoss(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DieRaidBoss, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand CycleTimerZero(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.CycleTimerZero, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param timeSec
             */
            public static CDataQuestCommand CycleTimer(int timeSec, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.CycleTimer, Param01 = timeSec, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand QuestNpcTalkAndOrderUi(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestNpcTalkAndOrderUi, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand QuestNpcTouchAndOrderUi(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestNpcTouchAndOrderUi, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param enemyId
             */
            public static CDataQuestCommand IsFoundRaidBoss(uint stageNo, int groupNo, int setNo, int enemyId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsFoundRaidBoss, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = enemyId };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand QuestOmSetTouchWithoutMarker(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestOmSetTouchWithoutMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand QuestOmReleaseTouchWithoutMarker(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestOmReleaseTouchWithoutMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param npcId
             */
            public static CDataQuestCommand TutorialTalkNpc(uint stageNo, NpcId npcId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TutorialTalkNpc, Param01 = (int)stageNo, Param02 = (int)npcId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsLogin(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsLogin, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsPlayEndFirstSeasonEndCredit(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsPlayEndFirstSeasonEndCredit, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand IsKilledTargetEnemySetGroup(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsKilledTargetEnemySetGroup, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand IsKilledTargetEmSetGrpNoMarker(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsKilledTargetEmSetGrpNoMarker, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param timeSec
             */
            public static CDataQuestCommand IsLeftCycleTimer(int timeSec, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsLeftCycleTimer, Param01 = timeSec, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand OmEndText(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OmEndText, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand QuestOmEndText(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestOmEndText, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param areaId
             */
            public static CDataQuestCommand OpenAreaMaster(int areaId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenAreaMaster, Param01 = areaId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param itemId
             * @param itemNum
             */
            public static CDataQuestCommand HaveItemAllBag(int itemId, int itemNum, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.HaveItemAllBag, Param01 = itemId, Param02 = itemNum, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenNewspaper(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenNewspaper, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenQuestBoard(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenQuestBoard, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             */
            public static CDataQuestCommand StageNoWithoutMarker(uint stageNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.StageNoWithoutMarker, Param01 = (int)stageNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand TalkQuestNpcUnitMarker(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TalkQuestNpcUnitMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand TouchQuestNpcUnitMarker(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TouchQuestNpcUnitMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsExistSecondPawn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsExistSecondPawn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsOrderJobTutorialQuest(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOrderJobTutorialQuest, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsOpenWarehouse(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOpenWarehouse, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param FlagNo
             */
            public static CDataQuestCommand IsMyquestLayoutFlagOn(int FlagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsMyquestLayoutFlagOn, Param01 = FlagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param FlagNo
             */
            public static CDataQuestCommand IsMyquestLayoutFlagOff(int FlagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsMyquestLayoutFlagOff, Param01 = FlagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsOpenWarehouseReward(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOpenWarehouseReward, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsOrderLightQuest(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOrderLightQuest, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsOrderWorldQuest(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOrderWorldQuest, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsLostMainPawn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsLostMainPawn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsFullOrderQuest(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsFullOrderQuest, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsBadStatus(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsBadStatus, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param AreaId
             * @param AreaRank
             */
            public static CDataQuestCommand CheckAreaRank(int AreaId, int AreaRank, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.CheckAreaRank, Param01 = AreaId, Param02 = AreaRank, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand Padding133(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.Padding133, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand EnablePartyWarp(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.EnablePartyWarp, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsHugeble(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsHugeble, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsDownEnemy(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsDownEnemy, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenAreaMasterSupplies(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenAreaMasterSupplies, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenEntryBoard(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenEntryBoard, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand NoticeInterruptContents(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NoticeInterruptContents, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenRetrySelect(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenRetrySelect, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsPlWeakening(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsPlWeakening, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand NoticePartyInvite(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NoticePartyInvite, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsKilledAreaBoss(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsKilledAreaBoss, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsPartyReward(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsPartyReward, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsFullBag(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsFullBag, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenCraftExam(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenCraftExam, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand LevelUpCraft(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.LevelUpCraft, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsClearLightQuest(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsClearLightQuest, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenJobMasterReward(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenJobMasterReward, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand TouchActQuestNpc(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TouchActQuestNpc, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             * @param pawnNum
             */
            public static CDataQuestCommand IsLeaderAndJoinPawn(int pawnNum, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsLeaderAndJoinPawn, Param01 = pawnNum, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsAcceptLightQuest(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsAcceptLightQuest, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsReleaseWarpPoint(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsReleaseWarpPoint, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsSetPlayerSkill(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsSetPlayerSkill, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsOrderMyQuest(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOrderMyQuest, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsNotOrderMyQuest(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsNotOrderMyQuest, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand HasMypawn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.HasMypawn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param warpPointId
             */
            public static CDataQuestCommand IsFavoriteWarpPoint(int warpPointId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsFavoriteWarpPoint, Param01 = warpPointId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand Craft(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.Craft, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand IsKilledTargetEnemySetGroupGmMain(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsKilledTargetEnemySetGroupGmMain, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand IsKilledTargetEnemySetGroupGmSub(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsKilledTargetEnemySetGroupGmSub, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand HasUsedKey(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.HasUsedKey, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsCycleFlagOffPeriod(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsCycleFlagOffPeriod, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsEnemyFoundGmMain(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEnemyFoundGmMain, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsEnemyFoundGmSub(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEnemyFoundGmSub, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsLoginBugFixedOnly(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsLoginBugFixedOnly, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsSearchClan(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsSearchClan, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsOpenAreaListUi(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOpenAreaListUi, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param warpPointId
             */
            public static CDataQuestCommand IsReleaseWarpPointAnyone(int warpPointId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsReleaseWarpPointAnyone, Param01 = warpPointId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand DevidePlayer(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DevidePlayer, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param phaseId
             */
            public static CDataQuestCommand NowPhase(int phaseId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NowPhase, Param01 = phaseId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsReleasePortal(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsReleasePortal, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsGetAppraiseItem(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsGetAppraiseItem, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsSetPartnerPawn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsSetPartnerPawn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsPresentPartnerPawn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsPresentPartnerPawn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsReleaseMyRoom(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsReleaseMyRoom, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsExistDividePlayer(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsExistDividePlayer, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand NotDividePlayer(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NotDividePlayer, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             */
            public static CDataQuestCommand IsGatherPartyInStage(uint stageNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsGatherPartyInStage, Param01 = (int)stageNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsFinishedEnemyDivideAction(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsFinishedEnemyDivideAction, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param questId
             */
            public static CDataQuestCommand IsOpenDoorOmQuestSetNoMarker(uint stageNo, int groupNo, int setNo, int questId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOpenDoorOmQuestSetNoMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = questId };
            }

            /**
             * @brief
             * @param stageNo
             * @param eventNo
             */
            public static CDataQuestCommand IsFinishedEventOrderNum(uint stageNo, int eventNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsFinishedEventOrderNum, Param01 = (int)stageNo, Param02 = eventNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsPresentPartnerPawnNoMarker(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsPresentPartnerPawnNoMarker, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsOmBrokenLayout(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOmBrokenLayout, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsOmBrokenQuest(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOmBrokenQuest, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsHoldingPeriodCycleContents(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsHoldingPeriodCycleContents, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsNotHoldingPeriodCycleContents(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsNotHoldingPeriodCycleContents, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsResetInstanceArea(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsResetInstanceArea, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param moonAgeStart
             * @param moonAgeEnd
             */
            public static CDataQuestCommand CheckMoonAge(int moonAgeStart, int moonAgeEnd, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.CheckMoonAge, Param01 = moonAgeStart, Param02 = moonAgeEnd, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param orderGroupSerial
             * @param noOrderGroupSerial
             */
            public static CDataQuestCommand IsOrderPawnQuest(int orderGroupSerial, int noOrderGroupSerial, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOrderPawnQuest, Param01 = orderGroupSerial, Param02 = noOrderGroupSerial, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsTakePictures(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsTakePictures, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             */
            public static CDataQuestCommand IsStageForMainQuest(uint stageNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsStageForMainQuest, Param01 = (int)stageNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsReleasePawnExpedition(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsReleasePawnExpedition, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenPpMode(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenPpMode, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param point
             */
            public static CDataQuestCommand PpNotLess(int point, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.PpNotLess, Param01 = point, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand OpenPpShop(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OpenPpShop, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand TouchClanBoard(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TouchClanBoard, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsOneOffGather(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOneOffGather, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsOmBrokenLayoutNoMarker(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOmBrokenLayoutNoMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             */
            public static CDataQuestCommand IsOmBrokenQuestNoMarker(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOmBrokenQuestNoMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param idx
             * @param num
             */
            public static CDataQuestCommand KeyItemPointEq(int idx, int num, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.KeyItemPointEq, Param01 = idx, Param02 = num, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param actNo
             */
            public static CDataQuestCommand IsEmotion(int actNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEmotion, Param01 = actNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param color
             */
            public static CDataQuestCommand IsEquipColor(int color, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEquipColor, Param01 = color, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param itemId
             */
            public static CDataQuestCommand IsEquip(int itemId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEquip, Param01 = itemId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param npcId01
             * @param npcId02
             * @param npcId03
             */
            public static CDataQuestCommand IsTakePicturesNpc(uint stageNo, int npcId01, int npcId02, int npcId03)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsTakePicturesNpc, Param01 = (int)stageNo, Param02 = npcId01, Param03 = npcId02, Param04 = npcId03 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand SayMessage(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SayMessage, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param x
             * @param y
             * @param z
             */
            public static CDataQuestCommand IsTakePicturesWithoutPawn(uint stageNo, int x, int y, int z)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsTakePicturesWithoutPawn, Param01 = (int)stageNo, Param02 = x, Param03 = y, Param04 = z };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param flagNo
             */
            public static CDataQuestCommand IsLinkageEnemyFlag(uint stageNo, int groupNo, int setNo, int flagNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsLinkageEnemyFlag, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = flagNo };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param flagNo
             */
            public static CDataQuestCommand IsLinkageEnemyFlagOff(uint stageNo, int groupNo, int setNo, int flagNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsLinkageEnemyFlagOff, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = flagNo };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand IsReleaseSecretRoom(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsReleaseSecretRoom, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand TalkNpcChoice(int stageNo, NpcId npcId, int choice, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.TalkNpcChoice, Param01 = stageNo, Param02 = (int) npcId, Param03 = choice, Param04 = param04 };
            }

            public static CDataQuestCommand OmSetTouchRadius(int stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OmSetTouchRadius, Param01 = stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            public static CDataQuestCommand OmReleaseTouchRadius(int stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OmReleaseTouchRadius, Param01 = stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            // Ghidra-discovered check commands (IDs 211–256)

            /** @brief Returns bit 18 of the substory state word at ctx+0x5c+0x20c. */
            public static CDataQuestCommand IsSubstoryStateBit18(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsSubstoryStateBit18, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Side-effect writer: reads bit 17 of ctx+0x5c+0x20c, inverts it, stores to DAT_021c06b8+0x263. No quest params. */
            public static CDataQuestCommand StoreLinkageEnemyFlagGlobal(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.StoreLinkageEnemyFlagGlobal, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Sets byte +0x11 on an NPC object matching npcLookupId, stores storeVal at ctx+0x5c+0x24c, returns bit 18 of ctx+0x5c+0x220. */
            public static CDataQuestCommand NpcPreTalkAndOrderUi(int stageNo, int npcId, int noOrderGroupSerial, int storeVal)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.NpcPreTalkAndOrderUi, Param01 = stageNo, Param02 = npcId, Param03 = noOrderGroupSerial, Param04 = storeVal };
            }

            /** @brief Checks if substory enemy's HP% >= hpRatePercent. */
            public static CDataQuestCommand SubstoryEnemyHpNotLess(int substoryId, int hpRatePercent, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SubstoryEnemyHpNotLess, Param01 = substoryId, Param02 = hpRatePercent, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if substory enemy's HP% < hpRatePercent. Inverse of SubstoryEnemyHpNotLess. */
            public static CDataQuestCommand SubstoryEnemyHpLess(int substoryId, int hpRatePercent, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SubstoryEnemyHpLess, Param01 = substoryId, Param02 = hpRatePercent, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if average HP% across all substory NPCs >= hpRatePercent. */
            public static CDataQuestCommand SubstoryAvgEnemyHpNotLess(int param01, int hpRatePercent, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SubstoryAvgEnemyHpNotLess, Param01 = param01, Param02 = hpRatePercent, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if average HP% across all substory NPCs < hpRatePercent. */
            public static CDataQuestCommand SubstoryAvgEnemyHpLess(int param01, int hpRatePercent, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.SubstoryAvgEnemyHpLess, Param01 = param01, Param02 = hpRatePercent, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if an OM's behavior state enum matches behaviorState. */
            public static CDataQuestCommand IsOmBehaviorState(uint stageNo, int groupNo, int setNo, int behaviorState)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOmBehaviorState, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = behaviorState };
            }

            /** @brief Checks if a specific enemy group has spawned in a monster gathering spot. */
            public static CDataQuestCommand MonsterGatheringSpotState(uint stageNo, int spotId, int spotState, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.MonsterGatheringSpotState, Param01 = (int)stageNo, Param02 = spotId, Param03 = spotState, Param04 = param04 };
            }

            /** @brief Checks if an OM has finished its animation. */
            public static CDataQuestCommand OmEndAnimation(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OmEndAnimation, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /** @brief Variant of OmEndAnimation without a marker. */
            public static CDataQuestCommand OmEndAnimationNoMarker(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.OmEndAnimationNoMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /** @brief Checks if a player has interacted with a quest-spawned OM and its animation has played out completely. */
            public static CDataQuestCommand QuestOmEndAnimation(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestOmEndAnimation, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /** @brief Variant of QuestOmEndAnimation (223) without quest markers. */
            public static CDataQuestCommand QuestOmEndAnimationNoMarker(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestOmEndAnimationNoMarker, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /** @brief Identical function to DieEnemy in all counts; despite that, it is unable to place markers on enemies at all. */
            public static CDataQuestCommand DieEnemyNoMarker(int stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DieEnemyNoMarker, Param01 = stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /** @brief Checks if an NPC interaction with a specific choice/event has completed. */
            public static CDataQuestCommand QuestTalkNpcRadius(uint stageNo, uint groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.QuestTalkNpcRadius, Param01 = (int)stageNo, Param02 = (int)groupNo, Param03 = setNo, Param04 = param04 };
            }

            /** @brief Checks if the OM matching (stageNo, groupNo, setNo) is broken in the current phase. */
            public static CDataQuestCommand IsOmBrokenInCurrentPhase(uint stageNo, int groupNo, int setNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsOmBrokenInCurrentPhase, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /** @brief Places a radius marker on an enemy group; progresses when the player discovers the enemy. setNo=-1 matches any. Must be followed by a kill command. */
            public static CDataQuestCommand IsEnemyFoundRadius(uint stageNo, int groupNo, int setNo = -1, int markerFlag = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEnemyFoundRadius, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = markerFlag };
            }

            /** @brief No-marker, lock-guarded variant of IsEnemyFoundForOrderRadius. markerFlag always 0 internally. setNo=-1 matches any. */
            public static CDataQuestCommand IsEnemyFoundForOrderRadius(uint stageNo, int groupNo, int setNo = -1, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEnemyFoundForOrderRadius, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = param04 };
            }

            /** @brief Checks if player has an achievement from a given category. */
            public static CDataQuestCommand HasAchievement(int categoryNo, int achievementId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.HasAchievement, Param01 = categoryNo, Param02 = achievementId, Param03 = param03, Param04 = param04 };
            }

            /** @brief Returns bit 19 of the substory state word at ctx+0x5c+0x20c. */
            public static CDataQuestCommand IsSubstoryStateBit19(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsSubstoryStateBit19, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks player equipment against a seasonal gear item list. 0 = Summer, 1 = Christmas. */
            public static CDataQuestCommand IsEquipSeasonal(int listNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEquipSeasonal, Param01 = listNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if player has performed a limit break on an equipment. */
            public static CDataQuestCommand DoLimitBreak(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DoLimitBreak, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if the player has performed extreme synthesis on an equipment (notice bit 21). */
            public static CDataQuestCommand DoExtremeSynthesis(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.DoExtremeSynthesis, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if player has obtained a High Orb from any source (notice bit 22). Quantity does not matter. */
            public static CDataQuestCommand GetHighOrb(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.GetHighOrb, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if player has obtained a Dominion Point from any source (notice bit 23). Quantity does not matter. */
            public static CDataQuestCommand GetDominionPoint(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.GetDominionPoint, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Directly jumps to IsReleaseWarpPointAnyone, but cannot place quest markers. */
            public static CDataQuestCommand IsReleaseWarpPointAnyoneNoMarker(int waypointId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsReleaseWarpPointAnyoneNoMarker, Param01 = waypointId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if the substory quest's mission progress is within [minValue, maxValue]. Min/max order does not matter. */
            public static CDataQuestCommand IsSubstoryProgressInRange(int minValue, int maxValue, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsSubstoryProgressInRange, Param01 = minValue, Param02 = maxValue, Param03 = param03, Param04 = param04 };
            }

            /** @brief Kill-group completion check gated on content mode. Marker vs no-marker determined by this+0x82 at runtime. */
            public static CDataQuestCommand IsKilledTargetEnemySetGroup2(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsKilledTargetEnemySetGroup2, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Identical to IsKilledTargetEnemySetGroupMode15; no-marker variant determined at runtime via this+0x82. */
            public static CDataQuestCommand IsKilledTargetEnemySetGroup2NoMarker(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsKilledTargetEnemySetGroup2NoMarker, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if a contents timer (Timer List B) has elapsed past its stored boundary. */
            public static CDataQuestCommand IsContentsTimerBElapsed(int timerNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsContentsTimerBElapsed, Param01 = timerNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if the quest clear count has reached a threshold. */
            public static CDataQuestCommand IsQuestClearCountNotLess(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsQuestClearCountNotLess, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief S3-only: checks if the contents mode elapsed timer >= timeSec. */
            public static CDataQuestCommand IsContentsModeTimerNotLess(int timeSec, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsContentsModeTimerNotLess, Param01 = timeSec, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Fire-once trigger: reads and clears a byte flag at DAT_021af4f4+0xEEA. Returns 1 if flag was set. */
            public static CDataQuestCommand IsTriggerFlagSetAndClear(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsTriggerFlagSetAndClear, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Direct kill-group completion check (no content-mode guard). Checks flagNo against kill-group list entry+0x14. */
            public static CDataQuestCommand IsWildHuntEnemyKilled(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsWildHuntEnemyKilled, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if a player has reached a chain number from a Chain Dungeon (Extreme Mission). */
            public static CDataQuestCommand ChainNotLess(int chainNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.ChainNotLess, Param01 = chainNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if a Timer List A entry's state value equals zero. Content-mode gated. */
            public static CDataQuestCommand IsEndTimer2(int timerNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsEndTimer2, Param01 = timerNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Wild Hunt system: checks if a target enemy in the zone-entry list has been killed. markerFlag is passed to the kill checker. */
            public static CDataQuestCommand IsWildHuntEnemyFound(int flagNo, int param02 = 0, int param03 = 0, int markerFlag = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsWildHuntEnemyFound, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = markerFlag };
            }

            /** @brief Checks if player has accepted and cleared a wild hunt quest. */
            public static CDataQuestCommand IsWildHuntClear(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsWildHuntClear, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Checks if a quest layout's HP-lost% <= hpLostPct (i.e., layout HP >= threshold). */
            public static CDataQuestCommand IsQuestLayoutHpNotGreater(uint stageNo, int groupNo, int setNo, int hpLostPct)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsQuestLayoutHpNotGreater, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = hpLostPct };
            }

            /** @brief Checks if a player has cleared a specific Extreme Mission/Grand Mission/Chain Dungeon (category 9 quests). questId matched against entry+4. */
            public static CDataQuestCommand IsExtremeMissionClear(int questId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestCheckCommand.IsExtremeMissionClear, Param01 = questId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief Used when command is unknown but seen in packet captures.
             */
            public static CDataQuestCommand Unknown(ushort commandId, int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = commandId, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }
        }

        public class ResultCommand
        {
            /**
              * @brief
              * @param stageNo
              * @param lotNo
              */
            public static CDataQuestCommand LotOn(uint stageNo, int lotNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.LotOn, Param01 = (int)stageNo, Param02 = lotNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param lotNo
             */
            public static CDataQuestCommand LotOff(uint stageNo, int lotNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.LotOff, Param01 = (int)stageNo, Param02 = lotNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param itemId
             * @param itemNum
             */
            public static CDataQuestCommand HandItem(int itemId, int itemNum, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.HandItem, Param01 = itemId, Param02 = itemNum, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param announceType
             * @param announceSubtype Some announce commands like accept use this parameter to distinguish between distinguish between "discovered (0)" and "accept (1)" banner.
             */
            public static CDataQuestCommand SetAnnounce(QuestAnnounceType announceType, int announceSubtype = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetAnnounce, Param01 = (int)announceType, Param02 = announceSubtype, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param type
             */
            public static CDataQuestCommand UpdateAnnounce(QuestAnnounceType announceType = QuestAnnounceType.Accept, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.UpdateAnnounce, Param01 = (int)announceType, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand ChangeMessage(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ChangeMessage, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand QstFlagOn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstFlagOn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand MyQstFlagOn(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.MyQstFlagOn, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand GlobalFlagOn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.GlobalFlagOn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param npcId
             * @param msgNo
             */
            public static CDataQuestCommand QstTalkChg(NpcId npcId, int msgNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstTalkChg, Param01 = (int)npcId, Param02 = msgNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param npcId
             */
            public static CDataQuestCommand QstTalkDel(NpcId npcId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstTalkDel, Param01 = (int)npcId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param startPos
             */
            public static CDataQuestCommand StageJump(uint stageNo, int startPos, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.StageJump, Param01 = (int)stageNo, Param02 = startPos, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param eventNo
             * @param jumpStageNo
             * @param jumpStartPosNo
             */
            public static CDataQuestCommand EventExec(uint stageNo, int eventNo, uint jumpStageNo, int jumpStartPosNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.EventExec, Param01 = (int)stageNo, Param02 = eventNo, Param03 = (int)jumpStageNo, Param04 = jumpStartPosNo };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand CallMessage(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.CallMessage, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param x
             * @param y
             * @param z
             */
            public static CDataQuestCommand Prt(uint stageNo, int x, int y, int z)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.Prt, Param01 = (int)stageNo, Param02 = x, Param03 = y, Param04 = z };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand QstLayoutFlagOn(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstLayoutFlagOn, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand QstLayoutFlagOff(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstLayoutFlagOff, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand QstSceFlagOn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstSceFlagOn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param orbNum
             */
            public static CDataQuestCommand QstDogmaOrb(int orbNum, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstDogmaOrb, Param01 = orbNum, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand GotoMainPwanEdit(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.GotoMainPwanEdit, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param npcId
             */
            public static CDataQuestCommand AddFsmNpcList(NpcId npcId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddFsmNpcList, Param01 = (int)npcId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand EndCycle(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.EndCycle, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param sec
             */
            public static CDataQuestCommand AddCycleTimer(int sec, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddCycleTimer, Param01 = sec, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param x
             * @param y
             * @param z
             */
            public static CDataQuestCommand AddMarkerAtItem(uint stageNo, int x, int y, int z)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddMarkerAtItem, Param01 = (int)stageNo, Param02 = x, Param03 = y, Param04 = z };
            }

            /**
             * @brief
             * @param stageNo
             * @param x
             * @param y
             * @param z
             */
            public static CDataQuestCommand AddMarkerAtDest(uint stageNo, int x, int y, int z)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddMarkerAtDest, Param01 = (int)stageNo, Param02 = x, Param03 = y, Param04 = z };
            }

            /**
             * @brief
             * @param tableIndex
             */
            public static CDataQuestCommand AddResultPoint(int tableIndex, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddResultPoint, Param01 = tableIndex, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param itemId
             * @param itemNum
             */
            public static CDataQuestCommand PushImteToPlBag(int itemId, int itemNum, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.PushImteToPlBag, Param01 = itemId, Param02 = itemNum, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param timerNo
             * @param sec
             */
            public static CDataQuestCommand StartTimer(int timerNo, int sec, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.StartTimer, Param01 = timerNo, Param02 = sec, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param randomNo
             * @param minValue
             * @param maxValue
             * @param resultValue
             */
            public static CDataQuestCommand SetRandom(int randomNo, int minValue, int maxValue, int resultValue)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetRandom, Param01 = randomNo, Param02 = minValue, Param03 = maxValue, Param04 = resultValue };
            }

            /**
             * @brief
             * @param randomNo
             */
            public static CDataQuestCommand ResetRandom(int randomNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ResetRandom, Param01 = randomNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param type
             * @param bgmId
             */
            public static CDataQuestCommand BgmRequest(int type, int bgmId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.BgmRequest, Param01 = type, Param02 = bgmId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand BgmStop(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.BgmStop, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param npcId
             * @param waypointNo0
             * @param waypointNo1
             * @param waypointNo2
             */
            public static CDataQuestCommand SetWaypoint(NpcId npcId, int waypointNo0, int waypointNo1, int waypointNo2)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetWaypoint, Param01 = (int)npcId, Param02 = waypointNo0, Param03 = waypointNo1, Param04 = waypointNo2 };
            }

            /**
             * @brief
             * @param npcId
             * @param groupSerial
             */
            public static CDataQuestCommand ForceTalkQuest(NpcId npcId, int groupSerial, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ForceTalkQuest, Param01 = (int)npcId, Param02 = groupSerial, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param guideNo
             */
            public static CDataQuestCommand TutorialDialog(TutorialId guideNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.TutorialDialog, Param01 = (int) guideNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param keyItemIdx
             * @param pointNum
             */
            public static CDataQuestCommand AddKeyItemPoint(int keyItemIdx, int pointNum, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddKeyItemPoint, Param01 = keyItemIdx, Param02 = pointNum, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand DontSaveProcess(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.DontSaveProcess, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand InterruptCycleContents(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.InterruptCycleContents, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param point
             */
            public static CDataQuestCommand QuestEvaluationPoint(int point, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QuestEvaluationPoint, Param01 = point, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand CheckOrderCondition(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.CheckOrderCondition, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             * @param questId
             */
            public static CDataQuestCommand WorldManageLayoutFlagOn(int flagNo, int questId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.WorldManageLayoutFlagOn, Param01 = flagNo, Param02 = questId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             * @param questId
             */
            public static CDataQuestCommand WorldManageLayoutFlagOff(int flagNo, int questId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.WorldManageLayoutFlagOff, Param01 = flagNo, Param02 = questId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand PlayEndingForFirstSeason(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.PlayEndingForFirstSeason, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param announceNo
             * @param type
             */
            public static CDataQuestCommand AddCyclePurpose(int announceNo, int type, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddCyclePurpose, Param01 = announceNo, Param02 = type, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param announceNo
             */
            public static CDataQuestCommand RemoveCyclePurpose(int announceNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.RemoveCyclePurpose, Param01 = announceNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param announceNo
             * @param type
             */
            public static CDataQuestCommand UpdateAnnounceDirect(int announceNo, int type, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.UpdateAnnounceDirect, Param01 = announceNo, Param02 = type, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand SetCheckPoint(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetCheckPoint, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param processNo
             */
            public static CDataQuestCommand ReturnCheckPoint(int processNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ReturnCheckPoint, Param01 = processNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param type
             * @param msgNo
             */
            public static CDataQuestCommand CallGeneralAnnounce(int type, int msgNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.CallGeneralAnnounce, Param01 = type, Param02 = msgNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand TutorialEnemyInvincibleOff(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.TutorialEnemyInvincibleOff, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param startPos
             * @param outSceNo
             */
            public static CDataQuestCommand SetDiePlayerReturnPos(uint stageNo, int startPos, int outSceNo, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetDiePlayerReturnPos, Param01 = (int)stageNo, Param02 = startPos, Param03 = outSceNo, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             * @param questId
             */
            public static CDataQuestCommand WorldManageQuestFlagOn(int flagNo, int questId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.WorldManageQuestFlagOn, Param01 = flagNo, Param02 = questId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             * @param questId
             */
            public static CDataQuestCommand WorldManageQuestFlagOff(int flagNo, int questId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.WorldManageQuestFlagOff, Param01 = flagNo, Param02 = questId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param processNo
             */
            public static CDataQuestCommand ReturnCheckPointEx(int processNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ReturnCheckPointEx, Param01 = processNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand ResetCheckPoint(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ResetCheckPoint, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param startPos
             */
            public static CDataQuestCommand ResetDiePlayerReturnPos(uint stageNo, int startPos, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ResetDiePlayerReturnPos, Param01 = (int)stageNo, Param02 = startPos, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand SetBarricade(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetBarricade, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand ResetBarricade(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ResetBarricade, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand TutorialEnemyInvincibleOn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.TutorialEnemyInvincibleOn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand ResetTutorialFlag(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ResetTutorialFlag, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand StartContentsTimer(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.StartContentsTimer, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param flagNo
             */
            public static CDataQuestCommand MyQstFlagOff(int flagNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.MyQstFlagOff, Param01 = flagNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param eventNo
             */
            public static CDataQuestCommand PlayCameraEvent(uint stageNo, int eventNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.PlayCameraEvent, Param01 = (int)stageNo, Param02 = eventNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand EndEndQuest(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.EndEndQuest, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand ReturnAnnounce(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ReturnAnnounce, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param announceNo
             * @param type
             */
            public static CDataQuestCommand AddEndContentsPurpose(int announceNo, int type, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddEndContentsPurpose, Param01 = announceNo, Param02 = type, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param announceNo
             */
            public static CDataQuestCommand RemoveEndContentsPurpose(int announceNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.RemoveEndContentsPurpose, Param01 = announceNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand StopCycleTimer(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.StopCycleTimer, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand RestartCycleTimer(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.RestartCycleTimer, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param AreaId
             * @param AddPoint
             */
            public static CDataQuestCommand AddAreaPoint(int AreaId, int AddPoint, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddAreaPoint, Param01 = AreaId, Param02 = AddPoint, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param FlanNo1
             * @param FlanNo2
             * @param FlanNo3
             * @param ResultNo
             */
            public static CDataQuestCommand LayoutFlagRandomOn(int FlanNo1, int FlanNo2, int FlanNo3, int ResultNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.LayoutFlagRandomOn, Param01 = FlanNo1, Param02 = FlanNo2, Param03 = FlanNo3, Param04 = ResultNo };
            }

            /**
             * @brief
             * @param stageNo
             * @param npcId
             * @param groupSerial
             */
            public static CDataQuestCommand SetDeliverInfo(uint stageNo, NpcId npcId, int groupSerial, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetDeliverInfo, Param01 = (int)stageNo, Param02 = (int)npcId, Param03 = groupSerial, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param groupSerial
             */
            public static CDataQuestCommand SetDeliverInfoQuest(uint stageNo, int groupNo, int setNo, int groupSerial)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetDeliverInfoQuest, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = groupSerial };
            }

            /**
             * @brief
             * @param type
             * @param bgmId
             */
            public static CDataQuestCommand BgmRequestFix(int type, int bgmId, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.BgmRequestFix, Param01 = type, Param02 = bgmId, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param eventNo
             * @param jumpStageNo
             * @param jumpStartPosNo
             */
            public static CDataQuestCommand EventExecCont(uint stageNo, int eventNo, uint jumpStageNo, int jumpStartPosNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.EventExecCont, Param01 = (int)stageNo, Param02 = eventNo, Param03 = (int)jumpStageNo, Param04 = jumpStartPosNo };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand PlPadOff(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.PlPadOff, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand PlPadOn(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.PlPadOn, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand EnableGetSetQuestList(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.EnableGetSetQuestList, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand StartMissionAnnounce(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.StartMissionAnnounce, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param type
             * @param num
             */
            public static CDataQuestCommand StageAnnounce(int type, int num, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.StageAnnounce, Param01 = type, Param02 = num, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param id
             */
            public static CDataQuestCommand ReleaseAnnounce(ContentsRelease releaseId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ReleaseAnnounce, Param01 = (int) releaseId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param buttonGuideNo
             */
            public static CDataQuestCommand ButtonGuideFlagOn(int buttonGuideNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ButtonGuideFlagOn, Param01 = buttonGuideNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param buttonGuideNo
             */
            public static CDataQuestCommand ButtonGuideFlagOff(int buttonGuideNo, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ButtonGuideFlagOff, Param01 = buttonGuideNo, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand AreaJumpFadeContinue(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AreaJumpFadeContinue, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param eventNo
             * @param startPos
             */
            public static CDataQuestCommand ExeEventAfterStageJump(uint stageNo, int eventNo, int startPos, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ExeEventAfterStageJump, Param01 = (int)stageNo, Param02 = eventNo, Param03 = startPos, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param eventNo
             * @param startPos
             */
            public static CDataQuestCommand ExeEventAfterStageJumpContinue(uint stageNo, int eventNo, int startPos, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ExeEventAfterStageJumpContinue, Param01 = (int)stageNo, Param02 = eventNo, Param03 = startPos, Param04 = param04 };
            }

            /**
             * @brief
             * @param groupNo
             * @param waitTime
             */
            public static CDataQuestCommand PlayMessage(int groupNo, int waitTime, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.PlayMessage, Param01 = groupNo, Param02 = waitTime, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand StopMessage(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.StopMessage, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param startPosNo
             */
            public static CDataQuestCommand DecideDivideArea(uint stageNo, int startPosNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.DecideDivideArea, Param01 = (int)stageNo, Param02 = startPosNo, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param phaseId
             */
            public static CDataQuestCommand ShiftPhase(int phaseId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ShiftPhase, Param01 = phaseId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand ReleaseMyRoom(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ReleaseMyRoom, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand DivideSuccess(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.DivideSuccess, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand DivideFailed(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.DivideFailed, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param rewardRank
             */
            public static CDataQuestCommand SetProgressBonus(int rewardRank, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetProgressBonus, Param01 = rewardRank, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             */
            public static CDataQuestCommand RefreshOmKeyDisp(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.RefreshOmKeyDisp, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param type
             */
            public static CDataQuestCommand SwitchPawnQuestTalk(int type, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SwitchPawnQuestTalk, Param01 = type, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param flagId
             */
            public static CDataQuestCommand LinkageEnemyFlagOn(uint stageNo, int groupNo, int setNo, int flagId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.LinkageEnemyFlagOn, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = flagId };
            }

            /**
             * @brief
             * @param stageNo
             * @param groupNo
             * @param setNo
             * @param flagId
             */
            public static CDataQuestCommand LinkageEnemyFlagOff(uint stageNo, int groupNo, int setNo, int flagId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.LinkageEnemyFlagOff, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = flagId };
            }

            // Ghidra-discovered result commands (IDs 99–134)

            /** @brief Adds a signed delta to the current substory progress value. Baked context, no subid params. */
            public static CDataQuestCommand SubstoryProgress(int delta, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SubstoryProgress, Param01 = delta, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Finds a substory entry by substoryId and adds progressDelta to its progress, clamped to [0,100]. */
            public static CDataQuestCommand AddSubstoryProgress(int substoryId, int progressDelta, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddSubstoryProgress, Param01 = substoryId, Param02 = progressDelta, Param03 = param03, Param04 = param04 };
            }

            /** @brief Triggers a substory event sequence. Checks mode; if mode==0xb fires substory FSM transition. */
            public static CDataQuestCommand TriggerSubstoryEvent(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.TriggerSubstoryEvent, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Triggers display of the substory UI element. No command params used; value read from baked quest context. */
            public static CDataQuestCommand EnableSubstoryUIElement(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.EnableSubstoryUIElement, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Disables the substory UI element (+0x44 reference cleared). */
            public static CDataQuestCommand DisableSubstoryUIElement(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.DisableSubstoryUIElement, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Redirects NPC talk for a substory context via FUN_009ce930(param01, param02). */
            public static CDataQuestCommand QstTalkChgFsm(NpcId npcId, int msgNo = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.QstTalkChgFsm, Param01 = (int) npcId, Param02 = msgNo, Param03 = param03, Param04 = param04 };
            }

            /** @brief Sets invincibility on a substory enemy group. param02=1 sets invincible. */
            public static CDataQuestCommand SetSubstoryEnemyInvincible(int enemyGroupFlag, int invincible, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetSubstoryEnemyInvincible, Param01 = enemyGroupFlag, Param02 = invincible, Param03 = param03, Param04 = param04 };
            }

            /** @brief Adds an NPC to the FSM talk NPC list. Validates FSM mode first. */
            public static CDataQuestCommand AddFsmTalkNpc(int npcId, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AddFsmTalkNpc, Param01 = npcId, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Displays an achievement banner from a given category. Only category 6 (Great Purpose) has banners to display. */
            public static CDataQuestCommand AchievementBanner(int categoryNo, int bannerNo, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.AchievementBanner, Param01 = categoryNo, Param02 = bannerNo, Param03 = param03, Param04 = param04 };
            }

            /** @brief Enables substory element variant B. Sets +0x4c reference via FUN_00598860. */
            public static CDataQuestCommand EnableSubstoryElementB(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.EnableSubstoryElementB, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Disables substory element variant B. Clears +0x4c reference via FUN_005986A0. */
            public static CDataQuestCommand DisableSubstoryElementB(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.DisableSubstoryElementB, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Overrides lighting and clouds on current map. Effects are purely visual and not persistent. */
            public static CDataQuestCommand SetEnvironmentalEffect(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetEnvironmentalEffect, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Resets environmental effects set by SetEnvironmentalEffect. */
            public static CDataQuestCommand ResetEnvironmentalEffect(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.ResetEnvironmentalEffect, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Schedules an FSM NPC behavior by calling FUN_009d1a60(scheduleId). scheduleId = param04. */
            public static CDataQuestCommand SetFsmNpcSchedule(int param01 = 0, int param02 = 0, int param03 = 0, int scheduleId = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetFsmNpcSchedule, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = scheduleId };
            }

            /** @brief Sets the level of a quest enemy group (type 3) via FUN_00bc0670. Phase-gated. */
            public static CDataQuestCommand SetQuestEnemyLevel(uint stageNo, int groupNo, int setNo, int level)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetQuestEnemyLevel, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = level };
            }

            /** @brief Area-aware variant of SetQuestEnemyLevel using FUN_00a41890. */
            public static CDataQuestCommand SetQuestEnemyLevelEx(uint stageNo, int groupNo, int setNo, int level)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetQuestEnemyLevelEx, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = level };
            }

            /** @brief Sets the danger tier (bits 23-21) of a quest enemy group via FUN_00bc0720. Phase-gated. */
            public static CDataQuestCommand SetQuestEnemyTierUp(uint stageNo, int groupNo, int setNo, int tier)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetQuestEnemyTierUp, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = tier };
            }

            /** @brief Area-aware variant of SetQuestEnemyTierUp. */
            public static CDataQuestCommand SetQuestEnemyTierUpEx(uint stageNo, int groupNo, int setNo, int tier)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetQuestEnemyTierUpEx, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = tier };
            }

            /** @brief Sets a body/stance pose (1-6) on a quest NPC/enemy via FUN_00bbf670. */
            public static CDataQuestCommand SetQuestOmMontageFix(uint stageNo, int groupNo, int setNo, int montagueNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetQuestOmMontageFix, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = montagueNo };
            }

            /** @brief Area-aware variant of AddResultCmdSetQuestOmMontageFix. */
            public static CDataQuestCommand SetQuestOmMontageFixEx(uint stageNo, int groupNo, int setNo, int montagueNo)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetQuestOmMontageFixEx, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = montagueNo };
            }

            /** @brief Sets the level of a layout enemy (type 2) by queuing it into a critical-section-guarded buffer. */
            public static CDataQuestCommand SetQuestLayoutEnemyLevel(uint stageNo, int groupNo, int setNo, int level)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetQuestLayoutEnemyLevel, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = level };
            }

            /** @brief Removes an FSM NPC entry from the process list via FUN_0063dda0(param01). */
            public static CDataQuestCommand RemoveFsmNpcFromSchedule(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.RemoveFsmNpcFromSchedule, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Controls enemy expedition state. mode=2: starts; mode=3: iterates party members and fires expedition signal. */
            public static CDataQuestCommand SetEnemyExpeditionState(int mode, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetEnemyExpeditionState, Param01 = mode, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Fires a substory ending sequence: calls FUN_00be9960, FUN_00b85670, and sends messages 0x25f/0x260. */
            public static CDataQuestCommand TriggerSubstoryEndSequence(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.TriggerSubstoryEndSequence, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Sends packet 63.5.16 (C2S_CHAIN_DUNGEON_END_CHAIN_NTC) kickstarting the rewards phase of chain dungeons. */
            public static CDataQuestCommand EndChain(int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.EndChain, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Controls pawn expedition. mode=1: starts; mode=2: stops. */
            public static CDataQuestCommand SetPawnExpeditionFlag(int mode, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetPawnExpeditionFlag, Param01 = mode, Param02 = param02, Param03 = param03, Param04 = param04 };
            }

            /** @brief Sets a body/pose mode on a layout enemy (type 2) via FUN_005be380(poseId). */
            public static CDataQuestCommand SetQuestLayoutEnemyBodyPose(uint stageNo, int groupNo, int setNo, int poseId)
            {
                return new CDataQuestCommand() { Command = (ushort)QuestResultCommand.SetQuestLayoutEnemyBodyPose, Param01 = (int)stageNo, Param02 = groupNo, Param03 = setNo, Param04 = poseId };
            }

            /**
             * @brief Used to send command values with unknown names
             */
            public static CDataQuestCommand Unknown(ushort commandId, int param01 = 0, int param02 = 0, int param03 = 0, int param04 = 0)
            {
                return new CDataQuestCommand() { Command = commandId, Param01 = param01, Param02 = param02, Param03 = param03, Param04 = param04 };
            }
        }

        public class NotifyCommand
        {
            /**
             * @brief Generic function used to create CDataQuestProjectWork objects.
             */
            public static CDataQuestProgressWork QuestProgressWork(uint commandNo, int work01 = 0, int work02 = 0, int work03 = 0, int work04 = 0)
            {
                return new CDataQuestProgressWork() { CommandNo = commandNo, Work01 = work01, Work02 = work02, Work03 = work03, Work04 = work04 };
            }

            /**
             * @brief Used to let the client know a World Quest (set quest) has been completed.
             */
            public static CDataQuestProgressWork WorldQuestClearNum(QuestAreaId areaId, uint amount, int work03 = 0, int work04 = 0)
            {
                return QuestProgressWork((int) QuestNotifyCommand.SetQuestClearNum, (int) amount, (int) areaId, work03, work04);
            }

            /**
             * @brief
             * @param flagNo
             * @param stageNo
             * @param groupNo
             */
            public static CDataQuestProgressWork KilledTargetEnemySetGroup(int flagNo, uint stageNo, int groupNo, int work04 = 0)
            {
                return new CDataQuestProgressWork() { CommandNo = (uint)QuestNotifyCommand.KilledTargetEnemySetGroup, Work01 = flagNo, Work02 = (int)stageNo, Work03 = groupNo, Work04 = work04 };
            }

            /**
             * @brief
             * @param flagNo
             * @param stageNo
             * @param groupNo
             */
            public static CDataQuestProgressWork KilledTargetEmSetGrpNoMarker(int flagNo, uint stageNo, int groupNo, int work04 = 0)
            {
                return new CDataQuestProgressWork() { CommandNo = (uint)QuestNotifyCommand.KilledTargetEmSetGrpNoMarker, Work01 = flagNo, Work02 = (int)stageNo, Work03 = groupNo, Work04 = work04 };
            }

            /**
             * @brief
             * @param npcId
             */
            public static CDataQuestProgressWork KilledTargetEnemySetGroup1(NpcId npcId, int work02 = 0, int work03 = 0, int work04 = 0)
            {
                return new CDataQuestProgressWork() { CommandNo = (uint)QuestNotifyCommand.FulfillDeliverItem, Work01 = (int)npcId, Work02 = work02, Work03 = work03, Work04 = work04 };
            }
        }

        public static bool IsBoardQuest(QuestId questId)
        {
            return QuestUtils.IsBoardQuest(questId);
        }

        public static bool IsBoardQuest(Quest quest)
        {
            return IsBoardQuest(quest.QuestId);
        }

        public static bool IsBoardQuest(uint questScheduleId)
        {
            return QuestScheduleId.GetType(questScheduleId) == QuestScheduleId.ScheduleIdType.Board;
        }

        public static bool IsTutorialQuest(QuestId questId)
        {
            return QuestUtils.IsTutorialQuest(questId);
        }

        public static bool IsTutorialQuest(Quest quest)
        {
            return IsTutorialQuest(quest.QuestId);
        }

        public static bool IsWorldQuest(QuestId questId)
        {
            return QuestUtils.IsWorldQuest(questId);
        }

        public static bool IsWorldQuest(Quest quest)
        {
            return IsWorldQuest(quest.QuestId);
        }

        public static bool IsClanQuest(QuestId questId)
        {
            return QuestUtils.IsClanQuest(questId);
        }

        public static bool IsClanQuest(Quest quest)
        {
            return IsClanQuest(quest.QuestId);
        }

        public static bool IsExmQuest(QuestId questId)
        {
            return QuestUtils.IsExmQuest(questId);
        }

        public static bool IsExmQuest(uint questScheduleId)
        {
            var quest = GetQuestByScheduleId(questScheduleId);
            if (quest == null)
            {
                return false;
            }
            return IsExmQuest(quest.QuestId);
        }

        private static Dictionary<QuestAreaId, ContentsRelease> WorldQuestRequiredUnlocks = new Dictionary<QuestAreaId, ContentsRelease>()
        {
            // S2
            [QuestAreaId.BloodbaneIsle] = ContentsRelease.BloodbaneIsleWorldQuests,
            [QuestAreaId.ElanWaterGrove] = ContentsRelease.ElanWaterGroveWorldQuests,
            [QuestAreaId.FaranaPlains] = ContentsRelease.FaranaPlainsWorldQuests,
            [QuestAreaId.MorrowForest] = ContentsRelease.MorrowForestWorldQuests,
            [QuestAreaId.KingalCanyon] = ContentsRelease.KingalCanyonWorldQuests,
            // S3
            [QuestAreaId.RathniteFoothills] = ContentsRelease.RathniteFoothillsWorldQuests,
            [QuestAreaId.FeryanaWilderness] = ContentsRelease.FeryanaWildernessWorldQuests,
            [QuestAreaId.MegadosysPlateau] = ContentsRelease.MegadosysPlateauWorldQuests,
            [QuestAreaId.UrtecaMountains] = ContentsRelease.UrtecaMountainsWorldQuests,
        };

        public static bool HasWorldQuestAreaReleased(Character character, QuestAreaId questAreaId)
        {
            if (!character.HasContentReleased(ContentsRelease.WorldQuests))
            {
                return false;
            }

            if (!WorldQuestRequiredUnlocks.ContainsKey(questAreaId))
            {
                return true;
            }

            var releaseId = WorldQuestRequiredUnlocks[questAreaId];
            return character.HasContentReleased(releaseId);
        }
    }
}
