using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataJobEmblemInhertianceIncreaseChanceItem
    {
        public ItemId ItemId { get; set; }
        public ushort AmountConsumed { get; set; }
        public byte PercentIncrease { get; set; }

        public class Serializer : EntitySerializer<CDataJobEmblemInhertianceIncreaseChanceItem>
        {
            public override void Write(IBuffer buffer, CDataJobEmblemInhertianceIncreaseChanceItem obj)
            {
                WriteUInt32(buffer, (uint) obj.ItemId);
                WriteUInt16(buffer, obj.AmountConsumed);
                WriteByte(buffer, obj.PercentIncrease);
            }

            public override CDataJobEmblemInhertianceIncreaseChanceItem Read(IBuffer buffer)
            {
                CDataJobEmblemInhertianceIncreaseChanceItem obj = new CDataJobEmblemInhertianceIncreaseChanceItem();
                obj.ItemId = (ItemId) ReadUInt32(buffer);
                obj.AmountConsumed = ReadUInt16(buffer);
                obj.PercentIncrease = ReadByte(buffer);
                return obj;
            }
        }
    }
}

