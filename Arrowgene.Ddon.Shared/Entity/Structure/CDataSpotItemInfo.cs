using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSpotItemInfo
    {
        public uint ItemId { get; set; }
        public byte PawnTakeRate { get; set; }

        public class Serializer : EntitySerializer<CDataSpotItemInfo>
        {
            public override void Write(IBuffer buffer, CDataSpotItemInfo obj)
            {
                WriteUInt32(buffer, obj.ItemId);
                WriteByte(buffer, obj.PawnTakeRate);
            }

            public override CDataSpotItemInfo Read(IBuffer buffer)
            {
                CDataSpotItemInfo obj = new CDataSpotItemInfo();
                obj.ItemId = ReadUInt32(buffer);
                obj.PawnTakeRate = ReadByte(buffer);
                return obj;
            }
        }
    }
}
