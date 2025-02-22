using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Model.Quest;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaMasterInfoHandler : GameRequestPacketHandler<C2SAreaGetAreaMasterInfoReq, S2CAreaGetAreaMasterInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaMasterInfoHandler));

        public AreaGetAreaMasterInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetAreaMasterInfoRes Handle(GameClient client, C2SAreaGetAreaMasterInfoReq request)
        {
            var clientRank = client.Character.AreaRanks.GetValueOrDefault(request.AreaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
            var completedQuests = client.Character.CompletedQuests;

            S2CAreaGetAreaMasterInfoRes result = new();

            result.AreaId = request.AreaId;
            result.Rank = clientRank.Rank;
            result.Point = clientRank.Point;
            result.WeekPoint = clientRank.WeekPoint;
            result.LastWeekPoint = clientRank.LastWeekPoint;
            result.ToNextPoint = Server.AreaRankManager.GetMaxPoints(request.AreaId, clientRank.Rank);

            result.ReleaseList = Server.AssetRepository.AreaRankSpotInfoAsset[request.AreaId]
                .Where(x =>
                    x.UnlockRank <= clientRank.Rank
                    && (x.UnlockQuest == 0 || completedQuests.ContainsKey((QuestId)x.UnlockQuest))
                )
                .Select(x => new CDataCommonU32(x.SpotId))
                .ToList();

            result.CanReceiveSupply = client.Character.AreaSupply.ContainsKey(request.AreaId) && client.Character.AreaSupply[request.AreaId].Any();
            result.CanRankUp = Server.AreaRankManager.CanRankUp(client, request.AreaId);

            result.SupplyItemInfoList = Server.AssetRepository.AreaRankSupplyAsset[request.AreaId]
            .Where(x =>
                x.MinRank <= clientRank.Rank
            )
            .LastOrDefault()
            ?.SupplyItemInfoList
            ?? new();
            result.AreaRankUpQuestInfoList = Server.AreaRankManager.RankUpQuestInfo(request.AreaId);

            return result;
        }
    }
}
