/**
 * @brief Enemy Spot in "Volden Mines" for "Mines' Large Landing"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.VoldenLargeTunnels0.AsStageLayoutId(23);
    public override QuestAreaId AreaId => QuestAreaId.VoldenMines;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint SkeletonKnightOfTheTunnel = 894; // Skeleton Knight of the Tunnel
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SkullLord, 63, 0)
                .SetNamedEnemyParams(NamedParamId.SkeletonKnightOfTheTunnel),
            LibDdon.Enemy.CreateAuto(EnemyId.SkullLord, 63, 1)
                .SetNamedEnemyParams(NamedParamId.SkeletonKnightOfTheTunnel),
            LibDdon.Enemy.CreateAuto(EnemyId.SkullLord, 63, 2)
                .SetNamedEnemyParams(NamedParamId.SkeletonKnightOfTheTunnel),
        };

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
