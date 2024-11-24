using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class StageGetSpAreaChangeInfoHandler : GameRequestPacketHandler<C2SStageGetSpAreaChangeInfoReq, S2CStageGetSpAreaChangeInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(StageGetSpAreaChangeIdFromNpcIdHandler));

        public StageGetSpAreaChangeInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CStageGetSpAreaChangeInfoRes Handle(GameClient client, C2SStageGetSpAreaChangeInfoReq packet)
        {
            return new S2CStageGetSpAreaChangeInfoRes()
            {
                Unk0 = 19,
                Unk1 = "Memory of Megadosys",
                StageId = 559,
            };
        }
    }
}
