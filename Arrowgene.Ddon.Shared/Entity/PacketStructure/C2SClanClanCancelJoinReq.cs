using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanCancelJoinReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_CANCEL_JOIN_REQ;

        public class Serializer : PacketEntitySerializer<C2SClanClanCancelJoinReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanCancelJoinReq obj)
            {
            }

            public override C2SClanClanCancelJoinReq Read(IBuffer buffer)
            {
                return new C2SClanClanCancelJoinReq();
            }
        }
    }
}
