/**
 * @brief Enemy Spot in "Mergoda Ruins" for "Mergan Alchemdia"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.MergodaRuinsRoyalPalaceLevel0.AsStageLayoutId(6);
    public override QuestAreaId AreaId => QuestAreaId.MergodaRuins;
    public override uint RequiredAreaRank => 9;

    public class NamedParamId
    {
        public const uint CloneZuhl = 282; // Clone Zuhl
    }

    public override void Initialize()
    {
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.Zuhl0, 60, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.CloneZuhl)
                .AddDrop(ItemId.CrestOfFortitude0, 1, 3, DropRate.UNCOMMON)
                .AddDrop(ItemId.CrestOfStubbornPerseverance0, 1, 3, DropRate.UNCOMMON),
        });
    }
}

return new MonsterSpotInfo();
