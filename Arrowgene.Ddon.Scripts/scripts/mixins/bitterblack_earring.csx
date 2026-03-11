#load "libs.csx"

public class Mixin : IBitterblackEarringMixin
{
    public override ushort RollBitterBlackMazeEarringPercent(JobId jobId)
    {
        /**
        * Based on research in discord, Warrior and Shield sage earrings can roll a
        * higher % range 8-20% when being appraised. The rest of the jobs can roll
        * 1-13% bonus on their equipment. The percentage values are encoded as ushorts.
        * For example 2 == 2% in the UI.
        */
        if (jobId == JobId.Warrior || jobId == JobId.ShieldSage)
        {
            // [8, 20]
            return (ushort)Random.Shared.Next(8, 20 + 1);
        }
        // [1, 13]
        return (ushort)Random.Shared.Next(1, 13 + 1);
    }
}

return new Mixin();
