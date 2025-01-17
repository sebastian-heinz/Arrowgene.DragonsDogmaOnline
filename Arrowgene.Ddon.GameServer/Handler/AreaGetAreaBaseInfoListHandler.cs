using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaBaseInfoListHandler : GameRequestPacketHandler<C2SAreaGetAreaBaseInfoListReq, S2CAreaGetAreaBaseInfoListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaBaseInfoListHandler));

        public AreaGetAreaBaseInfoListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetAreaBaseInfoListRes Handle(GameClient client, C2SAreaGetAreaBaseInfoListReq request)
        {
            // TODO: ClanAreaPoint
            S2CAreaGetAreaBaseInfoListRes res = new();

            foreach ((var area, var rank)  in client.Character.AreaRanks)
            {
                if (rank.Rank == 0 && (uint)area > 1)
                {
                    // Unranked areas are not displayed, except Hidell Plains.
                    continue;
                }

                res.AreaBaseInfoList.Add(new()
                {
                    AreaID = rank.AreaId,
                    Rank = rank.Rank,
                    CurrentPoint = rank.Point,
                    WeekPoint = rank.WeekPoint,
                    NextPoint = Server.AreaRankManager.GetMaxPoints(rank.AreaId, rank.Rank),
                    CanRankUp = Server.AreaRankManager.CanRankUp(client, rank.AreaId),
                    CanReceiveSupply = client.Character.AreaSupply.ContainsKey(rank.AreaId) && client.Character.AreaSupply[area].Any()
                });

            }
            return res;
        }
    }
}
