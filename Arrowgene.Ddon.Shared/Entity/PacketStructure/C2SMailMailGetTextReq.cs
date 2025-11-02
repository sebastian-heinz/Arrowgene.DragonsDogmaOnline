using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailMailGetTextReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_MAIL_GET_TEXT_REQ;

        public ulong MailId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SMailMailGetTextReq>
        {
            public override void Write(IBuffer buffer, C2SMailMailGetTextReq obj)
            {
                WriteUInt64(buffer, obj.MailId);
            }

            public override C2SMailMailGetTextReq Read(IBuffer buffer)
            {
                C2SMailMailGetTextReq obj = new C2SMailMailGetTextReq();
                obj.MailId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
