/**
 * @brief Controls if the Rookies Ring is enabled or not.
 */
bool EnableRookiesRing = true;

/**
 * @brief Controls the behavior of the Rookie Ring if it is enabled.
 * Constant = 0
 *   This is the behavior of the ring as it was in the original game.
 *   There is a constant exp buff until the maxiumum configured level is exceded.
 * Dynamic = 1
 *  This is a new custom behavior which dynamiclly adjusts the bonus of the Rookie Ring
 *  until the maximum level is execeded.
 */
uint RookiesRingMode = 0;

/**
 * @brief Settings used when the RookiesRingMode = 0 (Constant)
 */
uint RookiesRingMaxLevel = 89;
double RookiesRingBonus = 1.0;

/**
 * @brief Settings used when the RookiesRingMode = 1 (Dynamic)
 */
var DynamicExpBands = new List<(uint MinLv, uint MaxLv, double ExpMultiplier)>()
{
    // MinLv, MaxLv, ExpMultiplier
    (      1,     9,           1.5),
    (      10,   19,           1.0),
    (      20,   29,           0.9),
    (      30,   39,           0.8),
    (      40,   49,           0.7),
    (      50,   59,           0.6),
    (      60,   69,           0.5),
    (      70,   79,           0.4),
    (      80,   89,           0.3),
};
