using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Model;

public class DropsTable
{
    public DropsTable()
    {
        Name = string.Empty;
        Items = new List<GatheringItem>();
    }

    public uint Id { get; set; }
    public string Name { get; set; }
    public byte MdlType { get; set; }
    public List<GatheringItem> Items { get; set; }

    public DropsTable AddDrop(ItemId itemId, uint minAmount, uint maxAmount, double dropChance)
    {
        Items.Add(new GatheringItem()
        {
            ItemId = (uint)itemId,
            ItemNum = minAmount,
            MaxItemNum = maxAmount,
            DropChance = dropChance
        });
        return this;
    }
}
