/**
 * @brief Enemy Spot in "Deenan Woods" for "Sazu Deenan"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(323);
    public override QuestAreaId AreaId => QuestAreaId.DeenanWoods;
    public override uint RequiredAreaRank => 7;

    public class NamedParamId
    {
        public const uint ElfEater = 248; // Elf Eater <name>
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SilverRoar, 45, 2)
                .SetNamedEnemyParams(NamedParamId.ElfEater),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.HighVitalityRing, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BeastLeatherBracelet, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
