/**
 * @brief Enemy Spot in "Volden Mines" for "Mountainside Battlefield"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(188);
    public override QuestAreaId AreaId => QuestAreaId.VoldenMines;
    public override uint RequiredAreaRank => 6;

    public class NamedParamId
    {
        public const uint TunnelThief = 215; // Tunnel Thief
    }

    public override void Initialize()
    {
        var dropsTable = LibDdon.Enemy.GetDropsTable(EnemyId.Ogre, 32).Clone()
            .AddDrop(ItemId.VitalWristband, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.OilResistantEarrings, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.SolidFangEarrings, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BattlerBracelet, 1, 1, DropRate.RARE);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Ogre, 32, 1486, 0)
                .SetIsBoss(true)
                .SetDropsTable(dropsTable)
                .SetNamedEnemyParams(NamedParamId.TunnelThief),
        });
    }
}

return new MonsterSpotInfo();
