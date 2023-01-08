using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Model
{
    public class GatheringItem : IStageLocationAsset<uint>
    {
        public StageId StageId { get; set; }
        public uint SubGroupId { get; set; }
        public uint ItemId { get; set; }
        public uint ItemNum { get; set; }
        public uint Unk3 { get; set; }
        public bool Unk4 { get; set; }
    }
}