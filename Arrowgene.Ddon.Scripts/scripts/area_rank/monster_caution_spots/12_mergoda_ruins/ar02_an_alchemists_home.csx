/**
 * @brief Enemy Spot in "Mergoda Ruins" for "An Alchemists Home"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.AnAlchemistsHome.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.MergodaRuins;
    public override uint RequiredAreaRank => 2;

    public class NamedParamId
    {
        public const uint HieronymusTheAlchemist = 384; // Hieronymus the Alchemist
        public const uint HighPurityStoneEye = 385; // High Purity Stone Eye
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.MerganElementArcher, 53, 0)
                .SetNamedEnemyParams(NamedParamId.HieronymusTheAlchemist),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganHealer, 51, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.CrystalEye, 51, 2)
                .SetNamedEnemyParams(NamedParamId.HighPurityStoneEye),
            LibDdon.Enemy.CreateAuto(EnemyId.CrystalEye, 51, 3)
                .SetNamedEnemyParams(NamedParamId.HighPurityStoneEye),
        });
    }
}

return new MonsterSpotInfo();
