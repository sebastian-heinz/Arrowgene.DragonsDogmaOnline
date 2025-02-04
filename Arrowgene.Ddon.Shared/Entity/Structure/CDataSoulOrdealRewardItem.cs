using Arrowgene.Buffers;

namespace Arrowgene.Ddon.Shared.Entity.Structure
{
    public class CDataSoulOrdealRewardItem
    {
        public CDataSoulOrdealRewardItem()
        {
        }

        public uint RewardIndex { get; set; }
        public uint Amount { get; set; }

        public class Serializer : EntitySerializer<CDataSoulOrdealRewardItem>
        {
            public override void Write(IBuffer buffer, CDataSoulOrdealRewardItem obj)
            {
                WriteUInt32(buffer, obj.RewardIndex);
                WriteUInt32(buffer, obj.Amount);
            }

            public override CDataSoulOrdealRewardItem Read(IBuffer buffer)
            {
                CDataSoulOrdealRewardItem obj = new CDataSoulOrdealRewardItem();
                obj.RewardIndex = ReadUInt32(buffer);
                obj.Amount = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
