using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model.Clan;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanCreateHandler : GameRequestPacketHandler<C2SClanClanCreateReq, S2CClanClanCreateRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanCreateHandler));

        public ClanClanCreateHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanCreateRes Handle(GameClient client, C2SClanClanCreateReq request)
        {
            var res = new S2CClanClanCreateRes();

            res.ClanParam = Server.ClanManager.CreateClan(client, request.CreateParam);
            res.MemberList.Add(res.ClanParam.ClanServerParam.MasterInfo);

            return res;
        }
    }
}
