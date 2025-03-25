using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CContextGetPartyMyPawnContextNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_CONTEXT_GET_PARTY_MYPAWN_CONTEXT_NTC;


        public S2CContextGetPartyMyPawnContextNtc()
        {
            Context = new CDataPartyContextPawn();
        }

        public uint PawnId { get; set; }
        public CDataPartyContextPawn Context { get; set; }

        public class Serializer : PacketEntitySerializer<S2CContextGetPartyMyPawnContextNtc>
        {
            public override void Write(IBuffer buffer, S2CContextGetPartyMyPawnContextNtc obj)
            {
                WriteUInt32(buffer, obj.PawnId);
                WriteEntity<CDataPartyContextPawn>(buffer, obj.Context);
            }

            public override S2CContextGetPartyMyPawnContextNtc Read(IBuffer buffer)
            {
                S2CContextGetPartyMyPawnContextNtc obj = new S2CContextGetPartyMyPawnContextNtc();
                obj.PawnId = ReadUInt32(buffer);
                obj.Context = ReadEntity<CDataPartyContextPawn>(buffer);
                return obj;
            }
        }
    }
}
