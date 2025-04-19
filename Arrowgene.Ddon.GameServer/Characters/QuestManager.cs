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
        private static Dictionary<uint, Quest> gQuests = new Dictionary<uint, Quest>();
        private static readonly Dictionary<QuestId, HashSet<uint>> gVariantQuests = new();

        private static Dictionary<uint, HashSet<uint>> gTutorialQuests = new Dictionary<uint, HashSet<uint>>();
        private static Dictionary<QuestAreaId, HashSet<QuestId>> gWorldQuests = new Dictionary<QuestAreaId, HashSet<QuestId>>();
        private static Dictionary<QuestAdventureGuideCategory, HashSet<uint>> gAdventureGuideCategories = new Dictionary<QuestAdventureGuideCategory, HashSet<uint>>();
        private static Dictionary<QuestAreaId, Dictionary<uint,uint>> gAreaTrialRanks = new Dictionary<QuestAreaId, Dictionary<uint, uint>>();

        /// <summary>
        /// QuestScheduleIds that are requested as part of World Manage Quests from pcaps.
        /// We know they can't be found, so don't audibly complain about them.
        /// TODO: Remove this when those quests are handled properly.
        /// </summary>
        private static readonly HashSet<uint> KnownBadQuestScheduleIds = new HashSet<uint>()
        {
            25077, 43645, 43646, 47734, 47736, 47737, 47738, 47739, 49692, 77644, 151381, 208640, 233576, 259411, 259412, 287378, 315624
        };

        private static void AddQuestToCategory(Quest quest)
        {
            if (!gVariantQuests.ContainsKey(quest.QuestId))
            {
                gVariantQuests[quest.QuestId] = new HashSet<uint>();
            }
            gVariantQuests[quest.QuestId].Add(quest.QuestScheduleId);

            if (quest.QuestType == QuestType.Tutorial)
            {
                uint stageNo = (uint)StageManager.ConvertIdToStageNo(quest.StageId);
                if (!gTutorialQuests.ContainsKey(stageNo))
                {
                    gTutorialQuests[stageNo] = new HashSet<uint>();
                }
                gTutorialQuests[stageNo].Add(quest.QuestScheduleId);
            }
            else if (quest.QuestType == QuestType.World)
            {
                if (!gWorldQuests.ContainsKey(quest.QuestAreaId))
                {
                    gWorldQuests[quest.QuestAreaId] = new HashSet<QuestId>();
                }
                gWorldQuests[quest.QuestAreaId].Add(quest.QuestId);
            }

            if (!gAdventureGuideCategories.ContainsKey(quest.AdventureGuideCategory))
            {
                gAdventureGuideCategories[quest.AdventureGuideCategory] = new HashSet<uint>();
            }
            gAdventureGuideCategories[quest.AdventureGuideCategory].Add(quest.QuestScheduleId);

            // Build a ranking list for quests for area trials
            // TODO: This should probably be done in quest scripts, but who wants to rewrite 70+ quests?
            if (quest.AdventureGuideCategory == QuestAdventureGuideCategory.AreaTrialOrMission)
            {
                if (!gAreaTrialRanks.ContainsKey(quest.QuestAreaId))
                {
                    gAreaTrialRanks[quest.QuestAreaId] = new Dictionary<uint, uint>();
                }

                // Nightmarish
                var requiredRank = quest.ToCDataQuestList(0).QuestProcessStateList.First().CheckCommandList.First().ResultCommandList.First().Param02;
                gAreaTrialRanks[quest.QuestAreaId][quest.QuestScheduleId] = (uint) requiredRank;
            }
        }

        public static void LoadScriptedQuest(DdonGameServer server, IQuest questScript)
        {
            gQuests[questScript.QuestScheduleId] = questScript.GenerateQuest(server);
            var quest = gQuests[questScript.QuestScheduleId];
            if (quest.Enabled)
            {
                AddQuestToCategory(quest);
            }
        }

        public static void LoadQuests(DdonGameServer server)
        {
            var assetRepository = server.AssetRepository;

            // Iterate over quests generated from json
            foreach (var questAsset in assetRepository.QuestAssets.Quests)
            {
                gQuests[questAsset.QuestScheduleId] = GenericQuest.FromAsset(server, questAsset);

                var quest = gQuests[questAsset.QuestScheduleId];
                if (quest.Enabled)
                {
                    AddQuestToCategory(quest);
                }
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
            if (!gWorldQuests.ContainsKey(areaId))
            {
                return new HashSet<QuestId>();
            }

            return gWorldQuests[areaId];
        }

        public static Quest GetQuestByBoardId(ulong boardId)
        {
            uint questId = BoardManager.GetQuestIdFromBoardId(boardId);
            return GetQuestByScheduleId(questId);
        }

        public static HashSet<uint> GetTutorialQuestsByStageNo(uint stageNo)
        {
            if (!gTutorialQuests.ContainsKey(stageNo))
            {
                return new HashSet<uint>();
            }

            return gTutorialQuests[stageNo];
        }

        public static bool IsVariantQuest(QuestId baseQuestId)
        {
            return gVariantQuests.ContainsKey(baseQuestId);
        }

        public static Quest GetQuestByScheduleId(uint questScheduleId)
        {
            if (!gQuests.ContainsKey(questScheduleId))
            {
                if (!KnownBadQuestScheduleIds.Contains(questScheduleId))
                {
                    Logger.Error($"GetQuestByScheduleId: Invalid questScheduleId {questScheduleId}");
                }

                return null;
            }

            return gQuests[questScheduleId];
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

        public static HashSet<uint> GetQuestScheduleIdsForQuestId(QuestId questId)
        {
            if (gVariantQuests.ContainsKey(questId))
            {
                return gVariantQuests[questId];
            }
            return new HashSet<uint>();
        }

        public static Quest RollQuestForQuestId(QuestId questId)
        {
            var quests = GetQuestScheduleIdsForQuestId(questId);
            var questScheduleId = quests.ElementAt(Random.Shared.Next(0, quests.Count));
            return gQuests[questScheduleId];
        }

        public static HashSet<uint> GetQuestsByAdventureGuideCategory(QuestAdventureGuideCategory category)
        {
            if (!gAdventureGuideCategories.ContainsKey(category))
            {
                return new();
            }
            return gAdventureGuideCategories[category].ToHashSet();
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
                .Where(x => QuestManager.GetQuestByScheduleId(x).QuestType == QuestType.Tutorial)
                .Where(x => QuestManager.GetQuestStateManager(client, x).GetQuestState(x).State == QuestProgressState.Unknown)
                .Select(x => QuestManager.GetQuestByScheduleId(x))
                .ToList();

            foreach (var quest in unstartedTutorialQuests)
            {
                if (quest == null)
                {
                    continue;
                }

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
