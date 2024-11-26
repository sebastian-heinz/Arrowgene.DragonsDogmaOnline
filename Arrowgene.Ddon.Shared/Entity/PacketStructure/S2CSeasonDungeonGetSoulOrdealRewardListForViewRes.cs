using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGetSoulOrdealRewardListForViewRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_FOR_VIEW_RES;

        public S2CSeasonDungeonGetSoulOrdealRewardListForViewRes()
        {
            RewardList = new List<CDataSeasonDungeonRewardItemViewEntry>();
        }

        public List<CDataSeasonDungeonRewardItemViewEntry> RewardList { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGetSoulOrdealRewardListForViewRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGetSoulOrdealRewardListForViewRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.RewardList);
            }

            public override S2CSeasonDungeonGetSoulOrdealRewardListForViewRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonGetSoulOrdealRewardListForViewRes obj = new S2CSeasonDungeonGetSoulOrdealRewardListForViewRes();
                ReadServerResponse(buffer, obj);
                obj.RewardList = ReadEntityList<CDataSeasonDungeonRewardItemViewEntry>(buffer);
                return obj;
            }
        }
    }
}
