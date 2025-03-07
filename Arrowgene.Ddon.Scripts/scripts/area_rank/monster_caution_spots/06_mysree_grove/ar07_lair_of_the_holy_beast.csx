/**
 * @brief Enemy Spot in "Mysree Grove" for "Lair of the Holy Beast"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.SacredSpringTerrace.AsStageLayoutId(3);
    public override QuestAreaId AreaId => QuestAreaId.MysreeGrove;
    public override uint RequiredAreaRank => 7;

    public class NamedParamId
    {
        public const uint WhiteLionAlbus = 264; // White Lion Albus
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.WhiteChimera0, 44, 3)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.WhiteLionAlbus),
            LibDdon.Enemy.CreateAuto(EnemyId.Direwolf, 42, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.Direwolf, 42, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.Direwolf, 42, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.Direwolf, 42, 4),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.CrestOfDecreasedHolyResist, 1, 2, DropRate.UNCOMMON)
            .AddDrop(ItemId.CrestOfGreaterMagick0, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.CrestOfLightWarding0, 1, 2, DropRate.UNCOMMON)
            .AddDrop(ItemId.GoatHorn, 1, 2, DropRate.COMMON);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
