using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailSystemMailGetListHeadReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_SYSTEM_MAIL_GET_LIST_HEAD_REQ;

        public C2SMailSystemMailGetListHeadReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMailSystemMailGetListHeadReq>
        {
            public override void Write(IBuffer buffer, C2SMailSystemMailGetListHeadReq obj)
            {
            }

            public override C2SMailSystemMailGetListHeadReq Read(IBuffer buffer)
            {
                C2SMailSystemMailGetListHeadReq obj = new C2SMailSystemMailGetListHeadReq();
                return obj;
            }
        }
    }
}

