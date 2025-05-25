using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SJobEmblemGetEmblemListReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_JOB_EMBLEM_GET_EMBLEM_LIST_REQ;

        public class Serializer : PacketEntitySerializer<C2SJobEmblemGetEmblemListReq>
        {
            public override void Write(IBuffer buffer, C2SJobEmblemGetEmblemListReq obj)
            {
            }

            public override C2SJobEmblemGetEmblemListReq Read(IBuffer buffer)
            {
                C2SJobEmblemGetEmblemListReq obj = new C2SJobEmblemGetEmblemListReq();
                return obj;
            }
        }

    }
}
