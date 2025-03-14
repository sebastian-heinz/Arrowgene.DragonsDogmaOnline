/**
 * @brief Enemy Spot in "Northern Betland Plains" for "Fortress Front: Barded Line"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(282);
    public override QuestAreaId AreaId => QuestAreaId.NorthernBetlandPlains;
    public override uint RequiredAreaRank => 3;

    public class NamedParamId
    {
        public const uint BattleReadyPlatoonLeader = 234; // Battle-Ready Platoon Leader
        public const uint OrcSpy0 = 235; // Orc Spy
        public const uint OrcSpy1 = 236; // Orc Spy
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.GeneralOrc, 43, 0)
                .SetNamedEnemyParams(NamedParamId.BattleReadyPlatoonLeader),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBringer, 43, 1)
                .SetNamedEnemyParams(NamedParamId.OrcSpy0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcTrooper, 43, 2)
                .SetNamedEnemyParams(NamedParamId.OrcSpy1),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.BeastLeatherBracelet, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.DemonsGreaves0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.LeaderLegGuards0, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
