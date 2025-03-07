/**
 * @brief Enemy Spot in "Mysree Forest" for "The Pig-Eater's Den"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(35);
    public override QuestAreaId AreaId => QuestAreaId.MysreeForest;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint PigEatingApe = 210; // Pig-Eating Ape
    }

    public override void Initialize()
    {
        var dropsTable = LibDdon.Enemy.GetDropsTable(EnemyId.DreadApe, 27).Clone()
            .AddDrop(ItemId.CrookedFang, 1, 3, DropRate.UNCOMMON)
            .AddDrop(ItemId.LargeSimianThickPelt, 1, 3, DropRate.UNCOMMON)
            .AddDrop(ItemId.NecrophagousBristle, 1, 1, DropRate.RARE);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.DreadApe, 27, 1126, 0)
                .SetIsBoss(true)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.PigEatingApe),
        });
    }
}

return new MonsterSpotInfo();
