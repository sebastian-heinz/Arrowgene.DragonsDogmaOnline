using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailSystemMailDeleteRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_SYSTEM_MAIL_DELETE_RES;

        public S2CMailSystemMailDeleteRes()
        {
        }

        public ulong MessageId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMailSystemMailDeleteRes>
        {
            public override void Write(IBuffer buffer, S2CMailSystemMailDeleteRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.MessageId);
            }

            public override S2CMailSystemMailDeleteRes Read(IBuffer buffer)
            {
                S2CMailSystemMailDeleteRes obj = new S2CMailSystemMailDeleteRes();
                ReadServerResponse(buffer, obj);
                obj.MessageId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
