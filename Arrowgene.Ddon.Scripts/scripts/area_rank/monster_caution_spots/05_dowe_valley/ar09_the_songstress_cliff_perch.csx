/**
 * @brief Enemy Spot in "Dowe Valley" for "The Songstress' Cliff Perch"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(208);
    public override QuestAreaId AreaId => QuestAreaId.DoweValley;
    public override uint RequiredAreaRank => 9;

    public class NamedParamId
    {
        public const uint DemonSongtressLorelei = 221; //  Demon Songtress Lorelei
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Sphinx0, 40, 0)
                .SetIsBoss(true)
                .SetNamedEnemyParams(NamedParamId.DemonSongtressLorelei),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.PirateArms0, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
