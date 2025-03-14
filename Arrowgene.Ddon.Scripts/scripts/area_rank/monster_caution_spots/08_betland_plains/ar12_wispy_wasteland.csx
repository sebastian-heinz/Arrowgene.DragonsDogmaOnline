/**
 * @brief Enemy Spot in "Betland Plains" for "Wispy Wasteland"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(262);
    public override QuestAreaId AreaId => QuestAreaId.BetlandPlains;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint Disturbed = 896; // Disturbed <name>
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 63, 0)
                .SetNamedEnemyParams(NamedParamId.Disturbed),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 63, 1)
                .SetNamedEnemyParams(NamedParamId.Disturbed),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 63, 2)
                .SetNamedEnemyParams(NamedParamId.Disturbed),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 63, 3)
                .SetNamedEnemyParams(NamedParamId.Disturbed),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 63, 4)
                .SetNamedEnemyParams(NamedParamId.Disturbed),
            LibDdon.Enemy.CreateAuto(EnemyId.Gargoyle, 63, 5)
                .SetNamedEnemyParams(NamedParamId.Disturbed),
        };

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
