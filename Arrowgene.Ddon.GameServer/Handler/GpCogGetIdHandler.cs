#nullable enable
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpCogGetIdHandler : GameRequestPacketHandler<C2SGpCogGetIdReq, S2CGpCogGetIdRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpCogGetIdHandler));

        public GpCogGetIdHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpCogGetIdRes Handle(GameClient client, C2SGpCogGetIdReq request)
        {
            S2CGpCogGetIdRes res = new S2CGpCogGetIdRes();

            // TODO: implement S2C_GP_COG_GET_ID_RES

            res.CogId = "Arrowgene";
            
            return res;
        }
    }
}
