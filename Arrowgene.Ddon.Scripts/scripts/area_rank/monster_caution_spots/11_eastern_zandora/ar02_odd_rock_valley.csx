/**
 * @brief Enemy Spot in "Eastern Zandora" for "Odd Rock Valley"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(421);
    public override QuestAreaId AreaId => QuestAreaId.EasternZandora;
    public override uint RequiredAreaRank => 2;

    public class NamedParamId
    {
        public const uint FoulEarthFigure = 269; // Foul Earth Figure
        public const uint FoulMeatLump = 270; // Foul Meat Lump
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Sludgeman, 46, 0)
                .SetNamedEnemyParams(NamedParamId.FoulEarthFigure)
                .AddDrop(ItemId.GoldOre, 1, 3, DropRate.UNCOMMON),
            LibDdon.Enemy.CreateAuto(EnemyId.Blob, 46, 1)
                .SetNamedEnemyParams(NamedParamId.FoulMeatLump),
        });
    }
}

return new MonsterSpotInfo();
