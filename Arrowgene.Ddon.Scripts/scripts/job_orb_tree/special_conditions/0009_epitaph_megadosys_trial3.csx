#load "libs.csx"

public class SpecialCondition : IJobOrbSpecialCondition
{
    public override uint ConditionId => 9;
    public override string Message => "Epitaph Road (Megadosys): Highway";
    
    public override bool EvaluateCondition(GameClient client)
    {
        return LibDdon.EpitaphRoadMgr.IsStatueUnlocked(client, Stage.MemoryofMegadosys, 120, 0);
    }
}

return new SpecialCondition();