/**
 * @brief Enemy Spot in "Mergoda Ruins" for "Alchemy Laboratory"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.MergodaRuinsLowerLevel.AsStageLayoutId(2);
    public override QuestAreaId AreaId => QuestAreaId.MergodaRuins;
    public override uint RequiredAreaRank => 3;

    public class NamedParamId
    {
        public const uint SyntheticAlchemizedGoblin = 277; // Synthetic Alchemized Goblin
        public const uint SyntheticEye = 278; // Synthetic Eye
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.CrystalEye, 51, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.SyntheticEye),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedGoblin, 50, 1)
                .SetNamedEnemyParams(NamedParamId.SyntheticAlchemizedGoblin),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedGoblin, 50, 2)
                .SetNamedEnemyParams(NamedParamId.SyntheticAlchemizedGoblin),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedGoblin, 50, 3)
                .SetNamedEnemyParams(NamedParamId.SyntheticAlchemizedGoblin),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedGoblin, 50, 4)
                .SetNamedEnemyParams(NamedParamId.SyntheticAlchemizedGoblin),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedGoblin, 50, 5)
                .SetNamedEnemyParams(NamedParamId.SyntheticAlchemizedGoblin),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedGoblin, 50, 6)
                .SetNamedEnemyParams(NamedParamId.SyntheticAlchemizedGoblin),
        });
    }
}

return new MonsterSpotInfo();
