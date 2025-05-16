/**
 * @brief Enemy Spot in "Bitterblack Maze Rotunda" for "Rotunda of the Dead"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BitterblackMazeRotundaofDread.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.BitterblackMaze;
    public override uint RequiredAreaRank => 0;

    public class NamedParamId
    {
        public const uint TheDevastator = 2366; // The Devastator
        public const uint NetherworldFamiliar = 2355; // Netherworld Familiar
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.CursedDragon, 48, 13500, 0, assignDefaultDrops: false)
                .SetIsBoss(true)
                .SetIsAreaBoss(true)
                .SetNamedEnemyParams(NamedParamId.TheDevastator)
                .AddDrop(ItemId.BitterblackDeedBox, 1, 2, DropRate.COMMON)
                .AddDrop(ItemId.BitterblackOrb, 1, 2, DropRate.COMMON)
                .SetEnemyTargetTypesId(TargetTypesId.AreaBoss),
            LibDdon.Enemy.Create(EnemyId.Ghost, 48, 1750, 1, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.NetherworldFamiliar),
            LibDdon.Enemy.Create(EnemyId.Ghost, 48, 1750, 2, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.NetherworldFamiliar),
            LibDdon.Enemy.Create(EnemyId.Ghost, 48, 1750, 3, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.NetherworldFamiliar),
            LibDdon.Enemy.Create(EnemyId.Ghost, 48, 1750, 4, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.NetherworldFamiliar),
            LibDdon.Enemy.Create(EnemyId.Ghost, 48, 1750, 5, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.NetherworldFamiliar),
            LibDdon.Enemy.Create(EnemyId.Ghost, 48, 1750, 6, assignDefaultDrops: false)
                .SetNamedEnemyParams(NamedParamId.NetherworldFamiliar),
        });
    }
}

return new MonsterSpotInfo();
