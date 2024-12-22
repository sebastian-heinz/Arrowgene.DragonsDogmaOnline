using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CAreaGetAreaRewardInfoRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_AREA_GET_AREA_REWARD_INFO_RES;

        public S2CAreaGetAreaRewardInfoRes()
        {
            RewardItemInfoList = new();
        }

        public List<CDataRewardItemInfo> RewardItemInfoList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CAreaGetAreaRewardInfoRes>
        {
            public override void Write(IBuffer buffer, S2CAreaGetAreaRewardInfoRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.RewardItemInfoList);
            }

            public override S2CAreaGetAreaRewardInfoRes Read(IBuffer buffer)
            {
                S2CAreaGetAreaRewardInfoRes obj = new S2CAreaGetAreaRewardInfoRes();
                ReadServerResponse(buffer, obj);
                obj.RewardItemInfoList = ReadEntityList<CDataRewardItemInfo>(buffer);
                return obj;
            }
        }
    }
}
