/**
 * @brief Enemy Spot in "Volden Mines" for "Southern Wall Fissure"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.SouthernWallFissure.AsStageLayoutId(16);
    public override QuestAreaId AreaId => QuestAreaId.VoldenMines;
    public override uint RequiredAreaRank => 2;

    public override void Initialize()
    {
        var dropsTable = LibDdon.Enemy.GetDropsTable(EnemyId.Ogre, 26).Clone()
            .AddDrop(ItemId.NecrophagousBristle, 1, 2, DropRate.UNCOMMON);

        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Ogre, 26, 1060, 0)
                .SetIsBoss(true)
                .SetDropsTable(dropsTable),
        });
    }
}

return new MonsterSpotInfo();
