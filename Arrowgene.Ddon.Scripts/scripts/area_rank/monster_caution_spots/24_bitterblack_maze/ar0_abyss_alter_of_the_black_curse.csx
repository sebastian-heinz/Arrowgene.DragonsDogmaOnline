/**
 * @brief Enemy Spot in "Bitterblack Maze Abyss" for "Alter of the Black Curse"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BitterblackMazeAltaroftheBlackCurse.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.BitterblackMaze;
    public override uint RequiredAreaRank => 0;

    public class NamedParamId
    {
        public const uint TheFallen0 = 3154; // The Fallen
        public const uint TheFallen1 = 3048; // The Fallen
        public const uint TheFallen2 = 2678; // The Fallen
    }

    private InstancedEnemy CreateRandomBoss(ushort lv, uint exp, byte index)
    {
        return LibDdon.Enemy.CreateRandom(lv, exp, index, new List<(EnemyId, uint)>() {
            (EnemyId.Leo, NamedParamId.TheFallen0),
            (EnemyId.DiamantesHuman, NamedParamId.TheFallen1),
            (EnemyId.HoodedIris, NamedParamId.TheFallen2),
        }, assignDefaultDrops: false);
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            CreateRandomBoss(55, 15000, 0)
                .SetIsBoss(true)
                .SetIsAreaBoss(true)
                .SetEnemyTargetTypesId(TargetTypesId.AreaBoss),
        });
    }
}

return new MonsterSpotInfo();
