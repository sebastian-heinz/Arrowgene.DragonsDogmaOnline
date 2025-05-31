using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanGetJoinRequestedListHandler : GameRequestPacketHandler<C2SClanClanGetJoinRequestedListReq, S2CClanClanGetJoinRequestedListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetJoinRequestedListHandler));

        public ClanClanGetJoinRequestedListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanGetJoinRequestedListRes Handle(GameClient client, C2SClanClanGetJoinRequestedListReq request)
        {
            S2CClanClanGetJoinRequestedListRes res = new S2CClanClanGetJoinRequestedListRes();

            if (Server.ClanManager.CheckAnyPermissions(client.Character.CharacterId,
                [
                    ClanPermission.GuildMaster,
                    ClanPermission.JoinRequestApprove,
                    ClanPermission.JoinRequestDeny
                ]))
            {
                Server.Database.ExecuteInTransaction(conn =>
                {
                    res.JoinReqList = Server.Database.GetClanRequestsByClan(client.Character.ClanId, conn);
                });
            }

            return res;
        }
    }
}
