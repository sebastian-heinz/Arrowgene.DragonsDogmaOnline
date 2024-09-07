using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure;

public class CDataRewardItemDetail
{
    public uint ItemId { get; set; }
    public ushort Num { get; set; }
    public byte Type {  get; set; }

    public class Serializer : EntitySerializer<CDataRewardItemDetail>
    {
        public override void Write(IBuffer buffer, CDataRewardItemDetail obj)
        {
            WriteUInt32(buffer, obj.ItemId);
            WriteUInt16(buffer, obj.Num);
            WriteByte(buffer, obj.Type);
        }

        public override CDataRewardItemDetail Read(IBuffer buffer)
        {
            CDataRewardItemDetail obj = new CDataRewardItemDetail();
            obj.ItemId = ReadUInt32(buffer);
            obj.Num = ReadUInt16(buffer);
            obj.Type = ReadByte(buffer);
            return obj;
        }
    }
}
