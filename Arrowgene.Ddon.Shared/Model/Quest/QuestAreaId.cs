using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Model.Quest
{
    public enum QuestAreaId : uint
    {
        None = 0,
        HidellPlains = 1,
        BreyaCoast = 2,
        MysreeForest = 3,
        VoldenMines = 4,
        DoweValley = 5,
        MysreeGrove = 6,
        DeenanWoods = 7,
        BetlandPlains = 8,
        NorthernBetlandPlains = 9,
        ZandoraWastelands = 10,
        EasternZandora = 11,
        MergodaRuins = 12,
        BloodbaneIsle = 13,
        ElanWaterGrove = 14,
        FaranaPlains = 15,
        MorrowForest = 16,
        KingalCanyon = 17,
        RathniteFoothills = 18,
        FeryanaWilderness = 19,
        MegadosysPlateau = 20,
        UrtecaMountains = 21,
        MemoryOfMegadosys = 22,
        MemoryOfUrteca = 23,
        BitterblackMaze = 24
    }

    public static class QuestAreaIdExtension
    {
        private static Dictionary<QuestAreaId, string> PrettyNameMap = new Dictionary<QuestAreaId, string>()
        {
            [QuestAreaId.None] = "Unknown",
            [QuestAreaId.HidellPlains] = "Hidell Plains",
            [QuestAreaId.BreyaCoast] = "Breya Coast",
            [QuestAreaId.MysreeForest] = "Mysree Forest",
            [QuestAreaId.VoldenMines] = "Volden Mines",
            [QuestAreaId.DoweValley] = "Dowe Valley",
            [QuestAreaId.MysreeGrove] = "Mysree Grove",
            [QuestAreaId.DeenanWoods] = "Deenan Woods",
            [QuestAreaId.BetlandPlains] = "Betland Plains",
            [QuestAreaId.NorthernBetlandPlains] = "Northern Betland Plains",
            [QuestAreaId.ZandoraWastelands] = "Zandora Wastelands",
            [QuestAreaId.EasternZandora] = "Eastern Zandora",
            [QuestAreaId.MergodaRuins] = "Mergoda Ruins",
            [QuestAreaId.BloodbaneIsle] = "Bloodbane Island",
            [QuestAreaId.ElanWaterGrove] = "Elan Water Grove",
            [QuestAreaId.FaranaPlains] = "Farana Plains",
            [QuestAreaId.MorrowForest] = "Morrow Forest",
            [QuestAreaId.KingalCanyon] = "Kingal Canyon",
            [QuestAreaId.RathniteFoothills] = "Rathnite Foothills",
            [QuestAreaId.FeryanaWilderness] = "Feryana Wilderness",
            [QuestAreaId.MegadosysPlateau] = "Megadosys Plateau",
            [QuestAreaId.UrtecaMountains] = "Urteca Mountains",
            [QuestAreaId.MemoryOfMegadosys] = "Memory of Megadosys",
            [QuestAreaId.MemoryOfUrteca] = "Memory of Urteca",
            [QuestAreaId.BitterblackMaze] = "Bitterblack Maze",
        };

        public static string PrettyName(this QuestAreaId areaId)
        {
            return PrettyNameMap[areaId];
        }
    }
}
