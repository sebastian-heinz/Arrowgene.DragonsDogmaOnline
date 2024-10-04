using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanNegotiateMasterHandler : GameRequestPacketHandler<C2SClanClanNegotiateMasterReq, S2CClanClanNegotiateMasterRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanNegotiateMasterHandler));

        public ClanClanNegotiateMasterHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanNegotiateMasterRes Handle(GameClient client, C2SClanClanNegotiateMasterReq request)
        {
            Server.ClanManager.NegotiateMaster(request.CharacterId, client.Character.ClanId);

            return new S2CClanClanNegotiateMasterRes()
            {
                MemberId = request.CharacterId,
            };
        }
    }
}
