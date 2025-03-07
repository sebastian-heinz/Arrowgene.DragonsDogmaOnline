/**
 * @brief Enemy Spot in "Mysree Forest" for "Ancient Place of Rituals"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.AncientPlaceofRituals0.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.MysreeForest;
    public override uint RequiredAreaRank => 4;

    public class NamedParamId
    {
        public const uint OldGrownEnt = 212; // Old Grown Ent
        public const uint Ancient = 213; // Ancient <name>
    }

    public override void Initialize()
    {
        var dropsTable = LibDdon.Enemy.GetDropsTable(EnemyId.Ent, 18).Clone()
            .AddDrop(ItemId.CopperMail0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.WonderCoat0, 1, 1, DropRate.RARE);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Ent, 18, 604, 3)
                .SetIsBoss(true)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.OldGrownEnt),

            LibDdon.Enemy.Create(EnemyId.SlingForestGoblin, 16, 310, 0)
                .SetNamedEnemyParams(NamedParamId.Ancient),
            LibDdon.Enemy.Create(EnemyId.SlingForestGoblin, 16, 310, 1)
                .SetNamedEnemyParams(NamedParamId.Ancient),
            LibDdon.Enemy.Create(EnemyId.SlingForestGoblin, 16, 310, 2)
                .SetNamedEnemyParams(NamedParamId.Ancient),
        });
    }
}

return new MonsterSpotInfo();
