/**
 * @brief Enemy Spot in "Northern Betland Plains" for "War Demons' Encampment"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(281);
    public override QuestAreaId AreaId => QuestAreaId.NorthernBetlandPlains;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint InfectionResistingWarDemon = 899; // Infection Resisting War Demon
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.OrcBringer, 65, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.InfectedOrcSoldier, 65, 1)
                .SetNamedEnemyParams(NamedParamId.InfectionResistingWarDemon),
        };

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
