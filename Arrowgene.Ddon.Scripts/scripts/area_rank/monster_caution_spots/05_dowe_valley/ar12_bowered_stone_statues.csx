/**
 * @brief Enemy Spot in "Dowe Valley" for "The Songstress' Cliff Perch"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(229);
    public override QuestAreaId AreaId => QuestAreaId.DoweValley;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint GroveStatueDemon = 897; //  Grove Statue Demon
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 68, 0)
                .SetNamedEnemyParams(NamedParamId.GroveStatueDemon),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 68, 1)
                .SetNamedEnemyParams(NamedParamId.GroveStatueDemon),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 68, 2)
                .SetNamedEnemyParams(NamedParamId.GroveStatueDemon),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 68, 3)
                .SetNamedEnemyParams(NamedParamId.GroveStatueDemon),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 68, 4)
                .SetNamedEnemyParams(NamedParamId.GroveStatueDemon),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 68, 5)
                .SetNamedEnemyParams(NamedParamId.GroveStatueDemon),
        };

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
