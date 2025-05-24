#load "libs.csx"

public class JobEmblemStat : IJobEmblemStatTable
{
    public override EquipStatId StatId => EquipStatId.DarkResist;
}

var jobEmblemStat = new JobEmblemStat();

jobEmblemStat
    .Add(0, 0) // 0
    .Add(1, 5) // 5
    .Add(2, 5) // 10
    .Add(3, 5) // 15
    .Add(4, 5) // 20
    .Add(5, 10) // 30
    .Add(6, 10) // 40
    .Add(7, 10) // 50
    .Add(8, 10) // 60
    .Add(9, 10) // 70
    .Add(10, 2) // 72
    .Add(11, 2) // 74
    .Add(12, 2) // 76
    .Add(13, 2) // 78
    .Add(14, 2) // 80
    .Add(15, 20); // 100

return jobEmblemStat;
