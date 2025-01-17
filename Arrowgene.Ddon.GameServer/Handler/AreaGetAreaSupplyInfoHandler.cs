using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class AreaGetAreaSupplyInfoHandler : GameRequestPacketHandler<C2SAreaGetAreaSupplyInfoReq, S2CAreaGetAreaSupplyInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(AreaGetAreaSupplyInfoHandler));

        public AreaGetAreaSupplyInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CAreaGetAreaSupplyInfoRes Handle(GameClient client, C2SAreaGetAreaSupplyInfoReq request)
        {
            AreaRank clientRank = client.Character.AreaRanks.GetValueOrDefault(request.AreaId)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_AREAMASTER_AREA_INFO_NOT_FOUND);
            S2CAreaGetAreaSupplyInfoRes res = new();

            var asset = Server.AssetRepository.AreaRankSupplyAsset
                .Where(x =>
                    x.AreaId == request.AreaId
                    && x.MinRank <= clientRank.Rank
                )
                .LastOrDefault()
                ?.SupplyItemInfoList
                ?? new();

            if (asset.Count > 0)
            {
                res.SupplyGrade = (byte)asset.FindLastIndex(x => x.MinAreaPoint <= clientRank.LastWeekPoint);
            }

            if (client.Character.AreaSupply.ContainsKey(request.AreaId)) 
            {
                res.RewardItemInfoList = client.Character.AreaSupply[request.AreaId];
            }

            return res;
        }
    }
}
