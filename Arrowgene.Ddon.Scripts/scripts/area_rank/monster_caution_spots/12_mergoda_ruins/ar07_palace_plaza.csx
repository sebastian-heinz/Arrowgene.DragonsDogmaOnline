/**
 * @brief Enemy Spot in "Mergoda Ruins" for "Palace Plaza"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.MergodaRuinsLowerLevel.AsStageLayoutId(4);
    public override QuestAreaId AreaId => QuestAreaId.MergodaRuins;
    public override uint RequiredAreaRank => 7;

    public class NamedParamId
    {
        public const uint MerganEngineer = 279; // Mergan Engineer
        public const uint GigaMachinaCustom = 280; // Giga Machina Custom
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            // TODO: Named enemy should drop jewlery but no information about what
            LibDdon.Enemy.CreateAuto(EnemyId.GigantMachina, 57, 2, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.GigaMachinaCustom)
                .AddDrop(ItemId.DemonArmingPoints, 1, 3, DropRate.UNCOMMON)
                .AddDrop(ItemId.DarkMetal, 1, 3, DropRate.UNCOMMON),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganElementArcher, 56, 0)
                .SetNamedEnemyParams(NamedParamId.MerganEngineer),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganElementArcher, 56, 1)
                .SetNamedEnemyParams(NamedParamId.MerganEngineer),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganMage, 56, 3)
                .SetNamedEnemyParams(NamedParamId.MerganEngineer),
        });
    }
}

return new MonsterSpotInfo();
