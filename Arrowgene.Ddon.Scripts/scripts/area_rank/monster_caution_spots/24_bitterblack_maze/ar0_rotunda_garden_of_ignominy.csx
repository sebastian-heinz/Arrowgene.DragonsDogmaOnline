/**
 * @brief Enemy Spot in "Bitterblack Maze Rotunda" for "Garden of Ignominy"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BitterblackMazeGardenofIgnominy.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.BitterblackMaze;
    public override uint RequiredAreaRank => 0;

    public class NamedParamId
    {
        public const uint NetherworldFiend = 2358; // Netherworld Fiend
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Zuhl1, 10, 2250, 0, assignDefaultDrops: false)
                .SetIsBoss(true)
                .SetIsAreaBoss(true)
                .SetNamedEnemyParams(NamedParamId.NetherworldFiend)
                .SetEnemyTargetTypesId(TargetTypesId.AreaBoss)
        });
    }
}

return new MonsterSpotInfo();
