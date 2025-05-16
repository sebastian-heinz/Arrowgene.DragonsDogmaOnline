/**
 * @brief Enemy Spot in "Bitterblack Maze Abyss" for "Noxious Cathedral"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BitterblackMazeNoxiousCathedral.AsStageLayoutId(5);
    public override QuestAreaId AreaId => QuestAreaId.BitterblackMaze;
    public override uint RequiredAreaRank => 0;

    public class NamedParamId
    {
        public const uint LordOfTheFeast = 2666; // Lord of the Feast
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.AlteredZuhl, 10, 2250, 0, assignDefaultDrops: false)
                .SetIsBoss(true)
                .SetIsAreaBoss(true)
                .SetNamedEnemyParams(NamedParamId.LordOfTheFeast)
                .SetEnemyTargetTypesId(TargetTypesId.AreaBoss),
        });
    }
}

return new MonsterSpotInfo();
