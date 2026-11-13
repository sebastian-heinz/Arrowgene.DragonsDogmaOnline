#load "libs.csx"

public class SpecialCondition : IJobOrbSpecialCondition
{
    public override uint ConditionId => 10;
    public override string Message => "Epitaph Road (Megadosys): Mountain Trail";
    
    public override bool EvaluateCondition(GameClient client)
    {
        return LibDdon.EpitaphRoadMgr.IsStatueUnlocked(client, Stage.MemoryofMegadosys, 130, 0);
    }
}

return new SpecialCondition();