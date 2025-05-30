using Arrowgene.Ddon.Server.Scripting.utils;
using Arrowgene.Ddon.Shared.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace Arrowgene.Ddon.Server.Settings
{
    public class EmblemSettings : IGameSettings
    {
        public EmblemSettings(ScriptableSettings settingsData) : base(settingsData, typeof(EmblemSettings).Name)
        {
        }

        /// <summary>
        /// Sets the maximum level the emblem can be upgraded to.
        /// From 2017-10-12, the LV cap will be released to "10".
        /// From 2018-02-15, the LV cap will be released to "15"
        /// From 2018-06-14, the LV cap will be released to "20"
        /// From 2018-10-11, the LV cap will be released to "25".
        /// From 2018-12-06, the Lv cap will be released to "100".
        /// From 2019-04-04, the Lv cap will be released to "120"
        /// </summary>
        [DefaultValue(_MaxEmblemLevel)]
        public ushort MaxEmblemLevel
        {
            set
            {
                SetSetting("MaxEmblemLevel", value);
            }
            get
            {
                return TryGetSetting("MaxEmblemLevel", _MaxEmblemLevel);
            }
        }
        private const ushort _MaxEmblemLevel = 120;

        /// <summary>
        /// Controls the maximum number of times a stat can be upgraded.
        /// Should be less than or equal to the max level configured in scripts/emblems/stats.
        /// </summary>
        [DefaultValue(_MaxEmblemStatUpgrades)]
        public byte MaxEmblemStatUpgrades
        {
            set
            {
                SetSetting("MaxEmblemStatUpgrades", value);
            }
            get
            {
                return TryGetSetting("MaxEmblemStatUpgrades", _MaxEmblemStatUpgrades);
            }
        }
        private const byte _MaxEmblemStatUpgrades = 15;

        /// <summary>
        /// Defines the maximum amount you can increase your odds when attaching crests to the job emblem.
        /// </summary>
        [DefaultValue(_MaxInheritanceChanceIncrease)]
        public byte MaxInheritanceChanceIncrease
        {
            set
            {
                SetSetting("MaxInheritanceChanceIncrease", value);
            }
            get
            {
                return TryGetSetting("MaxInheritanceChanceIncrease", _MaxInheritanceChanceIncrease);
            }
        }
        private const byte _MaxInheritanceChanceIncrease = 20;

        /// <summary>
        /// Defines the amount GG needed to prevent a piece of equipment from being lost when inheritance fails.
        /// </summary>
        [DefaultValue(_EmblemInheritanceEquipLossGGCost)]
        public uint EmblemInheritanceEquipLossGGCost
        {
            set
            {
                SetSetting("EmblemInheritanceEquipLossGGCost", value);
            }
            get
            {
                return TryGetSetting("EmblemInheritanceEquipLossGGCost", _EmblemInheritanceEquipLossGGCost);
            }
        }
        private const uint _EmblemInheritanceEquipLossGGCost = 2;

        /// <summary>
        /// Defines the amount GG needed to reset the emblem points.
        /// </summary>
        [DefaultValue(_EmblemPointResetGGAmount)]
        public uint EmblemPointResetGGAmount
        {
            set
            {
                SetSetting("EmblemPointResetGGAmount", value);
            }
            get
            {
                return TryGetSetting("EmblemPointResetGGAmount", _EmblemPointResetGGAmount);
            }
        }
        private const uint _EmblemPointResetGGAmount = 10;

        /// <summary>
        /// Defines the amount playpoints (PP) needed to reset the emblem points.
        /// </summary>
        [DefaultValue(_EmblemPointResetPPAmount)]
        public uint EmblemPointResetPPAmount
        {
            set
            {
                SetSetting("EmblemPointResetPPAmount", value);
            }
            get
            {
                return TryGetSetting("EmblemPointResetPPAmount", _EmblemPointResetPPAmount);
            }
        }
        private const uint _EmblemPointResetPPAmount = 1000;

        /// <summary>
        /// Items which can be used to increase the crest inhertience chance when consuming items
        /// for the job emblem.
        /// </summary>
        [DefaultValue(@"new List<(ItemId ItemId, ushort AmountConsumed, byte PercentIncrease)>
{
    // ItemId, AmountConsumed, Percent Increase
    (ItemId.QualityDefenseUpgradeRock, 100, 1),
    (ItemId.WhiteDragonDefenseUpgradeRock, 1, 1),
};")]
        public List<(ItemId ItemId, ushort AmountConsumed, byte PercentIncrease)> InheritanceChanceIncreaseItems
        {
            set
            {
                SetSetting("InheritanceChanceIncreaseItems", value);
            }
            get
            {
                return TryGetSetting("InheritanceChanceIncreaseItems", new List<(ItemId ItemId, ushort AmountConsumed, byte PercentIncrease)>
                {
                    (ItemId.QualityDefenseUpgradeRock, 100, 1),
                    (ItemId.WhiteDragonDefenseUpgradeRock, 1, 1),
                });
            }
        }


        /// <summary>
        /// The levels which additional inheritance (the crest slots) unlock on the emblems.
        /// Emblems always start with 1 slot unlocked.
        /// </summary>
        [DefaultValue(@"new List<(ushort UnlockLevel, byte BaseChance)>
{
    (0,  64),
    (6,  32),
    (11, 16),
    (16, 8),
}")]
        public List<(ushort UnlockLevel, byte BaseChance)> InheritanceUnlockLevels
        {
            set
            {
                SetSetting("InheritanceUnlockLevels", value);
            }
            get
            {
                return TryGetSetting("InheritanceUnlockLevels", new List<(ushort UnlockLevel, byte BaseChance)>
                {
                    (0,  64),
                    (6,  32),
                    (11, 16),
                    (16, 8),
                });
            }
        }

        /// <summary>
        /// Defines the EP cose for each level when upgrading stats
        /// </summary>
        [DefaultValue(@"new List<(byte Level, uint EPAmount)>
{
    (1, 1),
    (2, 3),
    (3, 4),
    (4, 6),
    (5, 9),
    (6, 12),
    (7, 15),
    (8, 18),
    (9, 20),
    (10, 22),
    (11, 24),
    (12, 26),
    (13, 28),
    (14, 30),
    (15, 50),
    (16, 0),
}")]
        public List<(byte Level, uint EPAmount)> StatUpgradeCost
        {
            set
            {
                SetSetting("StatUpgradeCost", value);
            }
            get
            {
                return TryGetSetting("StatUpgradeCost", new List<(byte Level, uint EPAmount)>
                {
                    (1, 1),
                    (2, 3),
                    (3, 4),
                    (4, 6),
                    (5, 9),
                    (6, 12),
                    (7, 15),
                    (8, 18),
                    (9, 20),
                    (10, 22),
                    (11, 24),
                    (12, 26),
                    (13, 28),
                    (14, 30),
                    (15, 50),
                    (16, 0),
                });
            }
        }

        /// <summary>
        /// The Leveling data for emblems.
        /// </summary>
        [DefaultValue(@"new List<(ushort Level, uint PPCost, byte EP)>()
{
    (  1,    0,   5), // 5
    (  2,  100,   5), // 10
    (  3,  300,   5), // 15
    (  4,  500,   5), // 20
    (  5,  500,  10), // 30
    (  6,  800,   5), // 35
    (  7,  800,   5), // 40
    (  8,  800,   5), // 45
    (  9,  800,   5), // 50
    ( 10,  800,  15), // 65
    ( 11, 1000,   5), // 70
    ( 12, 1000,   5), // 75
    ( 13, 1000,   5), // 80
    ( 14, 1000,   5), // 85
    ( 15, 1000,  15), // 100
    ( 16, 1500,   5), // 105
    ( 17, 1500,   5), // 110
    ( 18, 1500,   5), // 115
    ( 19, 1500,   5), // 120
    ( 20, 1500,  15), // 135
    ( 21, 2000,   5), // 140
    ( 22, 2000,   5), // 145
    ( 23, 2000,   5), // 150
    ( 24, 2000,   5), // 155
    ( 25, 2000,  15), // 170
    ( 26,  100,   1),
    ( 27,  100,   1),
    ( 28,  100,   1),
    ( 29,  100,   1),
    ( 30,  100,   5),
    ( 31,  200,   1),
    ( 32,  200,   1),
    ( 33,  200,   1),
    ( 34,  200,   1),
    ( 35,  200,   5),
    ( 36,  300,   1),
    ( 37,  300,   1),
    ( 38,  300,   1),
    ( 39,  300,   1),
    ( 40,  300,   5),
    ( 41,  400,   1),
    ( 42,  400,   1),
    ( 43,  400,   1),
    ( 44,  400,   1),
    ( 45,  400,   5),
    ( 46,  500,   1),
    ( 47,  500,   1),
    ( 48,  500,   1),
    ( 49,  500,   1),
    ( 50,  500,   5),
    ( 51,  600,   1),
    ( 52,  600,   1),
    ( 53,  600,   1),
    ( 54,  600,   1),
    ( 55,  600,   5),
    ( 56,  700,   1),
    ( 57,  700,   1),
    ( 58,  700,   1),
    ( 59,  700,   1),
    ( 60,  700,   5),
    ( 61,  800,   1),
    ( 62,  800,   1),
    ( 63,  800,   1),
    ( 64,  800,   1),
    ( 65,  800,   5),
    ( 66,  900,   1),
    ( 67,  900,   1),
    ( 68,  900,   1),
    ( 69,  900,   1),
    ( 70,  900,   5),
    ( 71,  1000,  1),
    ( 72,  1000,  1),
    ( 73,  1000,  1),
    ( 74,  1000,  1),
    ( 75,  1000,  5),
    ( 76,  1100,  1),
    ( 77,  1100,  1),
    ( 78,  1100,  1),
    ( 79,  1100,  1),
    ( 80,  1100,  5),
    ( 81,  1200,  1),
    ( 82,  1200,  1),
    ( 83,  1200,  1),
    ( 84,  1200,  1),
    ( 85,  1200,  5),
    ( 86,  1300,  1),
    ( 87,  1300,  1),
    ( 88,  1300,  1),
    ( 89,  1300,  1),
    ( 90,  1300,  5),
    ( 91,  1400,  1),
    ( 92,  1400,  1),
    ( 93,  1400,  1),
    ( 94,  1400,  1),
    ( 95,  1500,  5),
    ( 96,  1600,  1),
    ( 97,  1700,  1),
    ( 98,  1800,  1),
    ( 99,  1900,  1),
    (100,  2000,  5),
    (101,  2000,  1),
    (102,  2000,  1),
    (103,  2000,  1),
    (104,  2000,  1),
    (105,  2000,  5),
    (106,  2000,  1),
    (107,  2000,  1),
    (108,  2000,  1),
    (109,  2000,  1),
    (110,  2000,  5),
    (111,  2000,  1),
    (112,  2000,  1),
    (113,  2000,  1),
    (114,  2000,  1),
    (115,  2000,  5),
    (116,  2000,  2),
    (117,  2000,  2),
    (118,  2000,  2),
    (119,  2000,  2),
    (120,  2000, 10),
}")]
        public List<(ushort Level, uint PPCost, byte EP)> LevelingData
        {
            set
            {
                SetSetting("LevelingData", value);
            }
            get
            {
                return TryGetSetting("LevelingData", new List<(ushort Level, uint PPCost, byte EP)>()
                {
                    (  1,    0,   5), // 5
                    (  2,  100,   5), // 10
                    (  3,  300,   5), // 15
                    (  4,  500,   5), // 20
                    (  5,  500,  10), // 30
                    (  6,  800,   5), // 35
                    (  7,  800,   5), // 40
                    (  8,  800,   5), // 45
                    (  9,  800,   5), // 50
                    ( 10,  800,  15), // 65
                    ( 11, 1000,   5), // 70
                    ( 12, 1000,   5), // 75
                    ( 13, 1000,   5), // 80
                    ( 14, 1000,   5), // 85
                    ( 15, 1000,  15), // 100
                    ( 16, 1500,   5), // 105
                    ( 17, 1500,   5), // 110
                    ( 18, 1500,   5), // 115
                    ( 19, 1500,   5), // 120
                    ( 20, 1500,  15), // 135
                    ( 21, 2000,   5), // 140
                    ( 22, 2000,   5), // 145
                    ( 23, 2000,   5), // 150
                    ( 24, 2000,   5), // 155
                    ( 25, 2000,  15), // 170
                    ( 26,  100,   1),
                    ( 27,  100,   1),
                    ( 28,  100,   1),
                    ( 29,  100,   1),
                    ( 30,  100,   5),
                    ( 31,  200,   1),
                    ( 32,  200,   1),
                    ( 33,  200,   1),
                    ( 34,  200,   1),
                    ( 35,  200,   5),
                    ( 36,  300,   1),
                    ( 37,  300,   1),
                    ( 38,  300,   1),
                    ( 39,  300,   1),
                    ( 40,  300,   5),
                    ( 41,  400,   1),
                    ( 42,  400,   1),
                    ( 43,  400,   1),
                    ( 44,  400,   1),
                    ( 45,  400,   5),
                    ( 46,  500,   1),
                    ( 47,  500,   1),
                    ( 48,  500,   1),
                    ( 49,  500,   1),
                    ( 50,  500,   5),
                    ( 51,  600,   1),
                    ( 52,  600,   1),
                    ( 53,  600,   1),
                    ( 54,  600,   1),
                    ( 55,  600,   5),
                    ( 56,  700,   1),
                    ( 57,  700,   1),
                    ( 58,  700,   1),
                    ( 59,  700,   1),
                    ( 60,  700,   5),
                    ( 61,  800,   1),
                    ( 62,  800,   1),
                    ( 63,  800,   1),
                    ( 64,  800,   1),
                    ( 65,  800,   5),
                    ( 66,  900,   1),
                    ( 67,  900,   1),
                    ( 68,  900,   1),
                    ( 69,  900,   1),
                    ( 70,  900,   5),
                    ( 71,  1000,  1),
                    ( 72,  1000,  1),
                    ( 73,  1000,  1),
                    ( 74,  1000,  1),
                    ( 75,  1000,  5),
                    ( 76,  1100,  1),
                    ( 77,  1100,  1),
                    ( 78,  1100,  1),
                    ( 79,  1100,  1),
                    ( 80,  1100,  5),
                    ( 81,  1200,  1),
                    ( 82,  1200,  1),
                    ( 83,  1200,  1),
                    ( 84,  1200,  1),
                    ( 85,  1200,  5),
                    ( 86,  1300,  1),
                    ( 87,  1300,  1),
                    ( 88,  1300,  1),
                    ( 89,  1300,  1),
                    ( 90,  1300,  5),
                    ( 91,  1400,  1),
                    ( 92,  1400,  1),
                    ( 93,  1400,  1),
                    ( 94,  1400,  1),
                    ( 95,  1500,  5),
                    ( 96,  1600,  1),
                    ( 97,  1700,  1),
                    ( 98,  1800,  1),
                    ( 99,  1900,  1),
                    (100,  2000,  5),
                    (101,  2000,  1),
                    (102,  2000,  1),
                    (103,  2000,  1),
                    (104,  2000,  1),
                    (105,  2000,  5),
                    (106,  2000,  1),
                    (107,  2000,  1),
                    (108,  2000,  1),
                    (109,  2000,  1),
                    (110,  2000,  5),
                    (111,  2000,  1),
                    (112,  2000,  1),
                    (113,  2000,  1),
                    (114,  2000,  1),
                    (115,  2000,  5),
                    (116,  2000,  2),
                    (117,  2000,  2),
                    (118,  2000,  2),
                    (119,  2000,  2),
                    (120,  2000, 10),
                });
            }
        }
    }
}
