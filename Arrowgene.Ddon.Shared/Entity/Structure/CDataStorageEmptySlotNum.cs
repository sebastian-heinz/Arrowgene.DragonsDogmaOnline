using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
        
namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataStorageEmptySlotNum
    {
        public CDataStorageEmptySlotNum()
        {
        }

        public StorageType StorageType { get; set; }
        public ushort Slots {  get; set; }

        public class Serializer : EntitySerializer<CDataStorageEmptySlotNum>
        {
            public override void Write(IBuffer buffer, CDataStorageEmptySlotNum obj)
            {
                WriteByte(buffer, (byte) obj.StorageType);
                WriteUInt16(buffer, obj.Slots);
            }

            public override CDataStorageEmptySlotNum Read(IBuffer buffer)
            {
                CDataStorageEmptySlotNum obj = new CDataStorageEmptySlotNum();
                obj.StorageType = (StorageType)ReadByte(buffer);
                obj.Slots = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
