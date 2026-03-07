using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentPhaseEntryReadyCancelRes : ServerResponse
    {
        public S2CBattleContentPhaseEntryReadyCancelRes()
        {
        }

        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_PHASE_ENTRY_READY_CANCEL_RES;

        public class Serializer : PacketEntitySerializer<S2CBattleContentPhaseEntryReadyCancelRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentPhaseEntryReadyCancelRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CBattleContentPhaseEntryReadyCancelRes Read(IBuffer buffer)
            {
                S2CBattleContentPhaseEntryReadyCancelRes obj = new();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
