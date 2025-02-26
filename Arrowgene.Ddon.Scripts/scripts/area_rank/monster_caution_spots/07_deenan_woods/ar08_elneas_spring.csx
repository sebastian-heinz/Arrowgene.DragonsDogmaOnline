/**
 * @brief Enemy Spot in "Deenan Woods" for "Elnea Spring"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(349);
    public override QuestAreaId AreaId => QuestAreaId.DeenanWoods;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint WanderingSpiritOfTheSpring0 = 243; // Wandering Spirit of the Spring
        public const uint WanderingSpiritOfTheSpring1 = 245; // Wandering Spirit of the Spring
        public const uint WanderingSpiritOfTheSpring2 = 246; // Wandering Spirit of the Spring
        public const uint WanderingSpiritOfTheSpring3 = 247; // Wandering Spirit of the Spring
    }

    public override void Initialize()
    {
        var dropsTable = LibDdon.Enemy.GetDropsTable(EnemyId.MistFighter, 45).Clone()
            .AddDrop(ItemId.CrestOfFlameWarding0, 1, 3, DropRate.UNCOMMON)
            .AddDrop(ItemId.CrestOfHolyWarding0, 1, 3, DropRate.UNCOMMON);

        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.MistFighter, 45, 0)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.WanderingSpiritOfTheSpring0),
            LibDdon.Enemy.CreateAuto(EnemyId.MistFighter, 45, 1)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.WanderingSpiritOfTheSpring0),
            LibDdon.Enemy.CreateAuto(EnemyId.MistFighter, 45, 2)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.WanderingSpiritOfTheSpring0),
            LibDdon.Enemy.CreateAuto(EnemyId.MistHunter, 45, 3)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.WanderingSpiritOfTheSpring1),
            LibDdon.Enemy.CreateAuto(EnemyId.MistHunter, 45, 4)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.WanderingSpiritOfTheSpring1),
            LibDdon.Enemy.CreateAuto(EnemyId.MistSorcerer, 45, 5)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.WanderingSpiritOfTheSpring2),
            LibDdon.Enemy.CreateAuto(EnemyId.MistSorcerer, 45, 6)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.WanderingSpiritOfTheSpring2),
        };

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
