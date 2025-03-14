using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonGetBlockadeIdFromOmHandler : GameRequestPacketHandler<C2SSeasonDungeonGetBlockadeIdFromOmReq, S2CSeasonDungeonGetBlockadeIdFromOmRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonGetBlockadeIdFromOmHandler));

        public SeasonDungeonGetBlockadeIdFromOmHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonGetBlockadeIdFromOmRes Handle(GameClient client, C2SSeasonDungeonGetBlockadeIdFromOmReq request)
        {
            var result = new S2CSeasonDungeonGetBlockadeIdFromOmRes();

            var stageId = request.LayoutId.AsStageLayoutId();
            if (Server.AssetRepository.EpitaphTrialAssets.Trials.ContainsKey(stageId))
            {
                var trial = Server.EpitaphRoadManager.GetTrial(stageId, request.PosId);
                if (trial != null)
                {
                    result.EpitaphId = trial.EpitaphId;
                }
            }
            else if (Server.AssetRepository.EpitaphRoadAssets.BarriersByOmId.ContainsKey(stageId))
            {
                result.EpitaphId = Server.AssetRepository.EpitaphRoadAssets.BarriersByOmId[stageId].EpitaphId;
            }

            return result;
        }
    }
}
