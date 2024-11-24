using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.EpitaphRoad;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonGetSoulOrdealListFromOmHandler : GameRequestPacketHandler<C2SSeasonDungeonGetSoulOrdealListfromOmReq, S2CSeasonDungeonGetSoulOrdealListfromOmRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonGetSoulOrdealListFromOmHandler));

        public SeasonDungeonGetSoulOrdealListFromOmHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonGetSoulOrdealListfromOmRes Handle(GameClient client, C2SSeasonDungeonGetSoulOrdealListfromOmReq request)
        {
            var result = new S2CSeasonDungeonGetSoulOrdealListfromOmRes();

            Logger.Debug($"EpitaphOM: {request.StageLayoutId.AsStageId()}, PosId={request.PosId}");

            var stageId = request.StageLayoutId.AsStageId();
            if (Server.EpitaphRoadManager.TrialHasRewards(client, stageId, request.PosId))
            {
                result.Type = SoulOrdealOrderState.GetRewards;
                result.Unk2 = true;
            }
            else if (Server.EpitaphRoadManager.TrialInProgress(client.Party))
            {
                result.Type = SoulOrdealOrderState.Interrupt;
                result.Unk2 = true;
            }
            else if (!Server.EpitaphRoadManager.TrialCompleted(client.Party, stageId, request.PosId))
            {
                // TODO: Check to make sure no one in party has rewards available
                result.Type = Server.EpitaphRoadManager.IsTrialUnlocked(client.Party, stageId, request.PosId) ? SoulOrdealOrderState.GetList : SoulOrdealOrderState.UnlockTrial;
                if (result.Type == SoulOrdealOrderState.GetList)
                {
                    var trial = Server.EpitaphRoadManager.GetTrial(stageId, request.PosId);
                    result.ElementParamList = trial.SoulOrdealOptions();
                }
                result.Unk2 = true;
            }
            else
            {
                result.Type = SoulOrdealOrderState.Completed;
                result.Unk2 = true;
            }

            return result;
        }
    }
}
