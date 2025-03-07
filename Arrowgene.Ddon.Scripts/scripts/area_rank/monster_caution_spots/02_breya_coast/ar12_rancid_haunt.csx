/**
 * @brief Enemy Spot in "Breya Coast" for "Rancid Haunt"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(91);
    public override QuestAreaId AreaId => QuestAreaId.BreyaCoast;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint SparkBlue = 892; // Spark Blue
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.LargeNewt, 60, 18000, 0)
                .SetNamedEnemyParams(NamedParamId.SparkBlue),
            LibDdon.Enemy.Create(EnemyId.BlueNewt, 60, 15254, 0),
            LibDdon.Enemy.Create(EnemyId.BlueNewt, 60, 15254, 0),
            LibDdon.Enemy.Create(EnemyId.BlueNewt, 60, 15254, 0),
            LibDdon.Enemy.Create(EnemyId.BlueNewt, 60, 15254, 0),
            LibDdon.Enemy.Create(EnemyId.BlueNewt, 60, 15254, 0),
        });
    }
}

return new MonsterSpotInfo();
