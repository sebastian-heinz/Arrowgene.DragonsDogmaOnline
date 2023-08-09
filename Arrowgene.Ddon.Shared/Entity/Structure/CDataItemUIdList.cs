using Arrowgene.Buffers;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemUIdList
    {
        public CDataItemUIdList() {
            UId = string.Empty;
        }
    
        public string UId { get; set; }
        public uint Num { get; set; }
    
        public class Serializer : EntitySerializer<CDataItemUIdList>
        {
            public override void Write(IBuffer buffer, CDataItemUIdList obj)
            {
                WriteMtString(buffer, obj.UId);
                WriteUInt32(buffer, obj.Num);
            }
        
            public override CDataItemUIdList Read(IBuffer buffer)
            {
                CDataItemUIdList obj = new CDataItemUIdList();
                obj.UId = ReadMtString(buffer);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}