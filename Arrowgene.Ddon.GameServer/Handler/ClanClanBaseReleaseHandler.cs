using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanBaseReleaseHandler : GameRequestPacketHandler<C2SClanClanBaseReleaseReq, S2CClanClanBaseReleaseRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanBaseReleaseHandler));

        public ClanClanBaseReleaseHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanBaseReleaseRes Handle(GameClient client, C2SClanClanBaseReleaseReq request)
        {
            Server.ClanManager.BaseRelease(client.Character.ClanId);

            return new();
        }
    }
}
