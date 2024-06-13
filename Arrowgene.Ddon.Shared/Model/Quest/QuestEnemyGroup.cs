using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public class QuestEnemyGroup
    {
        public uint SubGroupId { get; set; }
        public StageId StageId { get; set; }
        public uint StartingIndex { get; set; }
        public List<Enemy> Enemies { get; set; }

        public QuestEnemyGroup()
        {
            Enemies = new List<Enemy>();
            StageId = StageId.Invalid;
        }
    }
}
