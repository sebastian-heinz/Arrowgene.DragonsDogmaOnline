using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailMailGetListHeadReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_MAIL_GET_LIST_HEAD_REQ;

        public C2SMailMailGetListHeadReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMailMailGetListHeadReq>
        {
            public override void Write(IBuffer buffer, C2SMailMailGetListHeadReq obj)
            {
            }

            public override C2SMailMailGetListHeadReq Read(IBuffer buffer)
            {
                C2SMailMailGetListHeadReq obj = new C2SMailMailGetListHeadReq();
                return obj;
            }
        }
    }
}

