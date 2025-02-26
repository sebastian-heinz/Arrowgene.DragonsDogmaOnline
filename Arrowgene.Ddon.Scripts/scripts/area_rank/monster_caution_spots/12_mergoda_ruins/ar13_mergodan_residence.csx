/**
 * @brief Enemy Spot in "Mergoda Ruins" for "Mergodan Residence"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.MergodanResidence1.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.MergodaRuins;
    public override uint RequiredAreaRank => 13;

    public class NamedParamId
    {
        public const uint Remnant = 890; // Remnant <name>
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.MerganWarrior, 68, 0)
                .SetNamedEnemyParams(NamedParamId.Remnant),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganElementArcher, 68, 1)
                .SetNamedEnemyParams(NamedParamId.Remnant),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganHealer, 68, 2)
                .SetNamedEnemyParams(NamedParamId.Remnant),
        });
    }
}

return new MonsterSpotInfo();
