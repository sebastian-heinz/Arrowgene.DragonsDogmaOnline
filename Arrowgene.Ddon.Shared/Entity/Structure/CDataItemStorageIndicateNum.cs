using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataItemStorageIndicateNum
    {    
        public uint ItemNum { get; set; }
        public byte StorageType { get; set; }
    
        public class Serializer : EntitySerializer<CDataItemStorageIndicateNum>
        {
            public override void Write(IBuffer buffer, CDataItemStorageIndicateNum obj)
            {
                WriteUInt32(buffer,obj.ItemNum);
                WriteByte(buffer,obj.StorageType);
            }
        
            public override CDataItemStorageIndicateNum Read(IBuffer buffer)
            {
                CDataItemStorageIndicateNum obj = new CDataItemStorageIndicateNum();
                obj.ItemNum = ReadUInt32(buffer);
                obj.StorageType = ReadByte(buffer);
                return obj;
            }
        }
    }
}