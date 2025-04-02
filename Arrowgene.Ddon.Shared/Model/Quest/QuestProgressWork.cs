using Arrowgene.Ddon.Shared.Entity.PacketStructure;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public abstract class QuestProgressWork
    {
        public readonly uint QuestScheduleId;
        public readonly QuestProgressWorkType WorkType;
        public readonly QuestProcessState ProcessState;

        public QuestProgressWork(uint questScheduleId, QuestProcessState processState, QuestProgressWorkType workType)
        {
            WorkType = workType;
            ProcessState = processState;
            QuestScheduleId = questScheduleId;
        }

        public abstract S2CQuestQuestProgressWorkSaveNtc GetWork();
    }
}
