/**
 * @brief Enemy Spot fpr "Hidell Plains" for "Black Wolves Hideout"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(36);
    public override QuestAreaId AreaId => QuestAreaId.HidellPlains;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint BlackWolfRogue = 192; // Black Wolf Rogue
        public const uint BlackWolfLeader = 193; // Black Wolf Leader
    }

    public override void Initialize()
    {
        var namedEnemyDropTable = LibDdon.Enemy.GetDropsTable(EnemyId.RogueFighter, 30).Clone()
            .AddDrop(ItemId.ManAtArmsRing, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.FangWristband, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BottledHaste, 1, 1, DropRate.COMMON)
            .AddDrop(ItemId.CoinPouch10G, 14, 25, DropRate.VERY_COMMON)
            .AddDrop(ItemId.RiftCrystal10Rp, 14, 25, DropRate.VERY_COMMON);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.RogueFighter, 30, 280, 4)
                .SetDropsTable(namedEnemyDropTable)
                .SetHmPresetNo(HmPresetId.RogueFighter)
                .SetNamedEnemyParams(NamedParamId.BlackWolfLeader),
            LibDdon.Enemy.Create(EnemyId.RogueFighter, 25, 230, 0)
                .SetHmPresetNo(HmPresetId.RogueFighter)
                .SetNamedEnemyParams(NamedParamId.BlackWolfRogue),
            LibDdon.Enemy.Create(EnemyId.RogueSeeker, 25, 230, 1)
                .SetHmPresetNo(HmPresetId.RogueSeeker)
                .SetNamedEnemyParams(NamedParamId.BlackWolfRogue),
            LibDdon.Enemy.Create(EnemyId.RogueSeeker, 25, 230, 2)
                .SetHmPresetNo(HmPresetId.RogueSeeker)
                .SetNamedEnemyParams(NamedParamId.BlackWolfRogue),
            LibDdon.Enemy.Create(EnemyId.RogueMage, 25, 230, 3)
                .SetHmPresetNo(HmPresetId.RogueMage)
                .SetNamedEnemyParams(NamedParamId.BlackWolfRogue),
            LibDdon.Enemy.Create(EnemyId.RogueMage, 25, 230, 5)
                .SetHmPresetNo(HmPresetId.RogueMage)
                .SetNamedEnemyParams(NamedParamId.BlackWolfRogue),
        });
    }
}

return new MonsterSpotInfo();
