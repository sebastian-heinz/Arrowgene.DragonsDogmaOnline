using Arrowgene.Ddon.Shared.Model.Quest;

namespace Arrowgene.Ddon.Shared
{
    public class QuestUtils
    {
        public static QuestAdventureGuideCategory DetermineQuestAdventureCategory(QuestId questId, QuestType questType)
        {
            QuestAdventureGuideCategory result = QuestAdventureGuideCategory.None;
            switch (questType)
            {
                case QuestType.Main:
                    result = QuestAdventureGuideCategory.MainQuest;
                    break;
                case QuestType.WildHunt:
                    result = QuestAdventureGuideCategory.WildHunt;
                    break;
                default:
                    result = ((questType == QuestType.World) || QuestUtils.IsBoardQuest(questId)) ?
                        QuestAdventureGuideCategory.LevelingQuest :
                        QuestAdventureGuideCategory.None;
                    break;
            }

            return result;
        }

        public static bool DetermineIfQuestIsImportant(QuestAdventureGuideCategory adventureGuideCategory)
        {
            bool isImportant = false;
            switch (adventureGuideCategory)
            {
                case QuestAdventureGuideCategory.MainQuest:
                case QuestAdventureGuideCategory.AreaTrialOrMission:
                case QuestAdventureGuideCategory.CollaborationOrSeasonalQuest:
                case QuestAdventureGuideCategory.VocationQuest:
                case QuestAdventureGuideCategory.QuestUsefulForAdventure:
                    isImportant = true;
                    break;
                default:
                    // Wild Hunt should be important if S rank otherwise not important
                    isImportant = false;
                    break;
            }
            return isImportant;
        }

        public static bool IsBoardQuest(QuestId questId)
        {
            return (((uint)questId) >= 40000000) && (((uint)questId) < 50000000);
        }

        public static bool IsTutorialQuest(QuestId questId)
        {
            return (((uint)questId) >= 60000000) && (((uint)questId) < 70000000);
        }

        public static bool IsWorldQuest(QuestId questId)
        {
            return (((uint)questId) >= 20000000) && (((uint)questId) < 30000000);
        }

        public static bool IsClanQuest(QuestId questId)
        {
            return (((uint)questId) >= 30000000) && (((uint)questId) < 40000000);
        }

        public static bool IsExmQuest(QuestId questId)
        {
            return (((uint)questId) >= 50000000) && (((uint)questId) < 60000000);
        }
    }
}
