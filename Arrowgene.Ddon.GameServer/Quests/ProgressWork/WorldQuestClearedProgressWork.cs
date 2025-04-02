using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Quest;
using System;
using System.Collections.Generic;

namespace Arrowgene.Ddon.GameServer.Quests.Work
{
    public class WorldQuestClearedProgressWork : QuestProgressWork
    {
        public readonly QuestAreaId AreaId;
        public readonly uint Amount;

        public WorldQuestClearedProgressWork(uint questScheduleId, QuestProcessState processState, QuestAreaId areaId, uint amount) : base(questScheduleId, processState, QuestProgressWorkType.WorldQuestCleared)
        {
            AreaId = areaId;
            Amount = amount;
        }

        public bool QuestIsMatch(Quest quest)
        {
            return quest.QuestAreaId == AreaId;
        }

        public override S2CQuestQuestProgressWorkSaveNtc GetWork()
        {
            Console.WriteLine($"QuestScheduleId={QuestScheduleId}, ProcessWork={ProcessState}, AreaId={AreaId}");
            return new S2CQuestQuestProgressWorkSaveNtc()
            {
                QuestScheduleId = QuestScheduleId,
                ProcessNo = ProcessState.ProcessNo,
                SequenceNo = ProcessState.SequenceNo,
                BlockNo = ProcessState.BlockNo,
                WorkList = new List<CDataQuestProgressWork>
                {
                    QuestManager.NotifyCommand.WorldQuestClearNum(AreaId, Amount)
                }
            };
        }
    }
}
