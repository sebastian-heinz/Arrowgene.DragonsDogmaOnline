using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanSearchHandler : GameRequestPacketHandler<C2SClanClanSearchReq, S2CClanClanSearchRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanSearchHandler));

        public ClanClanSearchHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanSearchRes Handle(GameClient client, C2SClanClanSearchReq request)
        {
            // TODO: what is SearchType for, and do we care?
            List<CDataClanSearchResult> list = Server.ClanManager.SearchClans(client, request.SearchParam);

            S2CClanClanSearchRes res = new S2CClanClanSearchRes()
            {
                ClanList = list
            };

return res;

        }
    }
}
