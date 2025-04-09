/**
 * @brief Enemy Spot in "Volden Mines" for "Bandit Hideaway"
 */

#load "libs.csx"

public class MonsterSpotInfo : IMonsterSpotInfo
{
    public override StageLayoutId StageLayoutId => Stage.BanditHideaway.AsStageLayoutId(7);
    public override QuestAreaId AreaId => QuestAreaId.VoldenMines;
    public override uint RequiredAreaRank => 8;

    public class NamedParamId
    {
        public const uint BadgerWolfTamer = 312; // Badger Wolf Tamer
        public const uint TamedDirewolf = 313; // Tamed Direwolf
        public const uint BadgerChieftain = 327;// Badger Chieftain
        public const uint BadgerMember = 328; // Badger Member
    }

    public override void Initialize()
    {
        var enemies = new List<InstancedEnemy>()
        {
            LibDdon.Enemy.CreateAuto(EnemyId.RogueHunter, 35, 0)
                .SetNamedEnemyParams(NamedParamId.BadgerWolfTamer),
            LibDdon.Enemy.CreateAuto(EnemyId.Direwolf, 34, 5)
                .SetNamedEnemyParams(NamedParamId.TamedDirewolf),
            
            LibDdon.Enemy.CreateAuto(EnemyId.RogueSeeker, 35, 1)
                .SetNamedEnemyParams(NamedParamId.BadgerMember),
            LibDdon.Enemy.CreateAuto(EnemyId.RogueDefender, 35, 2)
                .SetNamedEnemyParams(NamedParamId.BadgerMember),
            LibDdon.Enemy.CreateAuto(EnemyId.RogueHealer, 35, 3)
                .SetNamedEnemyParams(NamedParamId.BadgerMember),
            LibDdon.Enemy.CreateAuto(EnemyId.RogueFighter, 35, 4)
                .SetNamedEnemyParams(NamedParamId.BadgerMember),
        };

        var dropsTable = LibDdon.Enemy.GetDropsTable(enemies[0]).Clone()
            .AddDrop(ItemId.CoinPouch10G, 3, 17, DropRate.VERY_COMMON)
            .AddDrop(ItemId.Lockpick, 2, 9, DropRate.UNCOMMON);
        enemies[0].SetDropsTable(dropsTable);

        AddEnemies(enemies);
    }
}

return new MonsterSpotInfo();
