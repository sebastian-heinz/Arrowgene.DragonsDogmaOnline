using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailMailSendReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_MAIL_SEND_REQ;

        public List<CDataCommonU32> CharacterIdList { get; set; } = [];
        public string MailText { get; set; } = string.Empty;

        public class Serializer : PacketEntitySerializer<C2SMailMailSendReq>
        {
            public override void Write(IBuffer buffer, C2SMailMailSendReq obj)
            {
                WriteEntityList(buffer, obj.CharacterIdList);
                WriteMtString(buffer, obj.MailText);
            }

            public override C2SMailMailSendReq Read(IBuffer buffer)
            {
                C2SMailMailSendReq obj = new C2SMailMailSendReq();
                obj.CharacterIdList = ReadEntityList<CDataCommonU32>(buffer);
                obj.MailText = ReadMtString(buffer);
                return obj;
            }
        }
    }
}
