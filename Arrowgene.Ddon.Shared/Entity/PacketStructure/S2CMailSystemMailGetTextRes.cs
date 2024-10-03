using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailSystemMailGetTextRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_SYSTEM_MAIL_GET_TEXT_RES;

        public S2CMailSystemMailGetTextRes()
        {
            MailTextInfo = new CDataMailTextInfo();
        }

        public ulong MailId {  get; set; }
        public CDataMailTextInfo MailTextInfo {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CMailSystemMailGetTextRes>
        {
            public override void Write(IBuffer buffer, S2CMailSystemMailGetTextRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt64(buffer, obj.MailId);
                WriteEntity(buffer, obj.MailTextInfo);
            }

            public override S2CMailSystemMailGetTextRes Read(IBuffer buffer)
            {
                S2CMailSystemMailGetTextRes obj = new S2CMailSystemMailGetTextRes();
                ReadServerResponse(buffer, obj);
                obj.MailId = ReadUInt64(buffer);
                obj.MailTextInfo = ReadEntity<CDataMailTextInfo>(buffer);
                return obj;
            }
        }
    }
}
