/**
 * @brief Enemy Spot in "Zandora Wastelands" for "South Mergoda Ruins"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(410);
    public override QuestAreaId AreaId => QuestAreaId.ZandoraWastelands;
    public override uint RequiredAreaRank => 5;

    public class NamedParamId
    {
        public const uint ZandoraBarbarian0 = 253; // Zandora Barbarian (Fighter)
        public const uint ZandoraBarbarian1 = 254; // Zandora Barbarian (Seeker)
        public const uint ZandoraBarbarian2 = 255; // Zandora Barbarian (Hunter)
        public const uint ZandoraBarbarian3 = 256; // Zandora Barbarian (Healer)
        public const uint ZandoraBarbarian4 = 257; // Zandora Barbarian (Defender)
        public const uint ZandoraBarbarian5 = 258; // Zandora Barbarian (Mage)
        public const uint ZandoraBarbarian6 = 259; // Zandora Barbarian (Warrior)
    }

    public override void Initialize()
    {
        // TODO: Unable to locate what jewlery these enemies should drop
        // TODO: Guessed based on item levels, but might need to be updated
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.BandedHunter, 48, 2)
                .SetNamedEnemyParams(NamedParamId.ZandoraBarbarian2)
                .AddDrop(ItemId.HighVitalityRing, 1, 1, DropRate.RARE)
                .AddDrop(ItemId.SacredTreeRing, 1, 1, DropRate.VERY_RARE),
            LibDdon.Enemy.CreateAuto(EnemyId.BandedFighter, 48, 0)
                .SetNamedEnemyParams(NamedParamId.ZandoraBarbarian0)
                .AddDrop(ItemId.HighVitalityRing, 1, 1, DropRate.RARE)
                .AddDrop(ItemId.SacredTreeRing, 1, 1, DropRate.VERY_RARE),
            LibDdon.Enemy.CreateAuto(EnemyId.BandedMage, 48, 1)
                .SetNamedEnemyParams(NamedParamId.ZandoraBarbarian5)
                .AddDrop(ItemId.HighVitalityRing, 1, 1, DropRate.RARE)
                .AddDrop(ItemId.SacredTreeRing, 1, 1, DropRate.VERY_RARE),
        });
    }
}

return new MonsterSpotInfo();
