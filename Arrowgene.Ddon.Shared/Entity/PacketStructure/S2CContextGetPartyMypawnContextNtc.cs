using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextGetPartyMypawnContextNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_GET_PARTY_MYPAWN_CONTEXT_NTC;


        public S2CContextGetPartyMypawnContextNtc()
        {
            Context = new CDataPartyContextPawn();
        }

        public uint PawnId { get; set; }
        public CDataPartyContextPawn Context { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextGetPartyMypawnContextNtc>
        {
            public override void Write(IBuffer buffer, S2CContextGetPartyMypawnContextNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataPartyContextPawn>(buffer, obj.Context);
            }

            public override S2CContextGetPartyMypawnContextNtc Read(IBuffer buffer)
            {
                S2CContextGetPartyMypawnContextNtc obj = new S2CContextGetPartyMypawnContextNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.Context = ReadEntity<CDataPartyContextPawn>(buffer);
                return obj;
            }
        }
    }
}
