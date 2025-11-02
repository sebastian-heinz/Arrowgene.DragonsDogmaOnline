using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailMailDeleteReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_MAIL_DELETE_REQ;

        public ulong MailId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SMailMailDeleteReq>
        {
            public override void Write(IBuffer buffer, C2SMailMailDeleteReq obj)
            {
                WriteUInt64(buffer, obj.MailId);
            }

            public override C2SMailMailDeleteReq Read(IBuffer buffer)
            {
                C2SMailMailDeleteReq obj = new C2SMailMailDeleteReq();
                obj.MailId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}
