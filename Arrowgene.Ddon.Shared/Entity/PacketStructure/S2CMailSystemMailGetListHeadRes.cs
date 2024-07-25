using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailSystemMailGetListHeadRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_SYSTEM_MAIL_GET_LIST_HEAD_RES;

        public S2CMailSystemMailGetListHeadRes()
        {
        }

        public uint Num {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CMailSystemMailGetListHeadRes>
        {
            public override void Write(IBuffer buffer, S2CMailSystemMailGetListHeadRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Num);
            }

            public override S2CMailSystemMailGetListHeadRes Read(IBuffer buffer)
            {
                S2CMailSystemMailGetListHeadRes obj = new S2CMailSystemMailGetListHeadRes();
                ReadServerResponse(buffer, obj);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
