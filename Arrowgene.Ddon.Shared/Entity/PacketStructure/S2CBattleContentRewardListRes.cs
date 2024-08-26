using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CBattleContentRewardListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_BATTLE_CONTENT_REWARD_LIST_RES;

        public S2CBattleContentRewardListRes()
        {
            RewardParamList = new List<CDataBattleContentRewardParam>();
        }

        public List<CDataBattleContentRewardParam> RewardParamList {  get; set; }

        public class Serializer : PacketEntitySerializer<S2CBattleContentRewardListRes>
        {
            public override void Write(IBuffer buffer, S2CBattleContentRewardListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.RewardParamList);
            }

            public override S2CBattleContentRewardListRes Read(IBuffer buffer)
            {
                S2CBattleContentRewardListRes obj = new S2CBattleContentRewardListRes();
                ReadServerResponse(buffer, obj);
                obj.RewardParamList = ReadEntityList<CDataBattleContentRewardParam>(buffer);
                return obj;
            }
        }
    }
}
