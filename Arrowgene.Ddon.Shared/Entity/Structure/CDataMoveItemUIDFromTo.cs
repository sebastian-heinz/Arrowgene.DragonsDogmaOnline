using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataMoveItemUIDFromTo
    {
        public string ItemUId { get; set; } = string.Empty;
        public uint Num { get; set; }
        public StorageType SrcStorageType { get; set; }
        public StorageType DstStorageType { get; set; }
        public ushort SlotNo { get; set; }

        public class Serializer : EntitySerializer<CDataMoveItemUIDFromTo>
        {
            public override void Write(IBuffer buffer, CDataMoveItemUIDFromTo obj)
            {
                WriteMtString(buffer, obj.ItemUId);
                WriteUInt32(buffer, obj.Num);
                WriteByte(buffer, (byte) obj.SrcStorageType);
                WriteByte(buffer, (byte) obj.DstStorageType);
                WriteUInt16(buffer, obj.SlotNo);
            }
        
            public override CDataMoveItemUIDFromTo Read(IBuffer buffer)
            {
                CDataMoveItemUIDFromTo obj = new CDataMoveItemUIDFromTo();
                obj.ItemUId = ReadMtString(buffer);
                obj.Num = ReadUInt32(buffer);
                obj.SrcStorageType = (StorageType) ReadByte(buffer);
                obj.DstStorageType = (StorageType) ReadByte(buffer);
                obj.SlotNo = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
