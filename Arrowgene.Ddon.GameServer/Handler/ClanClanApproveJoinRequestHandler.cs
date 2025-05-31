using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Logging;
using System;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanApproveJoinRequestHandler : GameRequestPacketHandler<C2SClanClanApproveJoinReq, S2CClanClanApproveJoinRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanApproveJoinRequestHandler));

        public ClanClanApproveJoinRequestHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanApproveJoinRes Handle(GameClient client, C2SClanClanApproveJoinReq request)
        {
            bool shouldDelete = false;

            if (request.IsApproved && Server.ClanManager.CheckAnyPermissions(client.Character.CharacterId,
                [
                    ClanPermission.GuildMaster,
                    ClanPermission.JoinRequestApprove
                ]))
            {
                shouldDelete = true;

                Server.Database.ExecuteInTransaction(conn =>
                {
                    UInt32 clanId = Server.Database.GetClanRequestsByCharacter(request.CharacterId, conn).First().ClanId;
                    Server.ClanManager.JoinClan(request.CharacterId, clanId, conn);

                    if (Server.ClientLookup.GetClientByCharacterId(request.CharacterId) == null)
                    {
                        Server.Database.SetClanRequestApproved(request.CharacterId, conn);
                        shouldDelete = false;
                    }
                });
            }
            else if (!request.IsApproved && Server.ClanManager.CheckAnyPermissions(client.Character.CharacterId,
                [
                    ClanPermission.GuildMaster,
                    ClanPermission.JoinRequestDeny
                ]))
            {
                shouldDelete = true;
            }

            if (shouldDelete)
            {
                Server.Database.ExecuteInTransaction(conn =>
                {
                    Server.Database.DeleteClanRequestByCharacter(request.CharacterId, conn);
                });
            }

            return new S2CClanClanApproveJoinRes();
        }
    }
}
