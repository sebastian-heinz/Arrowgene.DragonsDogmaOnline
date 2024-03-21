using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarGetExhibitPossibleNumReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_GET_EXHIBIT_POSSIBLE_NUM_REQ;

        public class Serializer : PacketEntitySerializer<C2SBazaarGetExhibitPossibleNumReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarGetExhibitPossibleNumReq obj)
            {
            }

            public override C2SBazaarGetExhibitPossibleNumReq Read(IBuffer buffer)
            {
                C2SBazaarGetExhibitPossibleNumReq obj = new C2SBazaarGetExhibitPossibleNumReq();
                return obj;
            }
        }

    }
}