/**
 * @brief Enemy Spot in "Zandora Wastelands" for "Fanatic's Hiding Ground"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(364);
    public override QuestAreaId AreaId => QuestAreaId.ZandoraWastelands;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint BlackSeaEagleOfTheRuinedSite = 901; // Black Sea Eagle of the Ruined Site
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.BlackGriffin0, 70, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.BlackSeaEagleOfTheRuinedSite)
        });
    }
}

return new MonsterSpotInfo();
