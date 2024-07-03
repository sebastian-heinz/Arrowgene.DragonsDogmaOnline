using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.BattleContent
{
    public class BitterblackMazeConfig
    {
        public BitterblackMazeConfig()
        {
            Destinations = new List<uint>();
            StageId = StageId.Invalid;
            ContentName = "";
        }

        public string ContentName { get; set; }
        public byte Tier { get; set; }
        public List<uint> Destinations { get; set; }
        public StageId StageId { get; set; }
    }
}
