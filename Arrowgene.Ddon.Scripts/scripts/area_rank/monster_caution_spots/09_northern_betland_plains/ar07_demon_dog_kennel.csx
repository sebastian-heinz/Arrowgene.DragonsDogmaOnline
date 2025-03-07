/**
 * @brief Enemy Spot in "Northern Betland Plains" for "Demon Dog Kennel"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(310);
    public override QuestAreaId AreaId => QuestAreaId.NorthernBetlandPlains;
    public override uint RequiredAreaRank => 7;

    public class NamedParamId
    {
        public const uint StrengthTrainingWarg = 237; // Strength Training Warg
        public const uint KennelKeeper = 238; // Kennel Keeper
        public const uint DogCatcher = 239; // Dog Catcher
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.CaptainOrc0, 48, 0)
                .SetNamedEnemyParams(NamedParamId.KennelKeeper),
            LibDdon.Enemy.CreateAuto(EnemyId.Grimwarg, 47, 1)
                .SetNamedEnemyParams(NamedParamId.StrengthTrainingWarg),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBringer, 46, 2)
                .SetNamedEnemyParams(NamedParamId.DogCatcher),
        };

        var captainDrops = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.Sandcrystal, 1, 3, DropRate.UNCOMMON)
            .AddDrop(ItemId.PearlMushroom, 1, 3, DropRate.UNCOMMON);
        enemies[0].SetDropsTable(captainDrops);

        var wargDrops = LibDdon.Enemy.GetDropsTable(enemies[1]).Clone()
            .AddDrop(ItemId.Sandcrystal, 1, 3, DropRate.UNCOMMON)
            .AddDrop(ItemId.PearlMushroom, 1, 3, DropRate.UNCOMMON);
        enemies[1].SetDropsTable(wargDrops);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
