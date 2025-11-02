using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailMailDeleteRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_MAIL_DELETE_RES;

        public ulong MailId { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMailMailDeleteRes>
        {
            public override void Write(IBuffer buffer, S2CMailMailDeleteRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.MailId);
            }

            public override S2CMailMailDeleteRes Read(IBuffer buffer)
            {
                S2CMailMailDeleteRes obj = new S2CMailMailDeleteRes();
                ReadServerResponse(buffer, obj);
                obj.MailId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
