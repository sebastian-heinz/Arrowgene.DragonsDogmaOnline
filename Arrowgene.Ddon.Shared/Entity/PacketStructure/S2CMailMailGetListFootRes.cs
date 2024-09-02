using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailMailGetListFootRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_MAIL_GET_LIST_FOOT_RES;

        public S2CMailMailGetListFootRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CMailMailGetListFootRes>
        {
            public override void Write(IBuffer buffer, S2CMailMailGetListFootRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CMailMailGetListFootRes Read(IBuffer buffer)
            {
                S2CMailMailGetListFootRes obj = new S2CMailMailGetListFootRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
