using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataRewardItemDetail
{
    public ItemId ItemId { get; set; }
    public ushort Num { get; set; }
    public byte Type {  get; set; }

    public class Serializer : EntitySerializer<CDataRewardItemDetail>
    {
        public override void Write(IBuffer buffer, CDataRewardItemDetail obj)
        {
            WriteUInt32(buffer, (uint) obj.ItemId);
            WriteUInt16(buffer, obj.Num);
            WriteByte(buffer, obj.Type);
        }

        public override CDataRewardItemDetail Read(IBuffer buffer)
        {
            CDataRewardItemDetail obj = new CDataRewardItemDetail();
            obj.ItemId = (ItemId) ReadUInt32(buffer);
            obj.Num = ReadUInt16(buffer);
            obj.Type = ReadByte(buffer);
            return obj;
        }
    }
}
