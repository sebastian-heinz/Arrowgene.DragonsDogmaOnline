/**
 * @brief Enemy Spot in "Bitterblack Maze Abyss" for "Traitor's Tower"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BitterblackMazeTraitorsTower.AsStageLayoutId(3);
    public override QuestAreaId AreaId => QuestAreaId.BitterblackMaze;
    public override uint RequiredAreaRank => 0;

    public class NamedParamId
    {
        public const uint DevilInADeadCity = 2670; //  Devil in a Dead City
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.EvilEye1, 26, 13500, 0, assignDefaultDrops: false)
                .SetIsBoss(true)
                .SetIsAreaBoss(true)
                .SetStartThinkTblNo(1)
                .SetNamedEnemyParams(NamedParamId.DevilInADeadCity)
                .SetEnemyTargetTypesId(TargetTypesId.AreaBoss),
        });
    }
}

return new MonsterSpotInfo();
