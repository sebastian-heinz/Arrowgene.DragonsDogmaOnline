using System;
namespace Arrowgene.Ddon.Shared.Model
{
    public class GatheringItem : IStageLocationAsset<uint>, ICloneable
    {
        public StageId StageId { get; set; }
        public uint SubGroupId { get; set; }
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public uint Unk3 { get; set; }
        public bool IsHidden { get; set; }

        public object Clone()
        {
            return new GatheringItem()
            {
                StageId = this.StageId,
                SubGroupId = this.SubGroupId,
                ItemId = this.ItemId,
                ItemNum = this.ItemNum,
                Unk3 = this.Unk3,
                IsHidden = this.IsHidden
            };
        }
    }
}