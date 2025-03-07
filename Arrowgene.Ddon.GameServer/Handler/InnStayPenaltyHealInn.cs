using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnStayPenaltyHealInn : GameRequestPacketHandler<C2SInnStayPenaltyHealInnReq, S2CInnStayPenaltyHealInnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnStayPenaltyHealInn));
        
        public InnStayPenaltyHealInn(DdonGameServer server) : base(server)
        {
        }

        public override S2CInnStayPenaltyHealInnRes Handle(GameClient client, C2SInnStayPenaltyHealInnReq request)
        {
            WalletType priceWalletType = InnGetPenaltyHealStayPrice.PointType;
            uint price = InnGetPenaltyHealStayPrice.Point;

            var walletUpdate = Server.WalletManager.RemoveFromWallet(client.Character, priceWalletType, price)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_INN_LACK_MONEY);

            return new S2CInnStayPenaltyHealInnRes()
            {
                PointType = priceWalletType,
                Point = walletUpdate.Value
            };
        }
    }
}
