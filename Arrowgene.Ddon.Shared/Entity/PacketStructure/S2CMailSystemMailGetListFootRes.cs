using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailSystemMailGetListFootRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_SYSTEM_MAIL_GET_LIST_FOOT_RES;

        public S2CMailSystemMailGetListFootRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CMailSystemMailGetListFootRes>
        {
            public override void Write(IBuffer buffer, S2CMailSystemMailGetListFootRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CMailSystemMailGetListFootRes Read(IBuffer buffer)
            {
                S2CMailSystemMailGetListFootRes obj = new S2CMailSystemMailGetListFootRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
