using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SMandragoraGetMyMandragoraReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_MANDRAGORA_GET_MY_MANDRAGORA_REQ;

        public uint Data0 { get; set; }
        
        public C2SMandragoraGetMyMandragoraReq()
        {
            Data0 = 0;
        }

        public class Serializer : PacketEntitySerializer<C2SMandragoraGetMyMandragoraReq>
        {
            public override void Write(IBuffer buffer, C2SMandragoraGetMyMandragoraReq obj)
            {
                WriteUInt32(buffer, obj.Data0);
            }

            public override C2SMandragoraGetMyMandragoraReq Read(IBuffer buffer)
            {
                C2SMandragoraGetMyMandragoraReq obj = new C2SMandragoraGetMyMandragoraReq();
                obj.Data0 = ReadUInt32(buffer);
                return obj;
            }
        }
    }
}
