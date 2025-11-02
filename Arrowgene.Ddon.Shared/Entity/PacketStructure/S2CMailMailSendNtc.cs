using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailMailSendNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_MAIL_MAIL_SEND_NTC;

        public CDataMailInfo MailInfo { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CMailMailSendNtc>
        {
            public override void Write(IBuffer buffer, S2CMailMailSendNtc obj)
            {
                WriteEntity(buffer, obj.MailInfo);
            }

            public override S2CMailMailSendNtc Read(IBuffer buffer)
            {
                S2CMailMailSendNtc obj = new S2CMailMailSendNtc();
                obj.MailInfo = ReadEntity<CDataMailInfo>(buffer);
                return obj;
            }
        }
    }
}
