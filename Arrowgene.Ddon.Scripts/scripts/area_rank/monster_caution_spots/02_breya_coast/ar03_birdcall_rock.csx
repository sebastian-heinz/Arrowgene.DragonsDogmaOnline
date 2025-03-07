/**
 * @brief Enemy Spot in "Breya Coast" for "Birdcall Rock"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(86);
    public override QuestAreaId AreaId => QuestAreaId.BreyaCoast;
    public override uint RequiredAreaRank => 3;

    public class NamedParamId
    {
        public const uint MotherHarpy = 200; // Mother Harpy
    }

    public override void Initialize()
    {
        var namedEnemyDrops = LibDdon.Enemy.GetDropsTable(EnemyId.Harpy, 13).Clone()
            .AddDrop(ItemId.Amethyst, 1, 2, DropRate.UNCOMMON)
            .AddDrop(ItemId.HardWood, 1, 1, DropRate.UNCOMMON);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Harpy, 13, 384, 1)
                .SetDropsTable(namedEnemyDrops)
                .SetNamedEnemyParams(NamedParamId.MotherHarpy),
            LibDdon.Enemy.Create(EnemyId.Harpy, 13, 276, 0),
            LibDdon.Enemy.Create(EnemyId.Harpy, 13, 276, 2),
            LibDdon.Enemy.Create(EnemyId.Harpy, 13, 276, 3),
            LibDdon.Enemy.Create(EnemyId.Harpy, 13, 276, 4),
        });
    }
}

return new MonsterSpotInfo();
