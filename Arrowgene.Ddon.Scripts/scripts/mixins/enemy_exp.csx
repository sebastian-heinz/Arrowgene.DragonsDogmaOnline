/**
 * @breif This mixin is used to calculate the exp amount for a given
 * enemy based on observations from realworld exp data. It should be
 * possible to call this script directly from the server code or from
 * other scripts.
 */

#load "libs.csx"

// Data-points
//  LV, EXP,        EnemyType, IsBoss, Player Level, Nameplate
//   1,   6,           Rabbit,  False
//   1,   8,               Ox,  False
//   1,  12,       Killer Bee,  False
//   1,  20,           Goblin,  False
//   1,  20,           Goblin,  False
//   1,  19,           Goblin,  False, 8
//   1,  24,           Undead,  False, , "Flocking Undead"
//   2,  12,       Killer Bee,  False
//   2,  23,   Goblin Fighter,  False
//   2,  23,           Goblin,  False
//   2,  26,           Undead,  False, , "Flocking Undead"
//   3,  26,             Wolf,  False
//   3,  24,             Wolf,  False, 11
//   3,  23,             Wolf,  False, 12
//   3,  32,    Rouge Fighter,  False
//   3,  29,    Rouge Fighter,  False
//   3,  32,     Rouge Healer,  False
//   3,  10,           Spider,  False
//   3,  9,            Spider,  False
//   3,  28,    Skeleton Mage,  False
//   3,  25,         Skeleton,  False
//   3,  22,         Skeleton,  False, 12
//   3,  28,           Undead,  False
//   3,  25,           Undead,  False, 12
//   3,  26,           Goblin,  False
//   3,  26,   Goblin Fighter,  False
//   3,  13,       Killer Bee,  False
//   4,  34,    Goblin Leader,  False
//   4,  27,    Forest Goblin,  False
//   5,  41,     Pawn Fighter,  False, , "Lost Pawn"
//   5,  38,     Pawn Fighter,  False, 12, "Lost Pawn",
//   5,  35,          Saurian,  False
//   5,  32,     Sling Goblin,  False
//   8,  50,   Redcap Slinger,  False, , "Vigilant Sling Redcap"
//   8,  50,   Redcap Fighter,  False, , "Vigilant Redcap Fighter"
//  11,  80,      Orc Soldier,  False

public class Mixin : IExpMixin
{
    private HashSet<uint> AutomaticExpQuestExceptions = new HashSet<uint>()
    {
        (uint) QuestId.ResolutionsAndOmens
    };

    public override uint GetExpValue(CharacterCommon characterCommon, InstancedEnemy enemy)
    {
        uint result = 0;

        if (AutomaticExpQuestExceptions.Contains(enemy.QuestScheduleId))
        {
            return 0;
        }

        var scheme = enemy.ExpScheme;
        if (StageManager.IsBitterBlackMazeStageId(enemy.StageLayoutId))
        {
            // TODO: Change to special BBM scheme
            scheme = EnemyExpScheme.Tool;
        }
        else if (enemy.QuestScheduleId != 0 && QuestManager.IsExmQuest(enemy.QuestScheduleId))
        {
            scheme = EnemyExpScheme.Exm;
        }

        switch (scheme)
        {
            case EnemyExpScheme.Automatic:
                result = GetAutomaticExpCalculation(characterCommon, enemy);
                break;
            case EnemyExpScheme.Exm:
                // Enemies in an EXM should reward 0 EXP
                result = 0;
                break;
            default:
                result = enemy.GetDroppedExperience();
                break;
        }

        return result;
    }

    /// <summary>
    /// Basic exp algorithm based on player level, enemy level, if the enemy is a boss and if the enemy is from a quest or not.
    /// </summary>
    /// <param name="characterCommon"></param>
    /// <param name="enemy"></param>
    /// <returns></returns>
    private uint GetAutomaticExpCalculation(CharacterCommon characterCommon, InstancedEnemy enemy)
    {
        int enemyLevel = enemy.Lv;
        int playerLevel = (int) characterCommon.ActiveCharacterJobData.Lv;

        int levelDiff = enemyLevel - playerLevel;

        int baseXP;
        if (enemyLevel <= 30) {
            baseXP = enemyLevel * 15 + 15;
        } else if (enemyLevel <= 50) {
            baseXP = enemyLevel * 30 + 300;
        } else if (enemyLevel <= 80) {
            baseXP = enemyLevel * 50 + 1000;
        } else if (enemyLevel <= 100) {
            baseXP = enemyLevel * 80 + 2000;
        } else {
            baseXP = enemyLevel * 100 + 4000;
        }

        double xp;
        if (levelDiff > 0)
        {
            xp = baseXP * (1.0f + levelDiff * 0.02f);
        }
        else if (levelDiff < -5)
        {
            xp = baseXP * 0.5f;
        }
        else
        {
            xp = baseXP * (1.0f + levelDiff * 0.0075f);
        }

        if (enemy.IsBossGauge)
        {
            xp *= 8.0f;
        }

        double questModifier = 1.0;
        if (enemy.QuestScheduleId != 0)
        {
            var quest = QuestManager.GetQuestByScheduleId(enemy.QuestScheduleId);
            if (quest != null)
            {
                questModifier = (quest.QuestType == QuestType.Main) ? 1.5 : 1.25;
            }
        }
        xp *= questModifier;

        // Put a cap on maximum amount of exp can be gained per kill
        // to slow down power leveling of early level players
        if (playerLevel <= 10)
        {
            xp = Math.Min(xp, (playerLevel * 250) + 1000);
        }
        else if (playerLevel < 60)
        {
            xp = Math.Min(xp, (playerLevel * 500) + 5000);
        }
        return (uint) Math.Max(xp, 0);
    }
}

return new Mixin();
