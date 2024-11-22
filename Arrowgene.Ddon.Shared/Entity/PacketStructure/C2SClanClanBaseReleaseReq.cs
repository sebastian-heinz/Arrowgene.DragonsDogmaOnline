using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SClanClanBaseReleaseReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CLAN_CLAN_BASE_RELEASE_REQ;

        public C2SClanClanBaseReleaseReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SClanClanBaseReleaseReq>
        {
            public override void Write(IBuffer buffer, C2SClanClanBaseReleaseReq obj)
            {
            }

            public override C2SClanClanBaseReleaseReq Read(IBuffer buffer)
            {
                C2SClanClanBaseReleaseReq obj = new C2SClanClanBaseReleaseReq();
                return obj;
            }
        }
    }
}
