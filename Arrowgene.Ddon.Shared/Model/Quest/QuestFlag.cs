namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestFlagType
    {
        None,
        QstLayout,
        MyQst,
        WorldManageLayout,
        WorldManageQuest,
    }

    public enum QuestFlagAction
    {
        None,
        CheckOn,
        CheckOff,
        Set,
        Clear,
        CheckSetFromFsm
    }

    public class QuestFlag
    {
        public QuestFlagType Type {  get; set; }
        public QuestFlagAction Action { get; set; }
        public int Value { get; set; }
        public int QuestId { get; set; }
    }
}
