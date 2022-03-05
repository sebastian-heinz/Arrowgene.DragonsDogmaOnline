using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CConnectionMoveInServerRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_CONNECTION_MOVE_IN_SERVER_RES;

        public class Serializer : PacketEntitySerializer<S2CConnectionMoveInServerRes>
        {
            public override void Write(IBuffer buffer, S2CConnectionMoveInServerRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CConnectionMoveInServerRes Read(IBuffer buffer)
            {
                S2CConnectionMoveInServerRes obj = new S2CConnectionMoveInServerRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}