/**
 * @brief Enemy Spot in "Bitterblack Maze Abyss" for "Netherworld 4" Variant C
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BitterblackMazeNetherworld4AbyssCDeath.AsStageLayoutId(31);
    public override QuestAreaId AreaId => QuestAreaId.BitterblackMaze;
    public override uint RequiredAreaRank => 0;

    public class NamedParamId
    {
        public const uint OneThatPromisesDeath = 2863; // One That Promises Death
        public const uint NetherworldGuardian = 2864; // Netherworld Guardian
    }

    private byte InitialiseIndex()
    {
        return new byte[] { 0, 1 }[Random.Shared.Next(0, 2)];
    }

    public override void Initialize()
    {
        // Skip this and override GetInstanceEnemiesInstead
    }

    public override ReadOnlyCollection<InstancedEnemy> GetInstanceEnemies()
    {
        byte pos = InitialiseIndex();
        return new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Death, 55, 80000, pos)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.OneThatPromisesDeath)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        }.AsReadOnly();
    }
}

return new MonsterSpotInfo();
