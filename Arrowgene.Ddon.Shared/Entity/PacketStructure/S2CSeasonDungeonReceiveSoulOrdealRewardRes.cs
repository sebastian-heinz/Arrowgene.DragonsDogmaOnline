using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonReceiveSoulOrdealRewardRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_RES;

        public S2CSeasonDungeonReceiveSoulOrdealRewardRes()
        {
            RewardList = new List<CDataSoulOrdealRewardItem>();
        }

        public List<CDataSoulOrdealRewardItem> RewardList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonReceiveSoulOrdealRewardRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonReceiveSoulOrdealRewardRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.RewardList);
            }

            public override S2CSeasonDungeonReceiveSoulOrdealRewardRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonReceiveSoulOrdealRewardRes obj = new S2CSeasonDungeonReceiveSoulOrdealRewardRes();
                ReadServerResponse(buffer, obj);
                obj.RewardList = ReadEntityList<CDataSoulOrdealRewardItem>(buffer);
                return obj;
            }
        }
    }
}
