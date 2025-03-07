/**
 * @brief Enemy Spot in "Zandora Wastelands" for "Twin Wheel Valley"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(360);
    public override QuestAreaId AreaId => QuestAreaId.ZandoraWastelands;
    public override uint RequiredAreaRank => 2;

    public class NamedParamId
    {
        public const uint MergodaPatrolCorps = 251; // Mergoda Patrol Corps
        public const uint MergodaFemaleOfficer = 252; // Mergoda Female Officer
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Witch, 47, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.MergodaFemaleOfficer)
                .AddDrop(ItemId.CrestOfGreaterMagick0, 1, 3, DropRate.UNCOMMON),
            LibDdon.Enemy.CreateAuto(EnemyId.AlchemizedSkeleton, 46, 1)
                .SetNamedEnemyParams(NamedParamId.MergodaPatrolCorps),
        });
    }
}

return new MonsterSpotInfo();
