using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyCreateReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_CREATE_REQ;

        public class Serializer : PacketEntitySerializer<C2SPartyPartyCreateReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyCreateReq obj)
            {
            }

            public override C2SPartyPartyCreateReq Read(IBuffer buffer)
            {
                return new C2SPartyPartyCreateReq();
            }
        }
    }
}