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
    public class ClanClanExpelMemberHandler : GameRequestPacketHandler<C2SClanClanExpelMemberReq, S2CClanClanExpelMemberRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanExpelMemberHandler));

        public ClanClanExpelMemberHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanExpelMemberRes Handle(GameClient client, C2SClanClanExpelMemberReq request)
        {
            Server.ClanManager.LeaveClan(request.CharacterId, client.Character.ClanId);
            
            return new S2CClanClanExpelMemberRes();
        }
    }
}
