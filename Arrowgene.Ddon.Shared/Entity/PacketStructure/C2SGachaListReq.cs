using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SGachaListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_GACHA_GACHA_LIST_REQ;

        public C2SGachaListReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SGachaListReq>
        {
            public override void Write(IBuffer buffer, C2SGachaListReq obj)
            {
            }

            public override C2SGachaListReq Read(IBuffer buffer)
            {
                C2SGachaListReq obj = new C2SGachaListReq();

                return obj;
            }
        }
    }
}
