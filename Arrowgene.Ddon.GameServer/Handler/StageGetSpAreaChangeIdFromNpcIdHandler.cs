using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageGetSpAreaChangeIdFromNpcIdHandler : GameRequestPacketHandler<C2SStageGetSpAreaChangeIdFromNpcIdReq, S2CStageGetSpAreaChangeIdFromNpcIdRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageGetSpAreaChangeIdFromNpcIdHandler));

        public StageGetSpAreaChangeIdFromNpcIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageGetSpAreaChangeIdFromNpcIdRes Handle(GameClient client, C2SStageGetSpAreaChangeIdFromNpcIdReq packet)
        {
            // TODO: Not sure what this req/res is for
            return new S2CStageGetSpAreaChangeIdFromNpcIdRes();
        }
    }
}
