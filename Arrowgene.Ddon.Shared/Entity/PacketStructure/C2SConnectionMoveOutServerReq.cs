using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class C2SConnectionMoveOutServerReq : IPacketStructure
    {
        public PacketId Id => PacketId.C2S_CONNECTION_MOVE_OUT_SERVER_REQ;

        public class Serializer : PacketEntitySerializer<C2SConnectionMoveOutServerReq>
        {

            public override void Write(IBuffer buffer, C2SConnectionMoveOutServerReq obj)
            {
                // No Data
            }

            public override C2SConnectionMoveOutServerReq Read(IBuffer buffer)
            {
                C2SConnectionMoveOutServerReq obj = new C2SConnectionMoveOutServerReq();
                return obj;
            }
        }
    }
}
