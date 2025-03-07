/**
 * @brief Enemy Spot in "Mysree Forest" for "Forest Goblins Village"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(58);
    public override QuestAreaId AreaId => QuestAreaId.MysreeForest;
    public override uint RequiredAreaRank => 2;

    public class NamedParamId
    {
        public const uint GuriggasMinion = 206; // Gurigga's Minion
        public const uint GuriggatheGreenDemon = 207;  // Gurigga the Green Demon
    }

    public override void Initialize()
    {
        var dropsTable = LibDdon.Enemy.GetDropsTable(EnemyId.ForestGoblinFighter, 14).Clone()
            .AddDrop(ItemId.LeafSignetRing, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GrassSignetRing, 1, 1, DropRate.RARE);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.ForestGoblinFighter, 14, 427, 3)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.GuriggatheGreenDemon),
            LibDdon.Enemy.Create(EnemyId.ForestGoblin, 12, 224, 0)
                .SetNamedEnemyParams(NamedParamId.GuriggasMinion),
            LibDdon.Enemy.Create(EnemyId.ForestGoblin, 12, 224, 1)
                .SetNamedEnemyParams(NamedParamId.GuriggasMinion),
            LibDdon.Enemy.Create(EnemyId.ForestGoblin, 12, 224, 2)
                .SetNamedEnemyParams(NamedParamId.GuriggasMinion),
            LibDdon.Enemy.Create(EnemyId.ForestGoblin, 12, 224, 4)
                .SetNamedEnemyParams(NamedParamId.GuriggasMinion),
        });
    }
}

return new MonsterSpotInfo();
