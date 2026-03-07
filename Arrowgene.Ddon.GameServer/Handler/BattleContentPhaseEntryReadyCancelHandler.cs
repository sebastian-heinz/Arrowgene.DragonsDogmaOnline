using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentPhaseEntryReadyCancelHandler : GameRequestPacketHandler<C2SBattleContentPhaseEntryReadyCancelReq,
        S2CBattleContentPhaseEntryReadyCancelRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentPhaseEntryReadyCancelHandler));

        public BattleContentPhaseEntryReadyCancelHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentPhaseEntryReadyCancelRes Handle(GameClient client, C2SBattleContentPhaseEntryReadyCancelReq request)
        {
            S2CBattleContentPhaseEntryReadyCancelRes res = new();
            return res;
        }
    }
}
