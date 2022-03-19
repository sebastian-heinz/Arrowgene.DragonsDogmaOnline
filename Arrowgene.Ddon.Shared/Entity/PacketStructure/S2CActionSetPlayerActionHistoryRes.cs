using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CActionSetPlayerActionHistoryRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_ACTION_SET_PLAYER_ACTION_HISTORY_RES;

        public class Serializer : PacketEntitySerializer<S2CActionSetPlayerActionHistoryRes>
        {

            public override void Write(IBuffer buffer, S2CActionSetPlayerActionHistoryRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CActionSetPlayerActionHistoryRes Read(IBuffer buffer)
            {
                S2CActionSetPlayerActionHistoryRes obj = new S2CActionSetPlayerActionHistoryRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }

}
