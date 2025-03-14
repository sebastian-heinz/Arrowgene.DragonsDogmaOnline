/**
 * @brief Enemy Spot in "Betland Plains" for "Lake Cera Ship Graveyard"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(265);
    public override QuestAreaId AreaId => QuestAreaId.BetlandPlains;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint WaterDragonOfCellarLake = 226; // Water Dragon of Cellar Lake
        public const uint LakeNewt = 225; // Lake Newt;
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Lindwurm0, 43, 1)
                .SetNamedEnemyParams(NamedParamId.WaterDragonOfCellarLake),
            LibDdon.Enemy.CreateAuto(EnemyId.BlueNewt, 43, 0)
                .SetNamedEnemyParams(NamedParamId.LakeNewt),
            LibDdon.Enemy.CreateAuto(EnemyId.BlueNewt, 43, 2)
                .SetNamedEnemyParams(NamedParamId.LakeNewt),
            LibDdon.Enemy.CreateAuto(EnemyId.BlueNewt, 43, 3)
                .SetNamedEnemyParams(NamedParamId.LakeNewt),
            LibDdon.Enemy.CreateAuto(EnemyId.BlueNewt, 43, 4)
                .SetNamedEnemyParams(NamedParamId.LakeNewt),
            LibDdon.Enemy.CreateAuto(EnemyId.BlueNewt, 43, 5)
                .SetNamedEnemyParams(NamedParamId.LakeNewt),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.MithrilIngot, 1, 3, DropRate.UNCOMMON)
            .AddDrop(ItemId.BlowResistantCloth, 1, 2, DropRate.UNCOMMON);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
