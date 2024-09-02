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
            UId = String.Empty;
        }

        public string UId { get; set; }
        public uint CharacterCommonId { get; set; }
        public JobId Job {  get; set; }
        public EquipType EquipType { get; set; }
        public ushort EquipSlot { get; set; }
    }
}
