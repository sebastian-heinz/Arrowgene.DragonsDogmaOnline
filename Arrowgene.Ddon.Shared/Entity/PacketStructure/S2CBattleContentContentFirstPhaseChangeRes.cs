using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentContentFirstPhaseChangeRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_CONTENT_FIRST_PHASE_CHANGE_RES;

        public class Serializer : PacketEntitySerializer<S2CBattleContentContentFirstPhaseChangeRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentContentFirstPhaseChangeRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CBattleContentContentFirstPhaseChangeRes Read(IBuffer buffer)
            {
                S2CBattleContentContentFirstPhaseChangeRes obj = new S2CBattleContentContentFirstPhaseChangeRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}

