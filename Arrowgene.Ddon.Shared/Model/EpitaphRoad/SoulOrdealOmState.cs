using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model.EpitaphRoad
{
    public enum SoulOrdealOmState : byte
    {
        Locked = 0, // White in 3.2 and 3.3 stages
        LegacyTrialAvailable = 0, // Red in 3.0 and 3.1 stages
        AreaUnlocked = 1, // Blows up rock wall, opens doors, etc.
        White1 = 1,
        White2 = 2,
        TrialComplete = 3, // BlueWithAnimation = 3,
        Blue = 4,
        RewardAvailable = 5, // BlueWithSparkle = 5,
        RewardReceived = 6, // Blue
        TrialAvailable = 10, // Red = 10

        DoorLocked = 0,
        ScatterPowers = 7,
        DoorUnlocked = 8,
        GatheringPointSpawned = 0, // Spawns green light pillar
    }
}
