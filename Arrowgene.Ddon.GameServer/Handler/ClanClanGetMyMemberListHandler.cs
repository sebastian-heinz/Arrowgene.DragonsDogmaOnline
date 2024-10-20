using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanGetMyMemberListHandler : GameRequestPacketHandler<C2SClanClanGetMyMemberListReq, S2CClanClanGetMyMemberListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMyMemberListHandler));

        public ClanClanGetMyMemberListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanGetMyMemberListRes Handle(GameClient client, C2SClanClanGetMyMemberListReq request)
        {
            S2CClanClanGetMyMemberListRes res = new S2CClanClanGetMyMemberListRes();

            res.MemberList = Server.ClanManager.MemberList(client.Character.ClanId);

            return res;
        }
    }
}
