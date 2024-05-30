using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class QuestProcess
    {
        public ushort ProcessNo { get; set; }
        public List<QuestBlock> Blocks {  get; set; }

        public QuestProcess()
        {
            Blocks = new List<QuestBlock>();
        }
    }
}
