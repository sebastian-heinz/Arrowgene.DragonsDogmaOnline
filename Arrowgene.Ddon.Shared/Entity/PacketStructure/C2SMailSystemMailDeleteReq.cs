using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMailSystemMailDeleteReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MAIL_SYSTEM_MAIL_DELETE_REQ;

        public C2SMailSystemMailDeleteReq()
        {
        }

        public ulong MessageId { get; set; }

        public class Serializer : PacketEntitySerializer<C2SMailSystemMailDeleteReq>
        {
            public override void Write(IBuffer buffer, C2SMailSystemMailDeleteReq obj)
            {
                WriteUInt64(buffer, obj.MessageId);
            }

            public override C2SMailSystemMailDeleteReq Read(IBuffer buffer)
            {
                C2SMailSystemMailDeleteReq obj = new C2SMailSystemMailDeleteReq();
                obj.MessageId = ReadUInt64(buffer);
                return obj;
            }
        }
    }
}

