/**
 * @brief Enemy Spot in "Eastern Zandora" for "East Zandora Ruins"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(461);
    public override QuestAreaId AreaId => QuestAreaId.EasternZandora;
    public override uint RequiredAreaRank => 4;

    public class NamedParamId
    {
        public const uint TorridGeoGolem = 271; // Torrid Geo Golem
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.GeoGolem, 47, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.TorridGeoGolem)
        });
    }
}

return new MonsterSpotInfo();
