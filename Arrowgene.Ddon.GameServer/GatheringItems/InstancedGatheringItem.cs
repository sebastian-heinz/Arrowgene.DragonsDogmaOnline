using System;
using Arrowgene.Ddon.Shared.Model;

public class InstancedGatheringItem
{
    public InstancedGatheringItem()
    {
    }

    public InstancedGatheringItem(GatheringItem gatheringItem)
    {
        ItemId = gatheringItem.ItemId;
        Quality = gatheringItem.Quality;
        IsHidden = gatheringItem.IsHidden;

        if(Random.Shared.NextDouble() <= gatheringItem.DropChance)
        {
            ItemNum = (uint) Random.Shared.Next((int) gatheringItem.ItemNum, (int) gatheringItem.MaxItemNum+1);
        }
        else
        {
            ItemNum = 0;
        }
    }

    public uint ItemId { get; set; }
    public uint ItemNum { get; set; }
    public uint Quality { get; set; }
    public bool IsHidden { get; set; }
}