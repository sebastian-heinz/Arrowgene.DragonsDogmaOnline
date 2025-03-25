using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBoxGachaListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BOX_GACHA_BOX_GACHA_LIST_REQ;

        public C2SBoxGachaListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SBoxGachaListReq>
        {
            public override void Write(IBuffer buffer, C2SBoxGachaListReq obj)
            {
            }

            public override C2SBoxGachaListReq Read(IBuffer buffer)
            {
                C2SBoxGachaListReq obj = new C2SBoxGachaListReq();

                return obj;
            }
        }
    }
}
