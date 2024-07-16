using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCurrentEquipInfo
    {
        public CDataCurrentEquipInfo()
        {
            ItemUId = string.Empty;
            EquipSlot = new CDataEquipSlot();
        }

        public string ItemUId { get; set; }
        public CDataEquipSlot EquipSlot {  get; set; }
        
        public class Serializer : EntitySerializer<CDataCurrentEquipInfo>
        {
            public override void Write(IBuffer buffer, CDataCurrentEquipInfo obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteEntity(buffer, obj.EquipSlot);
            }

            public override CDataCurrentEquipInfo Read(IBuffer buffer)
            {
                CDataCurrentEquipInfo obj = new CDataCurrentEquipInfo();
                obj.ItemUId = ReadMtString(buffer);
                obj.EquipSlot = ReadEntity<CDataEquipSlot>(buffer);
                return obj;
            }
        }
    }
}
