using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanLeaveMemberHandler : GameRequestPacketHandler<C2SClanClanLeaveMemberReq, S2CClanClanLeaveMemberRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMyJoinRequestListHandler));

        public ClanClanLeaveMemberHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanLeaveMemberRes Handle(GameClient client, C2SClanClanLeaveMemberReq request)
        {
            Server.ClanManager.LeaveClan(client.Character, client.Character.ClanId);

            return new S2CClanClanLeaveMemberRes();
        }
    }
}
