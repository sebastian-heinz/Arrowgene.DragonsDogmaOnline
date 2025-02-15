using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestNpcOrder
    {
        public NpcId NpcId { get; set; }
        public int MsgId { get; set; }
        public StageLayoutId StageId { get; set; }
        public QuestId QuestId { get; set; }

        public QuestNpcOrder()
        {
            StageId = StageLayoutId.Invalid;
        }
    }
}
