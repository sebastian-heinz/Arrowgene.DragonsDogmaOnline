namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestFlagInfo
    {
        public readonly uint Value;
        public readonly QuestFlagType FlagType;
        public readonly QuestId QuestId;
        public readonly StageInfo StageInfo;

        public QuestFlagInfo(uint value, QuestFlagType flagType, QuestId questId, StageInfo stageInfo)
        {
            Value = value;
            FlagType = flagType;
            QuestId = questId;
            StageInfo = stageInfo;
        }

        public static QuestFlagInfo WorldManageLayoutFlag(uint value, QuestId questId, StageInfo stageInfo)
        {
            return new QuestFlagInfo(value, QuestFlagType.WorldManageLayout, questId, stageInfo);
        }

        public static QuestFlagInfo WorldManageQuestFlag(uint value, QuestId questId)
        {
            return new QuestFlagInfo(value, QuestFlagType.WorldManageQuest, questId, null);
        }
    }
}
