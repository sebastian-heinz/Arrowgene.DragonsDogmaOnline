#load "libs.csx"

public class JobEmblemStat : IJobEmblemStatTable
{
    public override EquipStatId StatId => EquipStatId.BlowPower;
}

var jobEmblemStat = new JobEmblemStat();

jobEmblemStat
    .Add(0, 2) // 2
    .Add(1, 2) // 4
    .Add(2, 2) // 6
    .Add(3, 2) // 8
    .Add(4, 2) // 10
    .Add(5, 2) // 12
    .Add(6, 2) // 14
    .Add(7, 2) // 16
    .Add(8, 2) // 18
    .Add(9, 2) // 20
    .Add(10, 2) // 22
    .Add(11, 2) // 24
    .Add(12, 2) // 26
    .Add(13, 2) // 28
    .Add(14, 2) // 30
    .Add(15, 10); // 40

return jobEmblemStat;
