/**
 * @brief Enemy Spot fpr "Hidell Plains" for "Bloodwater Bank"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(28);
    public override QuestAreaId AreaId => QuestAreaId.HidellPlains;
    public override uint RequiredAreaRank => 7;

    public class NamedParamId
    {
        public const uint Variant = 190; // <name> Variant
        public const uint GluttonousGiantSaurian = 191; // Gluttonous Giant Saurian
    }

    public override void Initialize()
    {
        var namedEnemyDropTable = LibDdon.Enemy.GetDropsTable(EnemyId.GiantSaurian, 21).Clone()
            .AddDrop(ItemId.LargeBeastSteak, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.Crystal, 1, 2, DropRate.COMMON)
            .AddDrop(ItemId.SpunYarn, 1, 1, DropRate.UNCOMMON);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.GiantSaurian, 21, 250, 1)
                .SetDropsTable(namedEnemyDropTable)
                .SetNamedEnemyParams(NamedParamId.GluttonousGiantSaurian),
            LibDdon.Enemy.Create(EnemyId.Saurian, 19, 150, 0)
                .SetNamedEnemyParams(NamedParamId.Variant),
            LibDdon.Enemy.Create(EnemyId.Saurian, 19, 150, 2)
                .SetNamedEnemyParams(NamedParamId.Variant),
            LibDdon.Enemy.Create(EnemyId.Saurian, 19, 150, 3)
                .SetNamedEnemyParams(NamedParamId.Variant),
            LibDdon.Enemy.Create(EnemyId.Saurian, 19, 150, 4)
                .SetNamedEnemyParams(NamedParamId.Variant),
        });
    }
}

return new MonsterSpotInfo();
