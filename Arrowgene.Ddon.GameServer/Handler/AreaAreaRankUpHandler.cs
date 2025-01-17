using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaAreaRankUpHandler : GameRequestPacketHandler<C2SAreaAreaRankUpReq, S2CAreaAreaRankUpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaAreaRankUpHandler));


        public AreaAreaRankUpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaAreaRankUpRes Handle(GameClient client, C2SAreaAreaRankUpReq request)
        {
            S2CAreaAreaRankUpRes res = new();
            AreaRank clientRank = client.Character.AreaRanks.GetValueOrDefault(request.AreaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
            lock (clientRank)
            {
                clientRank.Rank += 1;
                clientRank.Point = 0;
            }

            res.AreaId = request.AreaId;
            res.AreaRank = clientRank.Rank;
            res.AreaPoint = clientRank.Point;
            res.NextAreaPoint = Server.AreaRankManager.GetMaxPoints(request.AreaId, clientRank.Rank);

            if (Server.GameLogicSettings.EnableAreaRankSpotLocks)
            {
                res.ReleaseList = Server.AssetRepository.AreaRankSpotInfoAsset
                .Where(x => x.AreaId == request.AreaId && x.UnlockRank == clientRank.Rank)
                .Select(x => new CDataCommonU32(x.SpotId))
                .ToList();
            }
            
            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.Database.UpdateAreaRank(client.Character.CharacterId, clientRank, connection);

                // First rank unlocks supplies, so give them out immediately.
                if (clientRank.Rank == 1)
                {
                    var rewards = Server.AreaRankManager.GetSupplyRewardList(request.AreaId, clientRank.Rank, 0);
                    client.Character.AreaSupply[request.AreaId] = new();

                    foreach (var reward in rewards)
                    {
                        Server.Database.InsertAreaRankSupply(client.Character.CharacterId, request.AreaId, reward.Index, reward.ItemId, reward.Num, connection);

                        // Do a manual deep copy to prevent reference issues down the line.
                        client.Character.AreaSupply[request.AreaId].Add(new()
                        {
                            Index = reward.Index,
                            ItemId = reward.ItemId,
                            Num = reward.Num
                        });
                    }
                }
            });

            return res;
        }
    }
}
