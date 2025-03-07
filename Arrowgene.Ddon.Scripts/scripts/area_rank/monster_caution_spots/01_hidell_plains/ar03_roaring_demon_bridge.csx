/**
 * @brief Enemy Spot in "Hidell Plains" for "Roaring Demon Bridge"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(29);
    public override QuestAreaId AreaId => QuestAreaId.HidellPlains;
    public override uint RequiredAreaRank => 3;

    public class NamedParamId
    {
        public const uint BinshaTheBridgeKeeper = 189; // Binsha the Bridge-Keeper
        public const uint BinshasHenchman = 188; // Binsha's Henchman
    }

    public override void Initialize()
    {
        var namedEnemyDropTable = LibDdon.Enemy.GetDropsTable(EnemyId.GoblinLeader, 10).Clone()
            .AddDrop(ItemId.ClericsCap0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.TigerBangles1, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.HardKnuckles0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BasicCape0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BronzeGuard0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BronzeBracers0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BronzeHelm0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.LeatherHood0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.CoinPouch10G, 5, 10, DropRate.VERY_COMMON)
            .AddDrop(ItemId.RiftCrystal10Rp, 5, 10, DropRate.VERY_COMMON);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.GoblinLeader, 10, 100, 3)
                .SetDropsTable(namedEnemyDropTable)
                .SetNamedEnemyParams(NamedParamId.BinshaTheBridgeKeeper),
            LibDdon.Enemy.Create(EnemyId.Goblin, 8, 80, 1)
                .SetNamedEnemyParams(NamedParamId.BinshasHenchman),
            LibDdon.Enemy.Create(EnemyId.SlingGoblinRock, 8, 80, 2)
                .SetNamedEnemyParams(NamedParamId.BinshasHenchman),
            LibDdon.Enemy.Create(EnemyId.Goblin, 8, 80, 4)
                .SetNamedEnemyParams(NamedParamId.BinshasHenchman),
        });
    }
}

return new MonsterSpotInfo();
