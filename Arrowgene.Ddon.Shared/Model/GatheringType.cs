using System;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum GatheringType : uint
    {
        OM_GATHER_NONE = 0x0,
        OM_GATHER_TREE_LV1 = 0x1,
        OM_GATHER_TREE_LV2 = 0x2,
        OM_GATHER_TREE_LV3 = 0x3,
        OM_GATHER_TREE_LV4 = 0x4,
        OM_GATHER_JWL_LV1 = 0x5,
        OM_GATHER_JWL_LV2 = 0x6,
        OM_GATHER_JWL_LV3 = 0x7,
        OM_GATHER_CRST_LV1 = 0x8,
        OM_GATHER_CRST_LV2 = 0x9,
        OM_GATHER_CRST_LV3 = 0xA,
        OM_GATHER_CRST_LV4 = 0xB,
        OM_GATHER_KEY_LV1 = 0xC,
        OM_GATHER_KEY_LV2 = 0xD,
        OM_GATHER_KEY_LV3 = 0xE,
        OM_GATHER_TREA_IRON = 0xF,
        OM_GATHER_DRAGON = 0x10,
        OM_GATHER_CORPSE = 0x11,
        OM_GATHER_SHIP = 0x12,
        OM_GATHER_GRASS = 0x13,
        OM_GATHER_FLOWER = 0x14,
        OM_GATHER_MUSHROOM = 0x15,
        OM_GATHER_CLOTH = 0x16,
        OM_GATHER_BOOK = 0x17,
        OM_GATHER_SAND = 0x18,
        OM_GATHER_BOX = 0x19,
        OM_GATHER_ALCHEMY = 0x1A,
        OM_GATHER_WATER = 0x1B,
        OM_GATHER_SHELL = 0x1C,
        OM_GATHER_ANTIQUE = 0x1D,
        OM_GATHER_TWINKLE = 0x1E,
        OM_GATHER_TREA_OLD = 0x1F,
        OM_GATHER_TREA_TREE = 0x20,
        OM_GATHER_TREA_SILVER = 0x21,
        OM_GATHER_TREA_GOLD = 0x22,
        OM_GATHER_KEY_LV4 = 0x23,
        OM_GATHER_ONE_OFF = 0x24,
    }

    public static class GatherTypeExtension
    {
        private static List<GatheringType> ChestType = new()
        {
            GatheringType.OM_GATHER_NONE,
            GatheringType.OM_GATHER_TREA_OLD,
            GatheringType.OM_GATHER_TREA_TREE,
            GatheringType.OM_GATHER_SHIP,
            GatheringType.OM_GATHER_KEY_LV1,
            GatheringType.OM_GATHER_KEY_LV2,
            GatheringType.OM_GATHER_KEY_LV3,
            GatheringType.OM_GATHER_KEY_LV4,
            GatheringType.OM_GATHER_TREA_IRON,
            GatheringType.OM_GATHER_TREA_SILVER,
            GatheringType.OM_GATHER_TREA_GOLD,
        };

        public static bool IsChest(this GatheringType gatheringType)
        {
            return ChestType.Contains(gatheringType);
        }

        private static List<GatheringType> LockedChestType = new()
        {
            GatheringType.OM_GATHER_KEY_LV1,
            GatheringType.OM_GATHER_KEY_LV2,
            GatheringType.OM_GATHER_KEY_LV3,
            GatheringType.OM_GATHER_KEY_LV4
        };

        public static bool IsLockedChest(this GatheringType gatheringType)
        {
            return LockedChestType.Contains(gatheringType);
        }

        private static Dictionary<GatheringType, int> ChestRank = new()
        {
            [GatheringType.OM_GATHER_NONE] = 1,
            [GatheringType.OM_GATHER_TREA_OLD] = 1,
            [GatheringType.OM_GATHER_TREA_TREE] = 2,
            [GatheringType.OM_GATHER_SHIP] = 2,
            [GatheringType.OM_GATHER_KEY_LV1] = 3,
            [GatheringType.OM_GATHER_TREA_IRON] = 4,
            [GatheringType.OM_GATHER_KEY_LV2] = 5,
            [GatheringType.OM_GATHER_KEY_LV3] = 6,
            [GatheringType.OM_GATHER_KEY_LV4] = 7,
            [GatheringType.OM_GATHER_TREA_SILVER] = 8,
            [GatheringType.OM_GATHER_TREA_GOLD] = 9,
        };

        public const int MIN_TYPE_RANK = 1;
        public const int MAX_TYPE_RANK = 9;

        public static int GetChestRank(this GatheringType gatheringType)
        {
            if (!ChestRank.ContainsKey(gatheringType))
            {
                return 1;
            }
            return ChestRank[gatheringType];
        }
    }
}
