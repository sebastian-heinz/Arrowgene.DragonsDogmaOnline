#load "libs.csx"

public class JobEmblemStat : IJobEmblemStatTable
{
    public override EquipStatId StatId => EquipStatId.HealingPower;
}

var jobEmblemStat = new JobEmblemStat();

jobEmblemStat
    .Add(0, 5) // 5
    .Add(1, 2) // 7
    .Add(2, 2) // 9
    .Add(3, 4) // 13
    .Add(4, 4) // 17
    .Add(5, 6) // 23
    .Add(6, 6) // 29
    .Add(7, 7) // 36
    .Add(8, 7) // 43
    .Add(9, 7) // 50
    .Add(10, 7) // 57
    .Add(11, 7) // 64
    .Add(12, 7) // 71
    .Add(13, 7) // 78
    .Add(14, 7) // 85
    .Add(15, 15); // 100

return jobEmblemStat;
