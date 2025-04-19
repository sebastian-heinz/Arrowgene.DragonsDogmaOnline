namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class CompletedQuest
    {
        public QuestType QuestType { get; set; }
        public QuestId QuestId { get; set; }
        public uint ClearCount { get; set; }
    }
}
