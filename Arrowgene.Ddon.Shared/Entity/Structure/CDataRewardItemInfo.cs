using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataRewardItemInfo
    {
        public uint Index { get; set; }
        public uint ItemId { get; set; }
        public byte Num { get; set; }

        public class Serializer : EntitySerializer<CDataRewardItemInfo>
        {
            public override void Write(IBuffer buffer, CDataRewardItemInfo obj)
            {
                WriteUInt32(buffer, obj.Index);
                WriteUInt32(buffer, obj.ItemId);
                WriteByte(buffer, obj.Num);
            }

            public override CDataRewardItemInfo Read(IBuffer buffer)
            {
                CDataRewardItemInfo obj = new CDataRewardItemInfo();
                obj.Index = ReadUInt32(buffer);
                obj.ItemId = ReadUInt32(buffer);
                obj.Num = ReadByte(buffer);
                return obj;
            }
        }
    }
}
