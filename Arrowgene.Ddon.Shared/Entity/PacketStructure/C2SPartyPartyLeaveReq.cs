using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SPartyPartyLeaveReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_PARTY_PARTY_LEAVE_REQ;

        public class Serializer : PacketEntitySerializer<C2SPartyPartyLeaveReq>
        {
            public override void Write(IBuffer buffer, C2SPartyPartyLeaveReq obj)
            {
            }

            public override C2SPartyPartyLeaveReq Read(IBuffer buffer)
            {
                return new C2SPartyPartyLeaveReq();
            }
        }
    }
}