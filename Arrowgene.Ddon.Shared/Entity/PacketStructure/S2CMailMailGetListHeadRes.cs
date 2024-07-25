using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailMailGetListHeadRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_MAIL_GET_LIST_HEAD_RES;

        public S2CMailMailGetListHeadRes()
        {
        }

        public uint Num {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CMailMailGetListHeadRes>
        {
            public override void Write(IBuffer buffer, S2CMailMailGetListHeadRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteUInt32(buffer, obj.Num);
            }

            public override S2CMailMailGetListHeadRes Read(IBuffer buffer)
            {
                S2CMailMailGetListHeadRes obj = new S2CMailMailGetListHeadRes();
                ReadServerResponse(buffer, obj);
                obj.Num = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
