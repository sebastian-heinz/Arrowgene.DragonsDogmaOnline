#load "libs.csx"

public class SpecialCondition : IJobOrbSpecialCondition
{
    public override uint ConditionId => 8;
    public override string Message => "Epitaph Road (Megadosys): Forest";
    
    public override bool EvaluateCondition(GameClient client)
    {
        return LibDdon.EpitaphRoadMgr.IsStatueUnlocked(client, Stage.MemoryofMegadosys, 110, 0);
    }
}

return new SpecialCondition();