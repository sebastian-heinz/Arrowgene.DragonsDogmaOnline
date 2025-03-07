/**
 * @brief Enemy Spot in "Volden Mines" for "The Giant Eagle's Treasure Hoard"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(192);
    public override QuestAreaId AreaId => QuestAreaId.VoldenMines;
    public override uint RequiredAreaRank => 9;

    public class NamedParamId
    {
        public const uint TreasureSeekingGriffin = 216; // Treasure Seeking Griffin
        public const uint GraveRobber = 217; // <name> Grave Robber
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Griffin0, 41, 5)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.TreasureSeekingGriffin),
            
            LibDdon.Enemy.CreateAuto(EnemyId.Harpy, 33, 0)
                .SetNamedEnemyParams(NamedParamId.GraveRobber),
            LibDdon.Enemy.CreateAuto(EnemyId.Harpy, 33, 1)
                .SetNamedEnemyParams(NamedParamId.GraveRobber),
            LibDdon.Enemy.CreateAuto(EnemyId.Harpy, 33, 2)
                .SetNamedEnemyParams(NamedParamId.GraveRobber),
            LibDdon.Enemy.CreateAuto(EnemyId.Harpy, 33, 3)
                .SetNamedEnemyParams(NamedParamId.GraveRobber),
            LibDdon.Enemy.CreateAuto(EnemyId.Harpy, 33, 4)
                .SetNamedEnemyParams(NamedParamId.GraveRobber),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.LieutenantsHelm0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.GrandSageCap0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SilverChainmail0, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
