using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class EquipItem
    {
        public EquipItem()
        {
            ItemUId = string.Empty;
        }

        public string ItemUId { get; set; }
        public uint CharacterCommonId { get; set; }
        public JobId Job { get; set; }
        public ushort EquipType { get; set; }
        public ushort EquipSlot { get; set; }
    }
}
