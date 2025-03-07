/**
 * @brief Enemy Spot fpr "Hidell Plains" for "Blade Scarred Path"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(32);
    public override QuestAreaId AreaId => QuestAreaId.HidellPlains;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint Wandering = 891; // Wandering <name>
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.AlchemizedSkeleton, 57, 6706, 0)
                .SetStartThinkTblNo(2)
                .SetNamedEnemyParams(NamedParamId.Wandering),
            LibDdon.Enemy.Create(EnemyId.AlchemizedSkeleton, 57, 6706, 1)
                .SetStartThinkTblNo(2)
                .SetNamedEnemyParams(NamedParamId.Wandering),
            LibDdon.Enemy.Create(EnemyId.AlchemizedSkeleton, 57, 6706, 2)
                .SetStartThinkTblNo(2)
                .SetNamedEnemyParams(NamedParamId.Wandering),
            LibDdon.Enemy.Create(EnemyId.AlchemizedSkeleton, 57, 6706, 3)
                .SetStartThinkTblNo(2)
                .SetNamedEnemyParams(NamedParamId.Wandering),
        });
    }
}

return new MonsterSpotInfo();
