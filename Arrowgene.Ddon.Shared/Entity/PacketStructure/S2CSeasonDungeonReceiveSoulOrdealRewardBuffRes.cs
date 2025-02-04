using Arrowgene.Buffers;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using System.Collections.Generic;

namespace Arrowgene.Ddon.Shared.Entity.PacketStructure
{
    public class S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes : ServerResponse
    {
        public override PacketId Id => PacketId.S2C_SEASON_DUNGEON_RECEIVE_SOUL_ORDEAL_REWARD_BUFF_RES;

        public S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes()
        {
        }

        public class Serializer : PacketEntitySerializer<S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes>
        {
            public override void Write(IBuffer buffer, S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes obj)
            {
                WriteServerResponse(buffer, obj);
            }

            public override S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes Read(IBuffer buffer)
            {
                S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes obj = new S2CSeasonDungeonReceiveSoulOrdealRewardBuffRes();
                ReadServerResponse(buffer, obj);
                return obj;
            }
        }
    }
}
