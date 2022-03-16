using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SServerGetRealTimeReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_SERVER_GET_REAL_TIME_REQ;

        public class Serializer : PacketEntitySerializer<C2SServerGetRealTimeReq>
        {
            public override void Write(IBuffer buffer, C2SServerGetRealTimeReq obj)
            {
            }
            
            public override C2SServerGetRealTimeReq Read(IBuffer buffer)
            {
                return new C2SServerGetRealTimeReq();
            }
        }
    }
}