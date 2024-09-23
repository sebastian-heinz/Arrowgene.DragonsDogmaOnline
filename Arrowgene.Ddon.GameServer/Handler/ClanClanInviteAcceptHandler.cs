using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    internal class ClanClanInviteAcceptHandler : GameRequestPacketHandler<C2SClanClanInviteAcceptReq, S2CClanClanInviteAcceptRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanInviteAcceptHandler));

        public ClanClanInviteAcceptHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanInviteAcceptRes Handle(GameClient client, C2SClanClanInviteAcceptReq request)
        {
            Server.ClanManager.AddMemberToClan(client.Character, request.ClanId);

            return new S2CClanClanInviteAcceptRes();
        }
    }
}
