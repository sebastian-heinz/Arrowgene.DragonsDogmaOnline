using Arrowgene.Buffers;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemEquipElement
    {
        public CDataItemEquipElement() {
            EquipElementList = new List<CDataItemEquipElementParam>();
        }
    
        public string ItemUID { get; set; } = string.Empty;
        public List<CDataItemEquipElementParam> EquipElementList { get; set; }
    
        public class Serializer : EntitySerializer<CDataItemEquipElement>
        {
            public override void Write(IBuffer buffer, CDataItemEquipElement obj)
            {
                WriteMtString(buffer, obj.ItemUID);
                WriteEntityList<CDataItemEquipElementParam>(buffer, obj.EquipElementList);
            }
        
            public override CDataItemEquipElement Read(IBuffer buffer)
            {
                CDataItemEquipElement obj = new CDataItemEquipElement();
                obj.ItemUID = ReadMtString(buffer);
                obj.EquipElementList = ReadEntityList<CDataItemEquipElementParam>(buffer);
                return obj;
            }
        }
    }
}
