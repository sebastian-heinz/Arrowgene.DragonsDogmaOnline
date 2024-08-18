using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.BattleContent
{
    public class BitterblackMazeProgress
    {
        public BitterblackMazeProgress()
        {
        }
        
        public ulong StartTime { get; set; }
        public uint ContentId { get; set; }
        public BattleContentMode ContentMode { get; set; }
        public uint Tier {  get; set; }
        public bool KilledDeath { get; set; }
        public ulong LastTicketTime {  get; set; }
    }
}
