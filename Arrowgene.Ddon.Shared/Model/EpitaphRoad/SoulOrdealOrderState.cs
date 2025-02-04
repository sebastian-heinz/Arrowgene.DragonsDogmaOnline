using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.EpitaphRoad
{
    public enum SoulOrdealOrderState : byte
    {
        GetList = 0,
        Start = 1,
        Waiting = 2,
        Interrupt = 3,
        GetRewards = 4,
        Completed = 5, // Stone Slab is glowing blue message
        UnlockTrial = 6, // BLOCKADE_ID_FROM_OM
    }
}
