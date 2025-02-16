using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestEvent
    {
        public int EventId { get; set; }
        public StageLayoutId JumpStageId { get; set; }
        public int StartPosNo { get; set; }
        public QuestJumpType JumpType { get; set; }
    }
}
