using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataChangeEquipJobItem
    {
        public CDataChangeEquipJobItem() {
            EquipJobItemUId = string.Empty;
        }
    
        public string EquipJobItemUId { get; set; }
        public byte EquipSlotNo { get; set; }
    
        public class Serializer : EntitySerializer<CDataChangeEquipJobItem>
        {
            public override void Write(IBuffer buffer, CDataChangeEquipJobItem obj)
            {
                WriteMtString(buffer, obj.EquipJobItemUId);
                WriteByte(buffer, obj.EquipSlotNo);
            }
        
            public override CDataChangeEquipJobItem Read(IBuffer buffer)
            {
                CDataChangeEquipJobItem obj = new CDataChangeEquipJobItem();
                obj.EquipJobItemUId = ReadMtString(buffer);
                obj.EquipSlotNo = ReadByte(buffer);
                return obj;
            }
        }
    }
}