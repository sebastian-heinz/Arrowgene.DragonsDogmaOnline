using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.Shared.Model
{
    public class Crest
    {
        public uint Slot {  get; set; }
        public uint CrestId { get; set; }
        public uint Amount { get; set; }

        public CDataEquipElementParam ToCDataEquipElementParam()
        {
            return new CDataEquipElementParam()
            {
                SlotNo = (byte)Slot,
                CrestId = CrestId,
                Add = (ushort)Amount
            };
        }
    }
}
