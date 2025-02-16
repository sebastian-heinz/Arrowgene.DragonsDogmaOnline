/**
 * Settings file for Seasonal Events.
 * This file supports hotloading.
 *
 * @note The way seasonal events are implemented by Capcom,
 * it is not possible to have all versions of the event enabled
 * at the same time. Each event should have ValidPeriod and Year
 * value which configures which set of quests are selected and when
 * they become active.
 */

/**
 * @brief Used to determine if the Halloween seasonal event is enabled or not.
 */
bool EnableHalloweenEvent = true;

/**
 * @brief The daterange that the Halloween event should be available
 * if EnableHalloweenEvent is set to true. The format is in MM/DD.
 */
var HalloweenValidPeriod = LibUtils.EventTimespan("10/1", "10/31");

/**
 * This option configures which version will be used when
 * the setting EnableHaloweenEvent is set to true.
 *
 * 2016 (Not implemented)
 *     Light a Pumpkin Lantern? (1)
 *     Light a Pumpkin Lantern? (2)
 * 2017
 *     The Darkness of Halloween
 * 2018
 *     Emergency! Not Enough Candy (1)
 *     Emergency! Not Enough Candy (2)
 */
uint HalloweenEventYear = 2018;

/**
 * @brief Used to determine if the Christmas Seasonal event is enabled or not.
 */
bool EnableChristmasEvent = true;

/**
 * @brief The daterange that the Christmas event should be available
 * if EnableChristmasEvent is set to true. The format is in MM/DD.
 */
var ChristmasValidPeriod = LibUtils.EventTimespan("12/1", "12/31");

/**
 * This option configures which version will be used when
 * the setting EnableChristmasEvent is set to true.
 * 
 * 2016 (Not implemented)
 *     Path to Miracles (1)
 *     Path to Miracles (2)
 *     Path to Miracles (3)
 *     Path to Miracles (4)
 *     With GreatDesire to Gift: Upper
 *     With GreatDesire to Gift: Lower
 * 2017 (Not implemented)
 *     Wish on a Shooting Star (1)
 *     Wish on a Shooting Star (2)
 *     Wish on a Shooting Star (3)
 *     Wish on a Shooting Star (4)
 * 2018
 *     Merry Christmas with Smiles (1)
 *     Merry Christmas with Smiles (2)
 */
uint ChristmasEventYear = 2018;

/**
 * @brief Used to determine if the Valentines Seasonal event is enabled or not.
 */
bool EnableValentinesEvent = true;

/**
 * @brief The daterange that the Valentines event should be available
 * if EnableValentinesEvent is set to true. The format is in MM/DD.
 */
var ValentinesValidPeriod = LibUtils.EventTimespan("2/14", "2/29");

/**
 * This option configures which version will be used when
 * the setting EnableValentinesEvent is set to true.
 *
 * 2017
 *     Shape of Love for Someone (1)
 *     Shape of Love for Someone (2)
 *     
 * 2018 (Not implemented)
 *     All Your Feelings in One Glance (1)
 *     All Your Feelings in One Glance (2)
 */
uint ValentinesEventYear = 2017;
