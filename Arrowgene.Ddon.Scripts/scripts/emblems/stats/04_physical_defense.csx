#load "libs.csx"

public class JobEmblemStat : IJobEmblemStatTable
{
    public override EquipStatId StatId => EquipStatId.PhysicalDefense;
}

var jobEmblemStat = new JobEmblemStat();

jobEmblemStat
    .Add(0, 1)
    .Add(1, 1)
    .Add(2, 1)
    .Add(3, 1)
    .Add(4, 1)
    .Add(5, 1)
    .Add(6, 1)
    .Add(7, 1)
    .Add(8, 1)
    .Add(9, 1)
    .Add(10, 1)
    .Add(11, 1)
    .Add(12, 1)
    .Add(13, 1)
    .Add(14, 1)
    .Add(15, 5);

return jobEmblemStat;
