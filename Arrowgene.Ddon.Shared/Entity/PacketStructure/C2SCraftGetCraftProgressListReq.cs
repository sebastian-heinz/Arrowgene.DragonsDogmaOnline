using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SCraftGetCraftProgressListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CRAFT_GET_CRAFT_PROGRESS_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SCraftGetCraftProgressListReq>
        {
            public override void Write(IBuffer buffer, C2SCraftGetCraftProgressListReq obj)
            {
            }

            public override C2SCraftGetCraftProgressListReq Read(IBuffer buffer)
            {
                C2SCraftGetCraftProgressListReq obj = new C2SCraftGetCraftProgressListReq();
                return obj;
            }
        }

    }
}