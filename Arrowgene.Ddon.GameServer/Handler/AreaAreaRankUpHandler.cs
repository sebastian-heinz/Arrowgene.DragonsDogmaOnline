using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
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

            do
            {
                // Rank up once and then see if there are any rank ups after we can also complete
                HandleRankUp(client, request.AreaId, clientRank, res);
            } while (Server.AreaRankManager.CanRankUp(client, request.AreaId));
            
            return res;
        }

        private void HandleRankUp(GameClient client, QuestAreaId areaId, AreaRank clientRank, S2CAreaAreaRankUpRes res)
        {
            uint requiredPoint = Server.AreaRankManager.GetMaxPoints(areaId, clientRank.Rank);
            if (clientRank.Point < requiredPoint)
            {
                throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_POINT_LACK);
            }

            lock (clientRank)
            {
                clientRank.Rank += 1;
                clientRank.Point -= requiredPoint;
            }

            res.AreaId = areaId;
            res.AreaRank = clientRank.Rank;
            res.AreaPoint = clientRank.Point;
            res.NextAreaPoint = Server.AreaRankManager.GetMaxPoints(areaId, clientRank.Rank);

            if (Server.GameSettings.GameServerSettings.EnableAreaRankSpotLocks)
            {
                res.ReleaseList.AddRange(Server.AssetRepository.AreaRankSpotInfoAsset[areaId]
                .Where(x => x.UnlockRank == clientRank.Rank)
                .Select(x => new CDataCommonU32(x.SpotId))
                .ToList());
            }

            Server.Database.ExecuteInTransaction(connection =>
            {
                Server.Database.UpdateAreaRank(client.Character.CharacterId, clientRank, connection);

                // First rank unlocks supplies, so give them out immediately.
                if (clientRank.Rank == 1)
                {
                    var rewards = Server.AreaRankManager.GetSupplyRewardList(areaId, clientRank.Rank, 0);
                    client.Character.AreaSupply[areaId] = new();

                    foreach (var reward in rewards)
                    {
                        Server.Database.InsertAreaRankSupply(client.Character.CharacterId, areaId, reward.Index, reward.ItemId, reward.Num, connection);

                        // Do a manual deep copy to prevent reference issues down the line.
                        client.Character.AreaSupply[areaId].Add(new()
                        {
                            Index = reward.Index,
                            ItemId = reward.ItemId,
                            Num = reward.Num
                        });
                    }
                }
            });
        }
    }
}
