using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public enum ShopType : uint
    {
        Unknown = 0,
        Trinkets = 1, // Does this shop have a proper name (NPC Gregory)?

        EmblemMedalExchangeRathniteFoothills = 2,
        EmblemMedalExchangeFeryanaWilderness = 3,
        EmblemMedalExchangeMegadosysPlateau = 4,
        EmblemMedalExchangeUrtecaMountains = 5,

        BitterblackMaze = 6,

        // I can find no evidence that these exchanges were ever used, but they have unique IDs.
        MedalExchangeHidellPlains = 7,
        MedalExchangeBriaCoast = 8,
        MedalExchangeMysreeForest = 9,
        MedalExchangeVoldenMines = 10,
        MedalExchangeDoweValley = 11,
        MedalExchangeMysreeGrove = 12,
        MedalExchangeDeenanWoods = 13,
        MedalExchangeBetlandPlains = 14,
        MedalExchangeNorthernBetlandPlains = 15,
        MedalExchangeZandoraWastelands = 16,
        MedalExchangeEasternZandora = 17,
        MedalExchangeMergodaRuins = 18,
        MedalExchangeBloodbaneIsle = 19,
        MedalExchangeElanWaterGrove = 20,
        MedalExchangeFaranaPlains = 21,
        MedalExchangeMorrowForest = 22,
        MedalExchangeKingalCanyon = 23,

        ExtremeMission = 27,
    }
}
