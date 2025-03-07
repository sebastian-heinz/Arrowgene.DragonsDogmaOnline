/**
 * @brief Enemy Spot in "Mysree Grove" for "The Giants' Bed"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(138);
    public override QuestAreaId AreaId => QuestAreaId.MysreeGrove;
    public override uint RequiredAreaRank => 9;

    public class NamedParamId
    {
        public const uint JotunTheReturned = 232; // Jotun the Returned
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.MoleTroll0, 43, 2)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.JotunTheReturned),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.JunglePants0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DeepForestMantle0, 1, 2, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
