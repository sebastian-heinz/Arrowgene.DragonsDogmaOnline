/**
 * @brief Handles calculating the results of item recycling via
 * - Dissassemble Arms
 * - Disassemble Special Arms
 * At the NPC Craig in the Craft Room
 * @settings scripts/uncategorized/recycle_equipment.csx
 *   - CrestRewardItems : List<(ItemId Itemid, uint Amount)>
 *   - CrestToItemRankRanges : List<(uint ItemRank, uint MinLv, uint MaxLv)>
 */

#load "libs.csx"

public class Mixin : IEquipmentRecycleMixin
{
    public override RecycleRewards GetRecycleRewards(AssetRepository assetRepository, ClientItemInfo itemInfo, Item item)
    {
        var rewards = new RecycleRewards();
        rewards.NumRewards = CalculateNumRewards(itemInfo, item);

        var boAmount = CalculateBoAmount(itemInfo, item);
        if (boAmount > 0)
        {
            rewards.AddWalletReward(WalletType.BloodOrbs, boAmount);
        }

        var riftAmount = CalculateRiftAmount(itemInfo, item);
        if (riftAmount > 0)
        {
            rewards.AddWalletReward(WalletType.RiftPoints, riftAmount);
        }

        rewards.AddWalletReward(WalletType.Gold, itemInfo.Price);

        // Find any crafting materials associated with the item
        var baseItemId = LibDdon.Crafting.GetCraftManager().GetItemBaseItemId((ItemId)itemInfo.ItemId);
        rewards.ItemRewards = LibDdon.Crafting.GetCraftManager()
            .GetRecipeMaterialsForItemId((ItemId)baseItemId)
            .Select(x => (x.ItemId, x.Num))
            .ToList();

        // Find any additional rewards to add to the loot pool
        rewards.ItemRewards.AddRange(GetAdditionalCrestRewards(itemInfo));

        return rewards;
    }

    private uint FindBestItemRankForItem(ClientItemInfo itemInfo)
    {
        uint bestItemRank = 0;
        foreach (var range in LibDdon.GetSetting<List<(uint ItemRank, uint MinLv, uint MaxLv)>>("equipment_recycle", "CrestToItemRankRanges"))
        {
            if (itemInfo.Rank >= range.MinLv)
            {
                bestItemRank = (range.ItemRank > bestItemRank) ? range.ItemRank : bestItemRank;
            }
        }
        return bestItemRank;
    }

    private List<(ItemId ItemId, uint Amount)> GetAdditionalCrestRewards(ClientItemInfo itemInfo)
    {
        var bestItemRank = FindBestItemRankForItem(itemInfo);

        return LibDdon.GetSetting<List<(ItemId ItemId, uint Amount)>>("equipment_recycle", "CrestRewardItems")
            .Where(x => LibDdon.Item.GetClientItemInfo(x.ItemId).Rank == bestItemRank)
            .ToList();
    }

    private uint CalculateNumRewards(ClientItemInfo itemInfo, Item item)
    {
        uint numRewards = 1;
        if (IsGachaItem(itemInfo))
        {
            numRewards = 7;
        }
        else if (IsSufficientlyEnhanced(item))
        {
            if (item.AddStatusParamList.Count > 1)
            {
                numRewards = 7;
            }
            else
            {
                numRewards = 2;
            }
        }
        else if (IsMinimalRewardViable(itemInfo, item))
        {
            numRewards = 2;
        }

        if (item.Color != 0 && itemInfo.Rank > 1)
        {
            numRewards += 1;
        }

        return numRewards;
    }

    private uint CalculateBoAmount(ClientItemInfo itemInfo, Item item)
    {
        uint boAmount = 0;
        if (IsGachaItem(itemInfo) || IsSufficientlyEnhanced(item))
        {
            var points = Math.Round(0.054614 * itemInfo.Rank * itemInfo.Rank - 1.348201 * itemInfo.Rank + 2.293587);
            boAmount = (uint)Math.Max(0, points);
        }
        else if (IsMinimalRewardViable(itemInfo, item))
        {
            var points = Math.Round(0.05436 * itemInfo.Level.Value * itemInfo.Level.Value - 1.30585 * itemInfo.Level.Value + 1.25149);
            boAmount = (uint)Math.Max(0, points);
        }

        if (item.Color != 0 && itemInfo.Rank > 1)
        {
            var points = 8.8037 * Math.Sqrt(itemInfo.Rank) - 7.8037;
            points = Math.Max(1, Math.Min(100, points));
            boAmount += (uint)Math.Round(points);
        }

        return boAmount;
    }

    private uint CalculateRiftAmount(ClientItemInfo itemInfo, Item item)
    {
        uint riftAmount = 0;
        uint enhancements = (uint)(item.AddStatusParamList.Count + item.EquipElementParamList.Count + item.PlusValue);

        if (IsGachaItem(itemInfo) || item.AddStatusParamList.Count > 0)
        {
            riftAmount = 1200;
        }
        else if (IsSufficientlyEnhanced(item))
        {
            double points = 0.0606818 * itemInfo.Rank * itemInfo.Rank - 1.220454 * itemInfo.Rank + 43.50425 * enhancements;
            riftAmount = (uint)Math.Round(Math.Max(0, points));
        }
        else if (IsMinimalRewardViable(itemInfo, item))
        {
            double points = -68.719 * Math.Log(itemInfo.Level.Value) + 8.389 * itemInfo.Level.Value + 41.42875 * enhancements;
            riftAmount = (uint)Math.Round(Math.Max(0, points));
        }

        if (item.Color != 0 && itemInfo.Rank > 1)
        {
            var points = 8.8037 * Math.Sqrt(itemInfo.Rank) - 7.8037;
            points = Math.Max(1, Math.Min(100, points));
            riftAmount += (uint)Math.Round(points);
        }

        return riftAmount;
    }

    /// <summary>
    /// Until actual gacha flags are extracted assume an item which has an IR > 1 and level 1 is gacha.
    /// </summary>
    /// <param name="itemInfo"></param>
    /// <returns></returns>
    private bool IsGachaItem(ClientItemInfo itemInfo)
    {
        return itemInfo.Rank > 1 && itemInfo.Level == 1;
    }

    /// <summary>
    /// Checks to see if the item has been enhanced sufficiently to count for proper rewards
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool IsSufficientlyEnhanced(Item item)
    {
        return (item.PlusValue == 3) && (item.EquipPoints == 4) && (item.EquipElementParamList.Count > 0);
    }

    /// <summary>
    /// Checks to see if the player put some work, but not too much
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private bool IsMinimalRewardViable(ClientItemInfo itemInfo, Item item)
    {
        return (item.PlusValue > 0) || (item.EquipPoints > 0) || (item.EquipElementParamList.Count > 0) && (itemInfo.Rank > 1);
    }
}

return new Mixin();
