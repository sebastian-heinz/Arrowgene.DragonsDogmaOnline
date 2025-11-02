using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CMailMailSendRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_MAIL_MAIL_SEND_RES;

        public List<CDataCommonU32> CharacterIdList { get; set; } = [];

        public class Serializer : PacketEntitySerializer<S2CMailMailSendRes>
        {
            public override void Write(IBuffer buffer, S2CMailMailSendRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.CharacterIdList);
            }

            public override S2CMailMailSendRes Read(IBuffer buffer)
            {
                S2CMailMailSendRes obj = new S2CMailMailSendRes();
                ReadServerResponse(buffer, obj);
                obj.CharacterIdList = ReadEntityList<CDataCommonU32>(buffer);
                return obj;
            }
        }
    }
}
