using System;
namespace Arrowgene.Ddon.Shared.Model
{
    public class GatheringItem
    {
        public ItemId ItemId { get; set; }
        public uint ItemNum { get; set; }
        public uint MaxItemNum { get; set; }
        public uint Quality { get; set; }
        public bool IsHidden { get; set; }
        public double DropChance { get; set; }
    }
}
