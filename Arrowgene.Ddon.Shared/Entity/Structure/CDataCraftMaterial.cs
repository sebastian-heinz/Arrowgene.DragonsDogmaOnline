using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCraftMaterial
    {
        public CDataCraftMaterial() {
            ItemUId = string.Empty;
        }
    
        public string ItemUId { get; set; }
        public uint ItemNum { get; set; }
    
        public class Serializer : EntitySerializer<CDataCraftMaterial>
        {
            public override void Write(IBuffer buffer, CDataCraftMaterial obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteUInt32(buffer, obj.ItemNum);
            }
        
            public override CDataCraftMaterial Read(IBuffer buffer)
            {
                CDataCraftMaterial obj = new CDataCraftMaterial();
                obj.ItemUId = ReadMtString(buffer);
                obj.ItemNum = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}