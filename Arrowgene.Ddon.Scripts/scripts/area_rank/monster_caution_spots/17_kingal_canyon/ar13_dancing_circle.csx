#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.KingalCanyon.AsStageLayoutId(44);
    public override QuestAreaId AreaId => QuestAreaId.KingalCanyon;
    public override uint RequiredAreaRank => 13;

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.SeverelyInfectedDemon, 78, 0),
            LibDdon.Enemy.CreateAuto(EnemyId.SeverelyInfectedDemon, 78, 1),
            LibDdon.Enemy.CreateAuto(EnemyId.SeverelyInfectedDemon, 78, 2),
            LibDdon.Enemy.CreateAuto(EnemyId.SeverelyInfectedDemon, 78, 3),
            LibDdon.Enemy.CreateAuto(EnemyId.SeverelyInfectedDemon, 78, 4),
            LibDdon.Enemy.CreateAuto(EnemyId.SeverelyInfectedDemon, 78, 5)
        };

        foreach (var enemy in enemies)
        {
            var dropsTable = LibDdon.Enemy.GetDropsTable(enemy).Clone()
                .AddDrop(ItemId.JetBlackPelt, 1, 2, DropRate.COMMON)
                .AddDrop(ItemId.JetBlackFur, 1, 3, DropRate.COMMON)
                .AddDrop(ItemId.Kingalite, 1, 2, DropRate.UNCOMMON);
            enemy.SetDropsTable(dropsTable);
        }

        AddEnemies(enemies);
    }
}


return new MonsterSpotInfo();
