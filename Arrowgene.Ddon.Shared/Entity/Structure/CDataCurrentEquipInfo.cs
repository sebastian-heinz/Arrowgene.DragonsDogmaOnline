using System.Linq;
using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCurrentEquipInfo
    {

        public CDataCurrentEquipInfo()
        {
            ItemUID = string.Empty;
        }
        public string ItemUID { get; set; }
        public CDataEquipSlot EquipSlot { get; set; }

        public class Serializer : EntitySerializer<CDataCurrentEquipInfo>
        {
            public override void Write(IBuffer buffer, CDataCurrentEquipInfo obj)
            {
                WriteMtString(buffer, obj.ItemUID);
                WriteEntity<CDataEquipSlot>(buffer, obj.EquipSlot);
            }

            public override CDataCurrentEquipInfo Read(IBuffer buffer)
            {
                    CDataCurrentEquipInfo obj = new CDataCurrentEquipInfo();
                    obj.ItemUID = ReadMtString(buffer);
                    obj.EquipSlot = ReadEntity<CDataEquipSlot>(buffer);
                    return obj;
                
            }
        }
    }
}
