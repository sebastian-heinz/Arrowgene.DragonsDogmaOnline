#load "libs.csx"

public class SpecialCondition : IJobOrbSpecialCondition
{
    public override uint ConditionId => 3;
    public override string Message => "Hero's Rest (Rathnite): Waterway Trial";

    public override bool EvaluateCondition(GameClient client)
    {
        return LibDdon.EpitaphRoadMgr.IsStatueUnlocked(client, Stage.HeroicSpiritSleepingPathWaterway, 33, 0);
    }
}

return new SpecialCondition();
