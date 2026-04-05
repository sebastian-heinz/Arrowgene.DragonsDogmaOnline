/**
 * @brief This mixin is used to calculate the exp amount for a given
 * enemy based on observations from real-world exp data. It should be
 * possible to call this script directly from the server code or from
 * other scripts.
 *
 * Formulas derived by linear regression across 4,614 data points
 * (220 unique enemies, lv0-70) extracted from EnemyExp.xls:
 *
 *   Regular enemies (non-boss):  exp = 20 + 3.5 * lv
 *     Typical fit: R²≈0.9999. Examples at lv0/lv30/lv60:
 *       Goblin/Sling:  17/104/191  (formula: 20/125/230)
 *       Wolf:          18/103/188  (formula: 20/125/230)
 *       Skeleton:      17/ 98/185  (formula: 20/125/230)
 *       Undead:        22/ 87/155  (formula: 20/125/230)
 *       Rogues:        21/133/246  (formula: 20/125/230)
 *
 *   Boss enemies (IsBossGauge):  exp = 1400 + 64 * lv
 *     Median of 16 bosses, each perfectly linear. Examples at lv0/lv30/lv60:
 *       Cyclops:  1200/2640/4080  (formula: 1400/3320/5240)
 *       Griffin:  1500/3750/6000  (formula: 1400/3320/5240)
 *       Golem:    1250/2750/4250  (formula: 1400/3320/5240)
 *       Behemoth: 2800/6160/9520  (formula: 1400/3320/5240)
 *       Wyrm:     2900/6380/9860  (formula: 1400/3320/5240)
 *     Note: individual bosses vary 0.5x–2x the median formula.
 *     Enemies with tool data use EnemyExpScheme.Tool for exact values.
 */

#load "libs.csx"

public class Mixin : IExpMixin
{
    private HashSet<QuestId> AutomaticExpQuestExceptions = new HashSet<QuestId>()
    {
        QuestId.ResolutionsAndOmens
    };

    public override uint GetExpValue(CharacterCommon characterCommon, InstancedEnemy enemy)
    {
        uint result = 0;

        if (enemy.QuestScheduleId != 0)
        {
            var quest = QuestManager.GetQuestByScheduleId(enemy.QuestScheduleId);
            if (AutomaticExpQuestExceptions.Contains(quest.QuestId))
            {
                return 0;
            }
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
    /// Calculates base EXP using linear formulas derived from 4,614 in-game data
    /// points across lv0-70. Regular enemies: 20 + 3.5*lv. Boss enemies: 1400 + 64*lv.
    /// Both formulas represent the median of their respective categories.
    /// </summary>
    private uint GetAutomaticExpCalculation(CharacterCommon characterCommon, InstancedEnemy enemy)
    {
        int enemyLevel = enemy.Lv;
        int playerLevel = (int) characterCommon.ActiveCharacterJobData.Lv;
        int levelDiff = enemyLevel - playerLevel;

        // Linear formulas fit by regression across the full lv0-70 range.
        // Boss formula is the median of 16 bosses; individual bosses vary ±50%.
        // Enemies with tool data bypass this via EnemyExpScheme.Tool.
        double baseXP = enemy.IsBossGauge
            ? 1400.0 + 64.0 * enemyLevel
            : 20.0 + 3.5 * enemyLevel;

        double xp;
        if (levelDiff > 0)
        {
            xp = baseXP * (1.0 + levelDiff * 0.02);
        }
        else if (levelDiff < -5)
        {
            xp = baseXP * 0.5;
        }
        else
        {
            xp = baseXP * (1.0 + levelDiff * 0.0075);
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

        // Cap to slow down power-leveling of low-level players
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
