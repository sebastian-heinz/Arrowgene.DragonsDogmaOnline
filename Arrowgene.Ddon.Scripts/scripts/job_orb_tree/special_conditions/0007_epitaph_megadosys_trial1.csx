#load "libs.csx"

public class SpecialCondition : IJobOrbSpecialCondition
{
    public override uint ConditionId => 7;
    public override string Message => "Epitaph Road (Megadosys): Castle Grounds";
    
    public override bool EvaluateCondition(GameClient client)
    {
        return LibDdon.EpitaphRoadMgr.IsStatueUnlocked(client, Stage.MemoryofMegadosys, 100, 0);
    }
}

return new SpecialCondition();