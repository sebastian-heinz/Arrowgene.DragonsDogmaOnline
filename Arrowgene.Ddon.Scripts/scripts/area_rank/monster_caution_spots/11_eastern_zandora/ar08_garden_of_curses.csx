/**
 * @brief Enemy Spot in "Eastern Zandora" for "Garden of Curses"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.Lestania.AsStageLayoutId(447);
    public override QuestAreaId AreaId => QuestAreaId.EasternZandora;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint HollowExecutioner = 274; // Hollow Executioner
        public const uint ShrineGuard = 276; // Shrine Guard
    }

    public override void Initialize()
    {
        // TODO: There should be crests dropped from this spot but can't locate any wiki data for it
        // TODO: Found comments talking about ice, shock and fire immunity so added them
        AddEnemies(new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SkullLord, 57, 0, isBoss: true)
                .SetNamedEnemyParams(NamedParamId.HollowExecutioner)
                .AddDrop(ItemId.CrestOfFreezeImmunity0, 1, 3, DropRate.UNCOMMON)
                .AddDrop(ItemId.CrestOfShockImmunity0, 1, 3, DropRate.UNCOMMON)
                .AddDrop(ItemId.CrestOfBurnImmunity0 , 1, 3, DropRate.UNCOMMON),
            LibDdon.Enemy.CreateAuto(EnemyId.GhostMail, 57, 1)
                .SetNamedEnemyParams(NamedParamId.ShrineGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.GhostMail, 57, 2)
                .SetNamedEnemyParams(NamedParamId.ShrineGuard),
            LibDdon.Enemy.CreateAuto(EnemyId.GhostMail, 57, 3)
                .SetNamedEnemyParams(NamedParamId.ShrineGuard),
        });
    }
}

return new MonsterSpotInfo();
