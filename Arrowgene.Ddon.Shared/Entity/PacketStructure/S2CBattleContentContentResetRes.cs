using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentContentResetRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_CONTENT_RESET_RES;

        public class Serializer : PacketEntitySerializer<S2CBattleContentContentResetRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentContentResetRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CBattleContentContentResetRes Read(IBuffer buffer)
            {
                S2CBattleContentContentResetRes obj = new S2CBattleContentContentResetRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
