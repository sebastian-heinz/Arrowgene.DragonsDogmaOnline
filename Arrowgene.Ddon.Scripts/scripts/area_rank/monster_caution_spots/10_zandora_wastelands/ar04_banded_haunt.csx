/**
 * @brief Enemy Spot in "Zandora Wastelands" for "Banded Haunt"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(360);
    public override QuestAreaId AreaId => QuestAreaId.ZandoraWastelands;
    public override uint RequiredAreaRank => 4;

    public class NamedParamId
    {
        public const uint RustedIronGiantWarrior = 249; // Rusted Iron Giant Warrior
        public const uint RustedAlchemizedGoblin = 250; // Rusted Alchemized Goblin
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.DamnedGolem, 49, 2, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.RustedIronGiantWarrior),
            LibDdon.Enemy.CreateAuto(EnemyId.DamnedGoblin, 49, 0)
                .SetNamedEnemyParams(NamedParamId.RustedAlchemizedGoblin),
            LibDdon.Enemy.CreateAuto(EnemyId.DamnedSlingGoblinFlask, 49, 1)
                .SetNamedEnemyParams(NamedParamId.RustedAlchemizedGoblin),
        });
    }
}

return new MonsterSpotInfo();
