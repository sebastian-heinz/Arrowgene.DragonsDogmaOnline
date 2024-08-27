using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextGetPartyRentedPawnContextNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_GET_PARTY_RENTED_PAWN_CONTEXT_NTC;


        public S2CContextGetPartyRentedPawnContextNtc()
        {
            Context = new CDataPartyContextPawn();
        }

        public uint PawnId { get; set; }
        public CDataPartyContextPawn Context { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextGetPartyRentedPawnContextNtc>
        {
            public override void Write(IBuffer buffer, S2CContextGetPartyRentedPawnContextNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataPartyContextPawn>(buffer, obj.Context);
            }

            public override S2CContextGetPartyRentedPawnContextNtc Read(IBuffer buffer)
            {
                S2CContextGetPartyRentedPawnContextNtc obj = new S2CContextGetPartyRentedPawnContextNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.Context = ReadEntity<CDataPartyContextPawn>(buffer);
                return obj;
            }
        }
    }
}
