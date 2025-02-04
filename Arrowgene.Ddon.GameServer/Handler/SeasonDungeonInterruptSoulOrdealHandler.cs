using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SeasonDungeonInterruptSoulOrdealHandler : GameRequestPacketHandler<C2SSeasonDungeonInterruptSoulOrdealReq, S2CSeasonDungeonInterruptSoulOrdealRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SeasonDungeonInterruptSoulOrdealHandler));

        public SeasonDungeonInterruptSoulOrdealHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSeasonDungeonInterruptSoulOrdealRes Handle(GameClient client, C2SSeasonDungeonInterruptSoulOrdealReq request)
        {
            if (Server.EpitaphRoadManager.TrialInProgress(client.Party))
            {
                Server.EpitaphRoadManager.InterruptTrialInProgress(client.Party);
            }

            return new S2CSeasonDungeonInterruptSoulOrdealRes();
        }
    }
}
