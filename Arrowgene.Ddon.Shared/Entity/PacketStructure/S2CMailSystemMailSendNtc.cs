using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailSystemMailSendNtc : IPacketStructure
    {
        public PacketId Id => PacketId.S2C_MAIL_SYSTEM_MAIL_SEND_NTC;

        public CDataMailInfo MailInfo { get; set; } = new();

        public class Serializer : PacketEntitySerializer<S2CMailSystemMailSendNtc>
        {
            public override void Write(IBuffer buffer, S2CMailSystemMailSendNtc obj)
            {
                WriteEntity<CDataMailInfo>(buffer, obj.MailInfo);
            }

            public override S2CMailSystemMailSendNtc Read(IBuffer buffer)
            {
                S2CMailSystemMailSendNtc obj = new S2CMailSystemMailSendNtc();
                obj.MailInfo = ReadEntity<CDataMailInfo>(buffer);
                return obj;
            }
        }
    }
}
