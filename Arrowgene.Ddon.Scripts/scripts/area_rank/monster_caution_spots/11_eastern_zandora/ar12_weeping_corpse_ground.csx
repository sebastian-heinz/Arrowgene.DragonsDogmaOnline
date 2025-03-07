/**
 * @brief Enemy Spot in "Eastern Zandora" for "Weeping Corpse Ground"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(458);
    public override QuestAreaId AreaId => QuestAreaId.EasternZandora;
    public override uint RequiredAreaRank => 12;

    public class NamedParamId
    {
        public const uint HollowKnight = 900; // Hollow Knight
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.GhostMail, 65, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.HollowKnight),
            LibDdon.Enemy.CreateAuto(EnemyId.GhostMail, 65, 1, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.HollowKnight)
        });
    }
}

return new MonsterSpotInfo();
