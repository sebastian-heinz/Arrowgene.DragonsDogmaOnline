using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanGetInfoHandler : GameRequestPacketHandler<C2SClanClanGetInfoReq, S2CClanClanGetInfoRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanSettingUpdateHandler));

        public ClanClanGetInfoHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanGetInfoRes Handle(GameClient client, C2SClanClanGetInfoReq request)
        {
            return new S2CClanClanGetInfoRes()
            {
                ClanParam = Server.ClanManager.GetClan(request.ClanId)
            };
        }
    }
}
