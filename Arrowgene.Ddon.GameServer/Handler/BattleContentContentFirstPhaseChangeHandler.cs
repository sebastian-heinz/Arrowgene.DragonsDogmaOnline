using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentContentFirstPhaseChangeHandler : GameRequestPacketHandler<C2SBattleContentContentFirstPhaseChangeReq, S2CBattleContentContentFirstPhaseChangeRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentContentFirstPhaseChangeHandler));

        public BattleContentContentFirstPhaseChangeHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentContentFirstPhaseChangeRes Handle(GameClient client, C2SBattleContentContentFirstPhaseChangeReq request)
        {
            return new S2CBattleContentContentFirstPhaseChangeRes();
        }
    }
}

