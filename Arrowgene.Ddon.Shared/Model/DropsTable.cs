using System.Collections.Generic;
using System.Linq;
using Arrowgene.Ddon.Shared.Model;

public class DropsTable
{
    public DropsTable()
    {
        Name = string.Empty;
        Items = new List<GatheringItem>();
    }

    public DropsTable(DropsTable table)
    {
        Id = table.Id;
        Name = table.Name;
        MdlType = table.MdlType;
        Items = CloneItemList(table.Items);
    }

    public uint Id { get; set; }
    public string Name { get; set; }
    public byte MdlType { get; set; }
    public List<GatheringItem> Items { get; set; }

    public DropsTable Clone()
    {
        return new DropsTable(this);
    }

    public DropsTable AddDrop(ItemId itemId, uint minAmount, uint maxAmount, double dropChance, uint quality = 0, bool isHidden = false)
    {
        Items.Add(new GatheringItem()
        {
            ItemId = itemId,
            ItemNum = minAmount,
            MaxItemNum = maxAmount,
            DropChance = dropChance,
            Quality = quality,
            IsHidden = isHidden,
        });
        return this;
    }

    public DropsTable Combine(DropsTable original, List<GatheringItem> items)
    {
        var newItems = CloneItemList(items);
        var copy = new DropsTable(original);
        copy.Items.AddRange(newItems);
        return copy;
    }

    private List<GatheringItem> CloneItemList(List<GatheringItem> items)
    {
        return items.Select(x => new GatheringItem()
        {
            ItemId = x.ItemId,
            ItemNum = x.ItemNum,
            DropChance = x.DropChance,
            IsHidden = x.IsHidden,
            MaxItemNum = x.MaxItemNum,
            Quality = x.Quality
        }).ToList();
    }
}
