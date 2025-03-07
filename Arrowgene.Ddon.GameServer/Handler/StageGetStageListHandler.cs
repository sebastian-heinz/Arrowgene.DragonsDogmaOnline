using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageGetStageListHandler : GameRequestPacketHandler<C2SStageGetStageListReq, S2CStageGetStageListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageGetStageListHandler));

        public StageGetStageListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageGetStageListRes Handle(GameClient client, C2SStageGetStageListReq request)
        {
            return new S2CStageGetStageListRes()
            {
                StageList = Server.StageList
            };
        }
    }
}
