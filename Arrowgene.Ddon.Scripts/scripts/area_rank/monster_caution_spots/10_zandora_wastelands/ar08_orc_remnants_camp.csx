/**
 * @brief Enemy Spot in "Zandora Wastelands" for "Orc Remnants Camp"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(424);
    public override QuestAreaId AreaId => QuestAreaId.ZandoraWastelands;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint OrcGeneralRemnant = 260; // Orc General Remnant
        public const uint OrcRemnants0 = 261; // Orc Remnants
        public const uint OrcRemnants1 = 262; // Orc Remnants
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.GeneralOrc, 56, 3)
                .SetNamedEnemyParams(NamedParamId.OrcRemnants1)
                .AddDrop(ItemId.RousingSteak, 1, 3, DropRate.UNCOMMON),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBattler, 56, 0)
                .SetNamedEnemyParams(NamedParamId.OrcRemnants0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBattler, 56, 1)
                .SetNamedEnemyParams(NamedParamId.OrcRemnants0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBattler, 56, 2)
                .SetNamedEnemyParams(NamedParamId.OrcRemnants0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBattler, 56, 4)
                .SetNamedEnemyParams(NamedParamId.OrcRemnants0),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcTrooper, 56, 5)
                .SetNamedEnemyParams(NamedParamId.OrcRemnants1),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcTrooper, 56, 6)
                .SetNamedEnemyParams(NamedParamId.OrcRemnants1),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcTrooper, 56, 8)
                .SetNamedEnemyParams(NamedParamId.OrcRemnants1),
            LibDdon.Enemy.CreateAuto(EnemyId.OrcTrooper, 56, 9)
                .SetNamedEnemyParams(NamedParamId.OrcRemnants1),
        });
    }
}

return new MonsterSpotInfo();
