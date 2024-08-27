using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;
using Arrowgene.Ddon.Shared.Entity.Structure;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentGetRewardRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_GET_REWARD_RES;

        public S2CBattleContentGetRewardRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CBattleContentGetRewardRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentGetRewardRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CBattleContentGetRewardRes Read(IBuffer buffer)
            {
                S2CBattleContentGetRewardRes obj = new S2CBattleContentGetRewardRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
