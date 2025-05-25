#load "libs.csx"

public class JobEmblemStat : IJobEmblemStatTable
{
    public override EquipStatId StatId => EquipStatId.MaxHp;
}

var jobEmblemStat = new JobEmblemStat();

jobEmblemStat
    .Add(0, 40) // 40
    .Add(1, 40) // 80
    .Add(2, 40) // 120
    .Add(3, 40) // 160
    .Add(4, 40) // 200
    .Add(5, 60) // 260
    .Add(6, 60) // 320
    .Add(7, 150) // 470
    .Add(8, 180) // 650
    .Add(9, 250) // 900
    .Add(10, 300) // 1200
    .Add(11, 300) // 1500
    .Add(12, 300) // 1800
    .Add(13, 300) // 2100
    .Add(14, 400) // 2500
    .Add(15, 2500); // 5000

return jobEmblemStat;
