/**
 * Settings file for scripts involved in exp modifications.
 * This file supports hotloading.
 */

// EXP Penalty Settings

/**
 * @brief Handles EXP penalties for the party based on the
 * difference between the lowest leveled member and highest
 * leveled member of the party. If the range is larger than
 * the last entry in AdjustPartyEnemyExpTiers, a 0% exp rate
 * is automatically applied.
 *
 * Can be turned on/off by configuring AdjustPartyEnemyExp.
 */
bool EnableAdjustPartyEnemyExp = true;
var AdjustPartyEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
{
    // MinLv and MaxLv define the relative level difference between the levels of the lowest and
    // highest members in the party.
    // The ExpMultiplier value can be a value between [0.0, 1.0] (1.0 = 100%, 0.0 = 0%)
    //
    // MinLv, MaxLv, ExpMultiplier
    (      0,     2,           1.0),
    (      3,     4,           0.9),
    (      5,     6,           0.8),
    (      7,     8,           0.6),
    (      9,    10,           0.5),
};

/**
 * @brief Handles EXP penalties based on the highest leveled member
 * in the party and the level of the target enemy. If the range is
 * larger than the last entry in AdjustTargetLvEnemyExpTiers, a 0%
 * exp rate is automatically applied.
 *
 * Can be turned on/off by configuring AdjustTargetLvEnemyExp.
 */
bool EnableAdjustTargetLvEnemyExp = false;
var AdjustTargetLvEnemyExpTiers = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
{
    // MinLv and MaxLv define the relative level difference between the target and highest member in the party.
    // The ExpMultiplier value can be a value between [0.0, 1.0] (1.0 = 100%, 0.0 = 0%)
    //
    // MinLv, MaxLv, ExpMultiplier
    (      0,     2,           1.0),
    (      3,     4,           0.9),
    (      5,     6,           0.8),
    (      7,     8,           0.6),
    (      9,    10,           0.5),
};

// Pawn EXP settings

/**
 * @brief If set to true, pawns owned by the player will not be included in exp penalty calculations.
 */
bool DisableExpCorrectionForMyPawn = true;

/**
 * @brief If set to true, if the pawn is PawnCatchupLvDiff or more levels behind the players current level, an exp multiplers of PawnCatchupMultiplier is applied.
 */
bool EnablePawnCatchup = true;

/**
 * @brief The exp bonus applied when the pawn catchup mechanic takes place if EnablePawnCatchup is set to true and the level difference requirements are met.
 */
double PawnCatchupMultiplier = 1.5;

/**
 * @brief The minimum level difference required for the catchup mechanic to be active if EnablePawnCatchup is set to true.
 */
uint PawnCatchupLvDiff = 5;
