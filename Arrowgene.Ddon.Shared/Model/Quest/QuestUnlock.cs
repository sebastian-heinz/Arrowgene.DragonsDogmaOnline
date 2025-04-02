namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestUnlock
    {
        public ContentsRelease ReleaseId;
        public TutorialId TutorialId;
        public QuestFlagInfo FlagInfo;

        public QuestUnlock()
        {
            ReleaseId = ContentsRelease.None;
            TutorialId = TutorialId.None;
            FlagInfo = null;
        }
    }
}
