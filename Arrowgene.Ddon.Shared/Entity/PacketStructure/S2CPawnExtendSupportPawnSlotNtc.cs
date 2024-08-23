using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExtendSupportPawnSlotNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_EXTEND_SUPPORT_PAWN_SLOT_NTC;

        public S2CPawnExtendSupportPawnSlotNtc()
        {
        }

        public byte AddNum { get; set; }
        public byte TotalNum { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnExtendSupportPawnSlotNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnExtendSupportPawnSlotNtc obj)
            {
                WriteByte(buffer, obj.AddNum);
                WriteByte(buffer, obj.TotalNum);
            }

            public override S2CPawnExtendSupportPawnSlotNtc Read(IBuffer buffer)
            {
                S2CPawnExtendSupportPawnSlotNtc obj = new S2CPawnExtendSupportPawnSlotNtc();
                obj.AddNum = ReadByte(buffer);
                obj.TotalNum = ReadByte(buffer);
                return obj;
            }
        }
    }
}
