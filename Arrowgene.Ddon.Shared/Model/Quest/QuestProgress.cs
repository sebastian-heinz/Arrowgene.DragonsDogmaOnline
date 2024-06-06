using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestProgress
    {
        public uint CharacterCommonId { get; set; }
        public QuestId QuestId { get; set; }
        public QuestType QuestType { get; set; }
        public uint Step { get; set; }
    }
}
