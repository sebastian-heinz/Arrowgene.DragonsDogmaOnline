using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SBlackListGetBlackListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_BLACK_LIST_GET_BLACK_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SBlackListGetBlackListReq>
        {
            public override void Write(IBuffer buffer, C2SBlackListGetBlackListReq obj)
            {
            }

            public override C2SBlackListGetBlackListReq Read(IBuffer buffer)
            {
                C2SBlackListGetBlackListReq obj = new C2SBlackListGetBlackListReq();
                return obj;
            }
        }
    }
}
