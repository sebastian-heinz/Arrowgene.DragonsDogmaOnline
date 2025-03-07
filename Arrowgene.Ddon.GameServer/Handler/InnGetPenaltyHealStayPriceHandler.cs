using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnGetPenaltyHealStayPrice : GameRequestPacketHandler<C2SInnGetPenaltyHealStayPriceReq, S2CInnGetPenaltyHealStayPriceRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnGetPenaltyHealStayPrice));

        public static readonly WalletType PointType = WalletType.Gold;
        public static readonly uint Point = 1000; // TODO: The amount went up each time

        public InnGetPenaltyHealStayPrice(DdonGameServer server) : base(server)
        {
        }

        public override S2CInnGetPenaltyHealStayPriceRes Handle(GameClient client, C2SInnGetPenaltyHealStayPriceReq request)
        {
            return new S2CInnGetPenaltyHealStayPriceRes()
            {
                PointType = InnGetPenaltyHealStayPrice.PointType,
                Point = InnGetPenaltyHealStayPrice.Point
            };
        }
    }
}
