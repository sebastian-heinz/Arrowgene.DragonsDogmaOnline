namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestSeverActionType
    {
        None,
        OmSetInstantValue
    }

    public class QuestServerAction
    {
        public QuestSeverActionType ActionType { get; set; }
        public StageLayoutId StageLayoutId { get; set; }
        public ulong Key { get; set; }
        public uint Value { get; set; }
        public OmInstantValueAction OmInstantValueAction { get; set; }
    }
}
