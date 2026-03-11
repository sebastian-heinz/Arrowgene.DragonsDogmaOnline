/**
 * @brief Enemy Spot in "Bitterblack Maze Rotunda" for "Netherworld 3" Variant A
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BitterblackMazeNetherworld3RoutundaADeath.AsStageLayoutId(9);
    public override QuestAreaId AreaId => QuestAreaId.BitterblackMaze;
    public override uint RequiredAreaRank => 0;

    public class NamedParamId
    {
        public const uint OneThatPromisesDeath = 2863; // One That Promises Death
        public const uint NetherworldGuardian = 2864; // Netherworld Guardian
    }

    private byte InitialiseIndex()
    {
        return new byte[] { 0, 1, 2, 3 }[Random.Shared.Next(0, 4)];
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
            LibDdon.Enemy.Create(EnemyId.Death, 48, 10550, pos)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.OneThatPromisesDeath)
                .SetEnemyTargetTypesId(TargetTypesId.Normal),
        }.AsReadOnly();
    }
}

return new MonsterSpotInfo();
