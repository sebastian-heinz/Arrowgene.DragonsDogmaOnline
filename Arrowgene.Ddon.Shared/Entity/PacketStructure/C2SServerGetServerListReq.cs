using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SServerGetServerListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SERVER_GET_SERVER_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SServerGetServerListReq>
        {

            public override void Write(IBuffer buffer, C2SServerGetServerListReq obj)
            {
            }

            public override C2SServerGetServerListReq Read(IBuffer buffer)
            {
                C2SServerGetServerListReq obj = new C2SServerGetServerListReq();
                return obj;
            }
        }
    }
}
