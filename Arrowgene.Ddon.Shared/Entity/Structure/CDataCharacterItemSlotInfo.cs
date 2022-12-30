using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataCharacterItemSlotInfo
    {
        public StorageType StorageType { get; set; }
        public ushort SlotMax { get; set; }

        public class Serializer : EntitySerializer<CDataCharacterItemSlotInfo>
        {
            public override void Write(IBuffer buffer, CDataCharacterItemSlotInfo obj)
            {
                WriteByte(buffer, (byte) obj.StorageType);
                WriteUInt16(buffer, obj.SlotMax);
            }

            public override CDataCharacterItemSlotInfo Read(IBuffer buffer)
            {
                CDataCharacterItemSlotInfo obj = new CDataCharacterItemSlotInfo();
                obj.StorageType = (StorageType) ReadByte(buffer);
                obj.SlotMax = ReadUInt16(buffer);
                return obj;
            }
        }
    }
}
