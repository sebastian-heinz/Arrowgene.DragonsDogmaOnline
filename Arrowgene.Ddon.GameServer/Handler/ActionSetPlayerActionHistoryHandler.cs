using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ActionSetPlayerActionHistoryHandler : GameRequestPacketHandler<C2SActionSetPlayerActionHistoryReq, S2CActionSetPlayerActionHistoryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ActionSetPlayerActionHistoryHandler));

        public ActionSetPlayerActionHistoryHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CActionSetPlayerActionHistoryRes Handle(GameClient client, C2SActionSetPlayerActionHistoryReq request)
        {
            return new();
        }
    }
}
