using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMandragoraGetMyMandragoraReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MANDRAGORA_GET_MY_MANDRAGORA_REQ;

        public C2SMandragoraGetMyMandragoraReq()
        {
        }

        public class Serializer : PacketEntitySerializer<C2SMandragoraGetMyMandragoraReq>
        {
            public override void Write(IBuffer buffer, C2SMandragoraGetMyMandragoraReq obj)
            {
            }

            public override C2SMandragoraGetMyMandragoraReq Read(IBuffer buffer)
            {
                C2SMandragoraGetMyMandragoraReq obj = new C2SMandragoraGetMyMandragoraReq();
                return obj;
            }
        }
    }
}
