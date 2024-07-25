using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailSystemMailGetListDataRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_SYSTEM_MAIL_GET_LIST_DATA_RES;

        public S2CMailSystemMailGetListDataRes()
        {
            MailInfo = new List<CDataMailInfo>();
        }

        public List<CDataMailInfo> MailInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMailSystemMailGetListDataRes>
        {
            public override void Write(IBuffer buffer, S2CMailSystemMailGetListDataRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.MailInfo);
            }

            public override S2CMailSystemMailGetListDataRes Read(IBuffer buffer)
            {
                S2CMailSystemMailGetListDataRes obj = new S2CMailSystemMailGetListDataRes();
                ReadServerResponse(buffer, obj);
                obj.MailInfo = ReadEntityList<CDataMailInfo>(buffer);
                return obj;
            }
        }
    }
}
