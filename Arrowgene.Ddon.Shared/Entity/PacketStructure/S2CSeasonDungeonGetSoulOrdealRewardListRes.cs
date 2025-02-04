using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonGetSoulOrdealRewardListRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_GET_SOUL_ORDEAL_REWARD_LIST_RES;

        public S2CSeasonDungeonGetSoulOrdealRewardListRes()
        {
            ItemRewards = new List<CDataGatheringItemElement>();
            BuffRewards = new List<CDataSeasonDungeonBuffEffectReward>();
        }

        public List<CDataGatheringItemElement> ItemRewards { get; set; }
        public List<CDataSeasonDungeonBuffEffectReward> BuffRewards { get; set; }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonGetSoulOrdealRewardListRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonGetSoulOrdealRewardListRes obj)
            {
                WriteServerResponse(buffer, obj);
                WriteEntityList(buffer, obj.ItemRewards);
                WriteEntityList(buffer, obj.BuffRewards);
            }

            public override S2CSeasonDungeonGetSoulOrdealRewardListRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonGetSoulOrdealRewardListRes obj = new S2CSeasonDungeonGetSoulOrdealRewardListRes();
                ReadServerResponse(buffer, obj);
                obj.ItemRewards = ReadEntityList<CDataGatheringItemElement>(buffer);
                obj.BuffRewards = ReadEntityList<CDataSeasonDungeonBuffEffectReward>(buffer);
                return obj;
            }
        }
    }
}
