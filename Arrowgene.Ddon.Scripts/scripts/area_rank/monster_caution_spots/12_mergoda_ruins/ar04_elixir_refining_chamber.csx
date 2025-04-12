/**
 * @brief Enemy Spot in "Mergoda Ruins" for "Elixir Refining Chamber"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.ElixirRefiningChamber.AsStageLayoutId(1);
    public override QuestAreaId AreaId => QuestAreaId.MergodaRuins;
    public override uint RequiredAreaRank => 4;

    public class NamedParamId
    {
        public const uint ImmortalDrugTestSubject = 381; // Immortal Drug Test Subject
        public const uint SergiusTheApothecary = 386; // Sergius the Apothecary
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            // TODO: Named enemy should drop crests but no information about what crests should drop
            LibDdon.Enemy.CreateAuto(EnemyId.MerganMage, 55, 0)
                .SetNamedEnemyParams(NamedParamId.SergiusTheApothecary),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganWarrior, 53, 1)
                .SetNamedEnemyParams(NamedParamId.ImmortalDrugTestSubject),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganDefender, 53, 2)
                .SetNamedEnemyParams(NamedParamId.ImmortalDrugTestSubject),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganHunter, 53, 3)
                .SetNamedEnemyParams(NamedParamId.ImmortalDrugTestSubject),
            LibDdon.Enemy.CreateAuto(EnemyId.MerganHealer, 53, 4)
                .SetNamedEnemyParams(NamedParamId.ImmortalDrugTestSubject),
        });
    }
}

return new MonsterSpotInfo();
