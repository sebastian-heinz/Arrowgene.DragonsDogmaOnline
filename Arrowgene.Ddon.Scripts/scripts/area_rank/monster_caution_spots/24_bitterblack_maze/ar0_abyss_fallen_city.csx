/**
 * @brief Enemy Spot in "Bitterblack Maze Abyss" for "Fallen City"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BitterblackMazeFallenCity.AsStageLayoutId(6);
    public override QuestAreaId AreaId => QuestAreaId.BitterblackMaze;
    public override uint RequiredAreaRank => 0;

    public class NamedParamId
    {
        public const uint AccursedLord = 2674; // Accursed Lord
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.BlackKnightPhantomClear, 50, 12000, 0, assignDefaultDrops: false)
                .SetIsBoss(true)
                .SetIsAreaBoss(true)
                .SetNamedEnemyParams(NamedParamId.AccursedLord)
                .SetEnemyTargetTypesId(TargetTypesId.AreaBoss),
        });
    }
}

return new MonsterSpotInfo();
