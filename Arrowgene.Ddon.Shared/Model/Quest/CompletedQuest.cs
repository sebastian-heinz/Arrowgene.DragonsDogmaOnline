using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class CompletedQuest
    {
        public QuestType QuestType { get; set; }
        public QuestId QuestId { get; set; }
        public uint ClearCount { get; set; }
    }
}
