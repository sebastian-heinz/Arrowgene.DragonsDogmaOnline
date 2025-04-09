/**
 * @brief Enemy Spot in "Betland Plains" for "Betland Bridge"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(257);
    public override QuestAreaId AreaId => QuestAreaId.BetlandPlains;
    public override uint RequiredAreaRank => 3;

    public class NamedParamId
    {
        public const uint ColossusPatrol = 265; // Colossus Patrol
        public const uint OddsElite = 223; // Odd's Elite
        public const uint OddTheShieldBreaker = 224; // Odd the Shield-Breaker
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.CaptainOrc0, 40, 3)
                .SetNamedEnemyParams(NamedParamId.OddTheShieldBreaker),
            LibDdon.Enemy.CreateAuto(EnemyId.Colossus0, 38, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.ColossusPatrol),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcAimer, 37, 1)
                .SetNamedEnemyParams(NamedParamId.OddsElite),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcAimer, 37, 2)
                .SetNamedEnemyParams(NamedParamId.OddsElite),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcAimer, 37, 4)
                .SetNamedEnemyParams(NamedParamId.OddsElite),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.ChampionsRing, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.MentorsRing, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.LifeRing, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
