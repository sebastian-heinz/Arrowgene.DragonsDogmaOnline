/**
 * @brief Enemy Spot in "Mysree Forest" for "Peddler's Peril"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(122);
    public override QuestAreaId AreaId => QuestAreaId.MysreeForest;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint MaritiatheKidnapper = 893; //  Maritia the Kidnapper
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.Create(EnemyId.Nightmare, 70, 60700, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.MaritiatheKidnapper),
        });
    }
}

return new MonsterSpotInfo();
