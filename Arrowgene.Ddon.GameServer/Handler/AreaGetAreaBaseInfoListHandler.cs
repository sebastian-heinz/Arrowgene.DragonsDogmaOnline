using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;
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
            // client.Send(InGameDump.Dump_58);

            // TODO: ClanAreaPoint
            S2CAreaGetAreaBaseInfoListRes res = new();

            foreach (var areaRank in client.Character.AreaRanks)
            {
                if (areaRank.Rank == 0 && (uint)areaRank.AreaId > 1)
                {
                    // Unranked areas are not displayed, except Hidell Plains.
                    continue;
                }

                res.AreaBaseInfoList.Add(new()
                {
                    AreaID = areaRank.AreaId,
                    Rank = areaRank.Rank,
                    CurrentPoint = areaRank.Point,
                    WeekPoint = areaRank.WeekPoint,
                    NextPoint = Server.AreaRankManager.GetMaxPoints(areaRank.AreaId, areaRank.Rank),
                    CanRankUp = Server.AreaRankManager.CanRankUp(client, areaRank.AreaId),
                    CanReceiveSupply = client.Character.AreaSupply.ContainsKey(areaRank.AreaId) && client.Character.AreaSupply[areaRank.AreaId].Any()
                });

            }
            return res;
        }
    }
}
