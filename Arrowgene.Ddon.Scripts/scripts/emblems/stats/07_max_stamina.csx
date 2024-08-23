#load "libs.csx"

public class JobEmblemStat : IJobEmblemStatTable
{
    public override EquipStatId StatId => EquipStatId.MaxStamina;
}

var jobEmblemStat = new JobEmblemStat();

jobEmblemStat
    .Add(0, 30) // 30
    .Add(1, 30) // 60
    .Add(2, 30) // 90
    .Add(3, 30) // 120
    .Add(4, 30) // 150
    .Add(5, 50) // 200
    .Add(6, 50) // 250
    .Add(7, 100) // 350
    .Add(8, 150) // 500
    .Add(9, 200) // 700
    .Add(10, 250) // 950
    .Add(11, 250) // 1200
    .Add(12, 250) // 1450
    .Add(13, 250) // 1700
    .Add(14, 300) // 2000
    .Add(15, 1000); // 3000

return jobEmblemStat;
