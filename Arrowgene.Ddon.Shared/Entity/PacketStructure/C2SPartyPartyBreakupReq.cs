using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyBreakupReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_BREAKUP_REQ;

        public class Serializer : PacketEntitySerializer<C2SPartyPartyBreakupReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyBreakupReq obj)
            {
            }

            public override C2SPartyPartyBreakupReq Read(IBuffer buffer)
            {
                return new C2SPartyPartyBreakupReq();
            }
        }
    }
}
