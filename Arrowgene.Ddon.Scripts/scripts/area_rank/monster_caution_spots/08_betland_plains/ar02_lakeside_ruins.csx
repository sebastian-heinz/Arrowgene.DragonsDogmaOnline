/**
 * @brief Enemy Spot in "Betland Plains" for "Lakeside Ruins"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(221);
    public override QuestAreaId AreaId => QuestAreaId.BetlandPlains;
    public override uint RequiredAreaRank => 2;

    public class NamedParamId
    {
        public const uint GowanTheColossus = 222; // Gowan the Colossus
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Colossus0, 39, 0)
                .SetNamedEnemyParams(NamedParamId.GowanTheColossus),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.BlackLeatherOverKneeBoots0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.BlackLeatherCirclet0, 1, 1, DropRate.RARE)
            .AddDrop(ItemId.RedGriffinSkin0, 1, 1, DropRate.RARE);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
