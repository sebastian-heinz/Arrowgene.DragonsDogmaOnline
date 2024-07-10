using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailSystemMailGetListFootReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_SYSTEM_MAIL_GET_LIST_FOOT_REQ;

        public C2SMailSystemMailGetListFootReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMailSystemMailGetListFootReq>
        {
            public override void Write(IBuffer buffer, C2SMailSystemMailGetListFootReq obj)
            {
            }

            public override C2SMailSystemMailGetListFootReq Read(IBuffer buffer)
            {
                C2SMailSystemMailGetListFootReq obj = new C2SMailSystemMailGetListFootReq();
                return obj;
            }
        }
    }
}

