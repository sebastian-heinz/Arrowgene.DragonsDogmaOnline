/**
 * @brief Enemy Spot in "Breya Coast" for "Deserted Village of Ruflow"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(105);
    public override QuestAreaId AreaId => QuestAreaId.BreyaCoast;
    public override uint RequiredAreaRank => 7;

    public class NamedParamId
    {
        public const uint FormerVillager = 204; // Former Villager
        public const uint SpecterOfRuflow = 203; //  Specter of Ruflow
    }

    public override void Initialize()
    {
        var namedEnemyDrops = LibDdon.Enemy.GetDropsTable(EnemyId.Wight0, 23).Clone()
            .AddDrop(ItemId.SorceryBangles0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.ProdigysShoes0, 1, 1, DropRate.RARE);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Wight0, 23, 874, 0)
                .SetIsBoss(true)
                .SetScale(115)
                .SetDropsTable(namedEnemyDrops)
                .SetNamedEnemyParams(NamedParamId.SpecterOfRuflow)
                .SetSpawnTime(GameTimeManager.NightTime),
        });
    }
}

return new MonsterSpotInfo();
