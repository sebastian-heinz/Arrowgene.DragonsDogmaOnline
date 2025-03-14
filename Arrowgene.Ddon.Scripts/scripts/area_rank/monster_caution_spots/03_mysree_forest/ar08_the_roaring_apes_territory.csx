/**
 * @brief Enemy Spot in "Mysree Forest" for "The Roaring Ape's Territory"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(53);
    public override QuestAreaId AreaId => QuestAreaId.MysreeForest;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint HowlerApe = 208; // Howler Ape
    }

    public override void Initialize()
    {
        var dropsTable = LibDdon.Enemy.GetDropsTable(EnemyId.DreadApe, 27).Clone()
            .AddDrop(ItemId.CrookedFang, 1, 3, DropRate.UNCOMMON)
            .AddDrop(ItemId.LargeSimianThickPelt, 1, 3, DropRate.UNCOMMON)
            .AddDrop(ItemId.NecrophagousBristle, 1, 1, DropRate.RARE);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.DreadApe, 27, 1126, 1)
                .SetIsBoss(true)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.HowlerApe),

            // Keep low level trash enemies
            LibDdon.Enemy.Create(EnemyId.Wolf, 5, 46, 0),
            LibDdon.Enemy.Create(EnemyId.Wolf, 5, 46, 2),
        });
    }
}

return new MonsterSpotInfo();
