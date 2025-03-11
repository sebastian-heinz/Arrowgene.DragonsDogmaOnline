using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CPartnerPawnLikabilityUpNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_PARTNER_PAWN_LIKABILITY_UP_NTC;

        public S2CPartnerPawnLikabilityUpNtc()
        {
            PawnId = 0;
        }

        public uint PawnId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CPartnerPawnLikabilityUpNtc>
        {
            public override void Write(IBuffer buffer, S2CPartnerPawnLikabilityUpNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
            }

            public override S2CPartnerPawnLikabilityUpNtc Read(IBuffer buffer)
            {
                S2CPartnerPawnLikabilityUpNtc obj = new S2CPartnerPawnLikabilityUpNtc();
                obj.PawnId = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
