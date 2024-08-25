using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPawnExtendMainPawnSlotNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PAWN_EXTEND_MAIN_PAWN_SLOT_NTC;

        public S2CPawnExtendMainPawnSlotNtc()
        {
        }

        public byte AddNum { get; set; }
        public byte TotalNum { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPawnExtendMainPawnSlotNtc>
        {
            public override void Write(IBuffer buffer, S2CPawnExtendMainPawnSlotNtc obj)
            {
                WriteByte(buffer, obj.AddNum);
                WriteByte(buffer, obj.TotalNum);
            }

            public override S2CPawnExtendMainPawnSlotNtc Read(IBuffer buffer)
            {
                S2CPawnExtendMainPawnSlotNtc obj = new S2CPawnExtendMainPawnSlotNtc();
                obj.AddNum = ReadByte(buffer);
                obj.TotalNum = ReadByte(buffer);
                return obj;
            }
        }
    }
}
