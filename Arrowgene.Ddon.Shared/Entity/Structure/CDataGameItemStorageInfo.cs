using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataGameItemStorageInfo
    {
        public CDataGameItemStorage GameItemStorage { get; set; } = new();
        public ushort UsedSlotNum { get; set; }
        public ushort MaxSlotNum { get; set; }

        public class Serializer : EntitySerializer<CDataGameItemStorageInfo>
        {
            public override void Write(IBuffer buffer, CDataGameItemStorageInfo obj)
            {
                WriteEntity<CDataGameItemStorage>(buffer, obj.GameItemStorage);
                WriteUInt16(buffer, obj.UsedSlotNum);
                WriteUInt16(buffer, obj.MaxSlotNum);
            }

            public override CDataGameItemStorageInfo Read(IBuffer buffer)
            {
                CDataGameItemStorageInfo obj = new CDataGameItemStorageInfo
                {
                    GameItemStorage = ReadEntity<CDataGameItemStorage>(buffer),
                    UsedSlotNum = ReadUInt16(buffer),
                    MaxSlotNum = ReadUInt16(buffer),
                };

                return obj;
            }
        }
    }
}
