using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailSystemMailGetTextReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_SYSTEM_MAIL_GET_TEXT_REQ;

        public C2SMailSystemMailGetTextReq()
        {
        }

        public ulong MailId {  get; set; } // Renamed from Id

        public class Serializer : PacketEntitySerializer<C2SMailSystemMailGetTextReq>
        {
            public override void Write(IBuffer buffer, C2SMailSystemMailGetTextReq obj)
            {
                WriteUInt64(buffer, obj.MailId);
            }

            public override C2SMailSystemMailGetTextReq Read(IBuffer buffer)
            {
                C2SMailSystemMailGetTextReq obj = new C2SMailSystemMailGetTextReq();
                obj.MailId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}

