#load "libs.csx"

using System;

public class Mixin : IRentalCostMixin
{
    /// <summary>
    /// The base cost of a rental pawn scales quadratically with their job level and craft rank.
    /// The cost for hiring higher level pawns (both combat and crafting) scales exponentially with the distance between that pawn and either your highest level job or your best crafting pawn.
    /// Pawns that are from clan-mates are discounted by 50%.
    /// </summary>

    public static readonly double JOB_PENALTY_MAGNITUDE = 300; // Set to 0.5 to disable this scaling.
    public static readonly double CRAFT_PENALTY_MAGNITUDE = 3; // Set to 0.5 to disable this scaling.
    public static readonly double JOB_LEVEL_PENALTY_RATE = Math.Pow(JOB_PENALTY_MAGNITUDE + 0.5, 1.0 / 99.0); // Approximately JOB_PENALTY_MAGNITUDE x cost at 99 level difference.
    public static readonly double CRAFT_LEVEL_PENALTY_RATE = Math.Pow(CRAFT_PENALTY_MAGNITUDE + 0.5, 1.0 / 75.0); // Approximately CRAFT_PENALTY_MAGNITUDE x cost at 75 craft rank difference.

    public static readonly double CLAN_DISCOUNT_FACTOR = 0.5;

    public override uint GetRentalCost(GameClient client, CDataRegisterdPawnList pawnListEntry, bool isClan)
    {
        uint level = pawnListEntry.PawnListData.Level;
        uint craft = pawnListEntry.PawnListData.CraftRank;

        uint maxLevel = client.Character.CharacterJobDataList.Select(x => x.Lv).Max();
        uint maxCraft = client.Character.Pawns.Select(x => x.CraftData.CraftRank).DefaultIfEmpty().Max();

        uint deltaLevel = level > maxLevel ? level - maxLevel : 0;
        uint deltaCraft = craft > maxCraft ? craft - maxCraft : 0;

        // These penalties are 0.5 for pawns of equal or lower level; the total penalty is the sum of the two, so the minimum price is the base cost.
        double levelPenalty = Math.Pow(JOB_LEVEL_PENALTY_RATE, deltaLevel) - 0.5; 
        double craftPenalty = Math.Pow(CRAFT_LEVEL_PENALTY_RATE, deltaCraft) - 0.5;

        double baseLevelCost = level * 10 + 0.9 * Math.Pow(level, 2);
        double baseCraftCost = craft * 15 + (79.0/98.0) * Math.Pow(craft, 2);

        double discountFactor = isClan ? CLAN_DISCOUNT_FACTOR : 1.0;

        uint adjustedCost = (uint)((baseLevelCost + baseCraftCost) * (levelPenalty + craftPenalty) * discountFactor);

        return adjustedCost;
    }
}

return new Mixin();
