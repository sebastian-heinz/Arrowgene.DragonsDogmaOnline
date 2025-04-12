/**
 * @brief Enemy Spot in "Northern Betland Plains" for "Fortress Front: Barded Line"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(283);
    public override QuestAreaId AreaId => QuestAreaId.NorthernBetlandPlains;
    public override uint RequiredAreaRank => 3;

    public class NamedParamId
    {
        public const uint AssaultColossus = 267; // Assault Colossus
    }

    public override void Initialize()
    { 
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Colossus0, 43, 3, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.AssaultColossus),
            LibDdon.Enemy.CreateAuto(EnemyId.Saurian, 35, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.Saurian, 35, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.Saurian, 35, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.Saurian, 35, 4),
        };

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
