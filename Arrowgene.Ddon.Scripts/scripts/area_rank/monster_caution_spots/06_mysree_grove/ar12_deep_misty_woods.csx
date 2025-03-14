/**
 * @brief Enemy Spot in "Mysree Grove" for "Deep Misty Woods"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(151);
    public override QuestAreaId AreaId => QuestAreaId.MysreeGrove;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint FigureFluctuatingInTheMist = 895; // Figure Fluctuating in the Mist
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.MistWarrior, 63, 0)
                .SetNamedEnemyParams(NamedParamId.FigureFluctuatingInTheMist),
            LibDdon.Enemy.CreateAuto(EnemyId.MistWarrior, 63, 1)
                .SetNamedEnemyParams(NamedParamId.FigureFluctuatingInTheMist),
            LibDdon.Enemy.CreateAuto(EnemyId.MistWarrior, 63, 2)
                .SetNamedEnemyParams(NamedParamId.FigureFluctuatingInTheMist),
            LibDdon.Enemy.CreateAuto(EnemyId.MistWarrior, 63, 3)
                .SetNamedEnemyParams(NamedParamId.FigureFluctuatingInTheMist),
            LibDdon.Enemy.CreateAuto(EnemyId.MistWarrior, 63, 4)
                .SetNamedEnemyParams(NamedParamId.FigureFluctuatingInTheMist),
            LibDdon.Enemy.CreateAuto(EnemyId.MistWarrior, 63, 5)
                .SetNamedEnemyParams(NamedParamId.FigureFluctuatingInTheMist),
        };

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
