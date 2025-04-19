using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CJobOrderCompleteNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_JOB_ORDER_COMPLETE_NTC;

        public S2CJobOrderCompleteNtc()
        {
        }

        public JobId JobId { get; set; }
        public ReleaseType RewardType { get; set; }
        public uint RewardNo { get; set; }
        public byte RewardLv { get; set; }

        public class Serializer : PacketEntitySerializer<S2CJobOrderCompleteNtc>
        {
            public override void Write(IBuffer buffer, S2CJobOrderCompleteNtc obj)
            {
                WriteByte(buffer, (byte) obj.JobId);
                WriteByte(buffer, (byte) obj.RewardType);
                WriteUInt32(buffer, obj.RewardNo);
                WriteByte(buffer, obj.RewardLv);
            }

            public override S2CJobOrderCompleteNtc Read(IBuffer buffer)
            {
                S2CJobOrderCompleteNtc obj = new S2CJobOrderCompleteNtc();
                obj.JobId = (JobId) ReadByte(buffer);
                obj.RewardType = (ReleaseType) ReadByte(buffer);
                obj.RewardNo = ReadUInt32(buffer);
                obj.RewardLv = ReadByte(buffer);
                return obj;
            }
        }
    }
}
