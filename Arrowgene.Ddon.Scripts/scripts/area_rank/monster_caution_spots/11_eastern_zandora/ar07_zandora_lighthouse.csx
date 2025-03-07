/**
 * @brief Enemy Spot in "Eastern Zandora" for "Zandora Lighthouse"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(467);
    public override QuestAreaId AreaId => QuestAreaId.EasternZandora;
    public override uint RequiredAreaRank => 7;

    public class NamedParamId
    {
        public const uint Tamed = 272; // Tamed <name>
        public const uint TamerBarbarian = 273; // Tamer Barbarian
    }

    public override void Initialize()
    {
        // TODO: Drops for cockatrice are made up, unable to locate source which
        // TODO: details which crests the enemies here should reward.
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Cockatrice, 56, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.Tamed)
                .AddDrop(ItemId.CrestOfPetrification0, 1, 3, DropRate.UNCOMMON)
                .AddDrop(ItemId.CrestOfPetrificationPrevention0, 1, 3, DropRate.UNCOMMON),
            LibDdon.Enemy.CreateAuto(EnemyId.BandedWarrior, 55, 1)
                .SetNamedEnemyParams(NamedParamId.TamerBarbarian),
            LibDdon.Enemy.CreateAuto(EnemyId.BandedHunter, 55, 2)
                .SetNamedEnemyParams(NamedParamId.TamerBarbarian),
            LibDdon.Enemy.CreateAuto(EnemyId.BandedHunter, 55, 3)
                .SetNamedEnemyParams(NamedParamId.TamerBarbarian),
            LibDdon.Enemy.CreateAuto(EnemyId.BandedMage, 55, 4)
                .SetNamedEnemyParams(NamedParamId.TamerBarbarian),
            LibDdon.Enemy.CreateAuto(EnemyId.BandedMage, 55, 5)
                .SetNamedEnemyParams(NamedParamId.TamerBarbarian),
        });
    }
}

return new MonsterSpotInfo();
