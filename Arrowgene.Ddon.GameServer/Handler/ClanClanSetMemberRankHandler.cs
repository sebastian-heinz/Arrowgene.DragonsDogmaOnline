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
    public class ClanClanSetMemberRankHandler : GameRequestPacketHandler<C2SClanClanSetMemberRankReq, S2CClanClanSetMemberRankRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanSetMemberRankHandler));

        public ClanClanSetMemberRankHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanSetMemberRankRes Handle(GameClient client, C2SClanClanSetMemberRankReq request)
        {
            Server.ClanManager.SetMemberRank(request.CharacterId, client.Character.ClanId, request.Rank, request.Permission);

            return new S2CClanClanSetMemberRankRes()
            {
                MemberId = request.CharacterId,
                Rank = request.Rank,
            };
        }
    }
}
