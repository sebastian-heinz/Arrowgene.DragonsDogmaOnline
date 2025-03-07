/**
 * @brief Enemy Spot in "Breya Coast" for "Blue Claw Beach"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(103);
    public override QuestAreaId AreaId => QuestAreaId.BreyaCoast;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint BlueClawBeach = 205; // Blue Claw Beach
    }

    public override void Initialize()
    {
        var namedEnemyDrops = LibDdon.Enemy.GetDropsTable(EnemyId.ArmoredCyclops, 27).Clone()
            .AddDrop(ItemId.LibertyBow0, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.Daciaensis0, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.BattleLegs0, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.RuggedLeather, 1, 1, DropRate.UNCOMMON);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.ArmoredCyclops, 27, 1126, 0)
                .SetIsBoss(true)
                .SetScale(135)
                .SetDropsTable(namedEnemyDrops)
                .SetNamedEnemyParams(NamedParamId.BlueClawBeach)
        });
    }
}

return new MonsterSpotInfo();
