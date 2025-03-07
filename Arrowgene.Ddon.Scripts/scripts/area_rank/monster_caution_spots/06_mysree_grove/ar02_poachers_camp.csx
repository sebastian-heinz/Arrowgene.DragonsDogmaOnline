/**
 * @brief Enemy Spot in "Mysree Grove" for "Poachers' Camp"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(132);
    public override QuestAreaId AreaId => QuestAreaId.MysreeGrove;
    public override uint RequiredAreaRank => 2;

    public class NamedParamId
    {
        public const uint ShadowPoacher = 227; //  Shadow Poacher
        public const uint PoacherGuard = 228; //  Poacher Guard
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.RogueHunter, 34, 3)
                .SetNamedEnemyParams(NamedParamId.ShadowPoacher),

            LibDdon.Enemy.CreateAuto(EnemyId.RogueHunter, 33, 0)
                .SetNamedEnemyParams(NamedParamId.PoacherGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.RogueHunter, 33, 1)
                .SetNamedEnemyParams(NamedParamId.PoacherGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.RogueHunter, 33, 2)
                .SetNamedEnemyParams(NamedParamId.PoacherGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.RogueFighter, 33, 4)
                .SetNamedEnemyParams(NamedParamId.PoacherGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.RogueFighter, 33, 5)
                .SetNamedEnemyParams(NamedParamId.PoacherGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.RogueSeeker, 33, 6)
                .SetNamedEnemyParams(NamedParamId.PoacherGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.RogueSeeker, 33, 7)
                .SetNamedEnemyParams(NamedParamId.PoacherGuard),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.IceGemChoker, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.LightningGemChoker, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
