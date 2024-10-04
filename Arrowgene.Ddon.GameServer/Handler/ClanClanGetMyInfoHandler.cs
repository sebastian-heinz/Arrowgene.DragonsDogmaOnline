using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanGetMyInfoHandler : GameRequestPacketHandler<C2SClanClanGetMyInfoReq, S2CClanClanGetMyInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMyInfoHandler));


        public ClanClanGetMyInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanGetMyInfoRes Handle(GameClient client, C2SClanClanGetMyInfoReq request)
        {
            var res = new S2CClanClanGetMyInfoRes()
            {
                ClanParam = Server.ClanManager.GetClan(client.Character.ClanId)
            };

            return res;
        }
    }
}
