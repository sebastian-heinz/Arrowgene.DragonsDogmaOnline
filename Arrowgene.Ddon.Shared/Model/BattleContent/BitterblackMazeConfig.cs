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
            StageId = StageLayoutId.Invalid;
            ContentName = "";
            ContentMode = BattleContentMode.Rotunda;
        }

        public uint ContentId { get; set; }
        public string ContentName { get; set; }
        public uint Tier { get; set; }
        public List<uint> Destinations { get; set; }
        public StageLayoutId StageId { get; set; }
        public BattleContentMode ContentMode {  get; set; }
    }
}
