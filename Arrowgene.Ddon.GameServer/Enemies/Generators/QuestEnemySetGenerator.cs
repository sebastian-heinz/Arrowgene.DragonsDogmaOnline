using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Quests;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Enemies.Generators
{
    public class QuestEnemySetGenerator : IEnemySetGenerator
    {
        public bool GetEnemySet(DdonGameServer server, GameClient client, StageLayoutId stageLayoutId, byte subGroupId, List<InstancedEnemy> instancedEnemySet, out QuestId questId)
        {
            questId = QuestId.None;

            Quest quest = FindQuestBasedOnPriority(client, stageLayoutId, subGroupId);
            if (quest == null)
            {
                return false;
            }

            questId = quest.QuestId;

            var questStateManager = QuestManager.GetQuestStateManager(client, quest);
            instancedEnemySet.AddRange(questStateManager.GetInstancedEnemies(quest, stageLayoutId, subGroupId));

            return true;
        }

        private Quest FindQuestBasedOnPriority(GameClient client, StageLayoutId stageLayoutId, uint subgroupId)
        {

            var quests = new List<Quest>();
            foreach (var questScheduleId in QuestManager.CollectQuestScheduleIds(client, stageLayoutId))
            {
                var quest = QuestManager.GetQuestByScheduleId(questScheduleId);
                var questStateManager = QuestManager.GetQuestStateManager(client, quest);
                if (quest.OverrideEnemySpawn && quest.HasEnemiesInCurrentStageGroup(stageLayoutId))
                {
                    quests.Add(quest);
                }
                else if (!quest.OverrideEnemySpawn && questStateManager.HasEnemiesForCurrentQuestStepInStageGroup(quest, stageLayoutId, subgroupId))
                {
                    quests.Add(quest);
                }
            }

            // There may be multiple quests conflicting for a StageId.LayerNo.GroupNo.
            // Certain quests should have a higher priority than other quests
            // and this list describes the ranking of the different quest types.
            var questPriorityList = new List<QuestType>()
            {
                QuestType.Main,
                QuestType.Tutorial,
                QuestType.WildHunt,
                QuestType.World,
            };

            Quest priorityQuest = null;
            foreach (var questType in questPriorityList)
            {
                var matches = quests.Where(x => x.QuestType == questType).ToList();
                if (matches.Count > 0)
                {
                    priorityQuest = matches[0];
                    break;
                }
            }

            if (priorityQuest == null && quests.Count > 0)
            {
                priorityQuest = quests[0];
            }

            return priorityQuest;
        }
    }
}
