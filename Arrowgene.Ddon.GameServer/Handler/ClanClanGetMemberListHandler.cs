using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanGetMemberListHandler : GameRequestPacketHandler<C2SClanClanGetMemberListReq, S2CClanClanGetMemberListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMemberListHandler));

        public ClanClanGetMemberListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanGetMemberListRes Handle(GameClient client, C2SClanClanGetMemberListReq request)
        {
            S2CClanClanGetMemberListRes res = new S2CClanClanGetMemberListRes();

            res.MemberList = Server.ClanManager.MemberList(request.ClanId);

            return res;
        }
    }
}
