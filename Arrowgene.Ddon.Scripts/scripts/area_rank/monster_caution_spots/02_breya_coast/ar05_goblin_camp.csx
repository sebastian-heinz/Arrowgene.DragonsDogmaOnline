/**
 * @brief Enemy Spot in "Breya Coast" for "Goblin Camp"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(96);
    public override QuestAreaId AreaId => QuestAreaId.BreyaCoast;
    public override uint RequiredAreaRank => 5;

    public class NamedParamId
    {
        public const uint GyumulTheSpikedHorn = 201; // Gyumul the Spiked Horn
        public const uint GyumulsMinion = 202; // Gyumul's Minion
    }

    private InstancedEnemy CreateRandomHobgoblin(ushort lv, uint exp, byte index)
    {
        return LibDdon.Enemy.CreateRandom(lv, exp, index, new List<EnemyId>() {
            EnemyId.Hobgoblin,
            EnemyId.HobgoblinFighter,
            EnemyId.SlingHobgoblinTorch,
            EnemyId.SlingHobgoblinOilFlask,
        });
    }

    public override void Initialize()
    {
        var namedEnemyDrops = LibDdon.Enemy.GetDropsTable(EnemyId.HobgoblinLeader, 20).Clone()
            .AddDrop(ItemId.BronzeScale0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.HematiteSand, 1, 3, DropRate.VERY_COMMON)
            .AddDrop(ItemId.LeatherCord, 1, 1, DropRate.UNCOMMON)
            .AddDrop(ItemId.RedLumber, 1, 1, DropRate.UNCOMMON);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.HobgoblinLeader, 20, 706, 2)
                .SetDropsTable(namedEnemyDrops)
                .SetNamedEnemyParams(NamedParamId.GyumulTheSpikedHorn),
            CreateRandomHobgoblin(18, 510, 0)
                .SetNamedEnemyParams(NamedParamId.GyumulsMinion),
            CreateRandomHobgoblin(18, 510, 1)
                .SetNamedEnemyParams(NamedParamId.GyumulsMinion),
            CreateRandomHobgoblin(18, 510, 3)
                .SetNamedEnemyParams(NamedParamId.GyumulsMinion),
            CreateRandomHobgoblin(18, 510, 4)
                .SetNamedEnemyParams(NamedParamId.GyumulsMinion),
            CreateRandomHobgoblin(18, 510, 5)
                .SetNamedEnemyParams(NamedParamId.GyumulsMinion),
            CreateRandomHobgoblin(18, 510, 6)
                .SetNamedEnemyParams(NamedParamId.GyumulsMinion),
        });
    }
}

return new MonsterSpotInfo();
