using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailMailGetListFootReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_MAIL_GET_LIST_FOOT_REQ;

        public C2SMailMailGetListFootReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMailMailGetListFootReq>
        {
            public override void Write(IBuffer buffer, C2SMailMailGetListFootReq obj)
            {
            }

            public override C2SMailMailGetListFootReq Read(IBuffer buffer)
            {
                C2SMailMailGetListFootReq obj = new C2SMailMailGetListFootReq();
                return obj;
            }
        }
    }
}

