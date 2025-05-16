/**
 * @brief Enemy Spot in "Bitterblack Maze Rotunda" for "Duskmoon Tower"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BitterblackMazeDuskmoonTower.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.BitterblackMaze;
    public override uint RequiredAreaRank => 0;

    public class NamedParamId
    {
        public const uint KingOfTheNetherworld = 2362; // King of the Netherworld
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Scourge1, 25, 13500, 0, assignDefaultDrops: false)
                .SetIsBoss(true)
                .SetIsAreaBoss(true)
                .SetNamedEnemyParams(NamedParamId.KingOfTheNetherworld)
                .SetEnemyTargetTypesId(TargetTypesId.AreaBoss)
        });
    }
}

return new MonsterSpotInfo();
