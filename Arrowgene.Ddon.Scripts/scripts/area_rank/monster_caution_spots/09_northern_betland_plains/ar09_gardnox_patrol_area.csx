/**
 * @brief Enemy Spot in "Northern Betland Plains" for "Gardnox Patrol Area"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(287);
    public override QuestAreaId AreaId => QuestAreaId.NorthernBetlandPlains;
    public override uint RequiredAreaRank => 9;

    public class NamedParamId
    {
        public const uint BifangMilitaryDragonBehemoth = 240; // Bifang Military Dragon Behemoth
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Behemoth0, 52, 3)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.BifangMilitaryDragonBehemoth),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 40, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 40, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 40, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 40, 4),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 40, 5),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.CrestOfIncineration0, 1, 3, DropRate.UNCOMMON)
            .AddDrop(ItemId.CrestOfBurnPrevention, 1, 3, DropRate.UNCOMMON);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
