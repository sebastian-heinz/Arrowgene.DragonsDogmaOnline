using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailMailGetListDataRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_MAIL_GET_LIST_DATA_RES;

        public S2CMailMailGetListDataRes()
        {
            MailInfo = new List<CDataMailInfo>();
        }

        public List<CDataMailInfo> MailInfo { get; set; }

        public class Serializer : PacketEntitySerializer<S2CMailMailGetListDataRes>
        {
            public override void Write(IBuffer buffer, S2CMailMailGetListDataRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.MailInfo);
            }

            public override S2CMailMailGetListDataRes Read(IBuffer buffer)
            {
                S2CMailMailGetListDataRes obj = new S2CMailMailGetListDataRes();
                ReadServerResponse(buffer, obj);
                obj.MailInfo = ReadEntityList<CDataMailInfo>(buffer);
                return obj;
            }
        }
    }
}
