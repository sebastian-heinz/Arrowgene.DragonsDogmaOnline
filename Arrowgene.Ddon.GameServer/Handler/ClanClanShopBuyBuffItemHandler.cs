using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class ClanClanShopBuyBuffItemHandler : GameRequestPacketHandler<C2SClanClanShopBuyBuffItemReq, S2CClanClanShopBuyBuffItemRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(ClanClanGetMemberListHandler));

        public ClanClanShopBuyBuffItemHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CClanClanShopBuyBuffItemRes Handle(GameClient client, C2SClanClanShopBuyBuffItemReq request)
        {
            // TODO: Implement pawn expeditions.
            throw new ResponseErrorException(ErrorCode.ERROR_CODE_FAIL);
        }
    }
}
