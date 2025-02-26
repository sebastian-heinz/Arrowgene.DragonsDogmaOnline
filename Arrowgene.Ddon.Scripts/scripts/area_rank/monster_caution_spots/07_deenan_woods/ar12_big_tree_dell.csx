/**
 * @brief Enemy Spot in "Deenan Woods" for "Big Tree Dell"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(343);
    public override QuestAreaId AreaId => QuestAreaId.DeenanWoods;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint Heretic = 898; // Heretic <name>
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Ent, 68, 0)
                .SetNamedEnemyParams(NamedParamId.Heretic),
        };

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
