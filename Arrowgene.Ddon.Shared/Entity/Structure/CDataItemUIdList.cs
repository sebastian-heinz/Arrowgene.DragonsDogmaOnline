using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemUIDList
    {
        public CDataItemUIDList() {
            ItemUID = string.Empty;
        }
    
        public string ItemUID { get; set; }
        public uint Num { get; set; }
    
        public class Serializer : EntitySerializer<CDataItemUIDList>
        {
            public override void Write(IBuffer buffer, CDataItemUIDList obj)
            {
                WriteMtString(buffer, obj.ItemUID);
                WriteUInt32(buffer, obj.Num);
            }
        
            public override CDataItemUIDList Read(IBuffer buffer)
            {
                CDataItemUIDList obj = new CDataItemUIDList();
                obj.ItemUID = ReadMtString(buffer);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
