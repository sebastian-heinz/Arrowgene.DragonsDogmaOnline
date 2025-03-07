/**
 * @brief Enemy Spot in "Dowe Valley" for "Dreed Watchtower"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(211);
    public override QuestAreaId AreaId => QuestAreaId.DoweValley;
    public override uint RequiredAreaRank => 2;

    public class NamedParamId
    {
        public const uint SnowGuraHarpy = 220; // Snow Gura Harpy
    }

    public override void Initialize()
    {
        var dropsTable = LibDdon.Enemy.GetDropsTable(EnemyId.SnowHarpy, 30).Clone()
            .AddDrop(ItemId.CrestOfFreezing0, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.CrestOfColdWarding0, 1, 1, DropRate.UNCOMMON);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 30, 0)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.SnowGuraHarpy),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 30, 1)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.SnowGuraHarpy),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 30, 2)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.SnowGuraHarpy),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 30, 3)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.SnowGuraHarpy),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 30, 4)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.SnowGuraHarpy),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 30, 5)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.SnowGuraHarpy),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 30, 6)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.SnowGuraHarpy),
            LibDdon.Enemy.CreateAuto(EnemyId.SnowHarpy, 30, 7)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.SnowGuraHarpy),
        });
    }
}

return new MonsterSpotInfo();
