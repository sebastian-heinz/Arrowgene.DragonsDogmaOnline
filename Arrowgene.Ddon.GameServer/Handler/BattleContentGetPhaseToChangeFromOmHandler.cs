using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class BattleContentGetPhaseToChangeFromOmHandler : GameRequestPacketHandler<C2SBattleContentGetPhaseToChangeFromOmReq, S2CBattleContentGetPhaseToChangeFromOmRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(BattleContentGetPhaseToChangeFromOmHandler));

        public BattleContentGetPhaseToChangeFromOmHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CBattleContentGetPhaseToChangeFromOmRes Handle(GameClient client, C2SBattleContentGetPhaseToChangeFromOmReq request)
        {
            S2CBattleContentGetPhaseToChangeFromOmRes res = new()
            {
                BattleContentStage = new CDataBattleContentStage
                {
                    Id = 2,
                    StageName = "Bitterblack Maze Rotunda: Netherworld 1",
                    Mode = BattleContentMode.Rotunda
                }
            };

            return res;
        }
    }
}
