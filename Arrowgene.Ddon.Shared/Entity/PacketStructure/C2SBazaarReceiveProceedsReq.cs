using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBazaarReceiveProceedsReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BAZAAR_RECEIVE_PROCEEDS_REQ;

        public class Serializer : PacketEntitySerializer<C2SBazaarReceiveProceedsReq>
        {
            public override void Write(IBuffer buffer, C2SBazaarReceiveProceedsReq obj)
            {
            }

            public override C2SBazaarReceiveProceedsReq Read(IBuffer buffer)
            {
                C2SBazaarReceiveProceedsReq obj = new C2SBazaarReceiveProceedsReq();
                return obj;
            }
        }

    }
}