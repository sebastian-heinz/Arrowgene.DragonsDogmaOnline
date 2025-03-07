using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStorageItemUIDList
    {
        public CDataStorageItemUIDList() {
            
        }
    
        public string ItemUId { get; set; } = string.Empty;
        public uint Num { get; set; }
        public StorageType StorageType { get; set; }
        public ushort SlotNo { get; set; }
    
        public class Serializer : EntitySerializer<CDataStorageItemUIDList>
        {
            public override void Write(IBuffer buffer, CDataStorageItemUIDList obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteUInt32(buffer, obj.Num);
                WriteByte(buffer, (byte) obj.StorageType);
                WriteUInt16(buffer, obj.SlotNo);
            }
        
            public override CDataStorageItemUIDList Read(IBuffer buffer)
            {
                CDataStorageItemUIDList obj = new CDataStorageItemUIDList();
                obj.ItemUId = ReadMtString(buffer);
                obj.Num = ReadUInt32(buffer);
                obj.StorageType = (StorageType) ReadByte(buffer);
                obj.SlotNo = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
