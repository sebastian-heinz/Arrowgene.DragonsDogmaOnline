using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.BattleContent
{
    public class BitterblackMazeMarkRewards
    {
        public uint StageId { get; set; }
        public uint GoldMarks { get; set; }
        public uint SilverMarks { get; set; }
        public uint RedMarks { get; set; }

        public bool Any
        {
            get
            {
                return GoldMarks > 0 || SilverMarks > 0 || RedMarks > 0;
            }
        }
    }
}
