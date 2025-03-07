/**
 * @brief Enemy Spot in "Mysree Grove" for "Nameless Deserted Village"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(133);
    public override QuestAreaId AreaId => QuestAreaId.MysreeGrove;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint ForgottenSpecter = 229; // Forgotten Specter
        public const uint EnlightenedUndead = 230; //  Enlightened Undead
        public const uint WanderingUndead = 231; //  Wandering Undead
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Wight0, 44, 0)
                .SetIsBoss(true)
                .SetSpawnTime(GameTimeManager.NightTime)
                .SetNamedEnemyParams(NamedParamId.ForgottenSpecter),
            LibDdon.Enemy.CreateAuto(EnemyId.SwordUndead, 42, 1)
                .SetSpawnTime(GameTimeManager.NightTime)
                .SetNamedEnemyParams(NamedParamId.EnlightenedUndead),
            LibDdon.Enemy.CreateAuto(EnemyId.Mudman, 42, 2)
                .SetSpawnTime(GameTimeManager.NightTime)
                .SetNamedEnemyParams(NamedParamId.WanderingUndead),
            LibDdon.Enemy.CreateAuto(EnemyId.SkeletonSorcerer0, 42, 3)
                .SetSpawnTime(GameTimeManager.NightTime)
                .SetNamedEnemyParams(NamedParamId.EnlightenedUndead),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.OrbOfDarkness, 1, 2, DropRate.COMMON)
            .AddDrop(ItemId.BlackSkull, 1, 2, DropRate.UNCOMMON);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
