/**
 * @brief Enemy Spot in "Deenan Woods" for "Groan Nos"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(332);
    public override QuestAreaId AreaId => QuestAreaId.DeenanWoods;
    public override uint RequiredAreaRank => 3;

    public class NamedParamId
    {
        public const uint ClayDollOfStagnation = 241; // Clay Doll of Stagnation <name>
        public const uint CrystalOfStagnation = 242; //  Crystal of Stagnation <name>
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, 41, 0)
                .SetNamedEnemyParams(NamedParamId.ClayDollOfStagnation)
                .AddDrop(ItemId.MagickSilt, 1, 3, DropRate.UNCOMMON)
                .AddDrop(ItemId.SilverOre, 1, 3, DropRate.UNCOMMON),
            LibDdon.Enemy.CreateAuto(EnemyId.GluttonOoze, 41, 1)
                .SetNamedEnemyParams(NamedParamId.CrystalOfStagnation),
            LibDdon.Enemy.CreateAuto(EnemyId.GluttonOoze, 41, 2)
                .SetNamedEnemyParams(NamedParamId.CrystalOfStagnation),
            LibDdon.Enemy.CreateAuto(EnemyId.GluttonOoze, 41, 3)
                .SetNamedEnemyParams(NamedParamId.CrystalOfStagnation),
            LibDdon.Enemy.CreateAuto(EnemyId.GluttonOoze, 41, 4)
                .SetNamedEnemyParams(NamedParamId.CrystalOfStagnation),
        });
    }
}

return new MonsterSpotInfo();
