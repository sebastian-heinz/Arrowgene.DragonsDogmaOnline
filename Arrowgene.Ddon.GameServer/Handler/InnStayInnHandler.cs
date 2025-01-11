using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnStayInnHandler : GameRequestPacketHandler<C2SInnStayInnReq, S2CInnStayInnRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnStayInnHandler));

        public InnStayInnHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInnStayInnRes Handle(GameClient client, C2SInnStayInnReq request)
        {
            // TODO: Different values for each request.InnId
            WalletType priceWalletType = InnGetStayPriceHandler.PointType;
            uint price = InnGetStayPriceHandler.Point;

            var walletUpdate = Server.WalletManager.RemoveFromWallet(client.Character, priceWalletType, price)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_INN_LACK_MONEY);

            return new S2CInnStayInnRes()
            {
                PointType = priceWalletType,
                Point = walletUpdate.Value
            };
        }
    }
}
