using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanUpdateHandler : GameRequestPacketHandler<C2SClanClanUpdateReq, S2CClanClanUpdateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanSettingUpdateHandler));

        public ClanClanUpdateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanUpdateRes Handle(GameClient client, C2SClanClanUpdateReq request)
        {
            Server.ClanManager.UpdateClan(client, request.CreateParam);

            return new S2CClanClanUpdateRes();
        }
    }
}
