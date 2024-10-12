using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnStayInnHandler : GameStructurePacketHandler<C2SInnStayInnReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnStayInnHandler));

        public InnStayInnHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInnStayInnReq> req)
        {
            // TODO: Different values for each req.Structure.InnId
            WalletType priceWalletType = InnGetStayPriceHandler.PointType;
            uint price = InnGetStayPriceHandler.Point;

            var walletUpdate = Server.WalletManager.RemoveFromWallet(client.Character, priceWalletType, price)
                ?? throw new ResponseErrorException(ErrorCode.ERROR_CODE_INN_LACK_MONEY);

            client.Send(new S2CInnStayInnRes()
            {
                PointType = priceWalletType,
                Point = walletUpdate.Value
            });
        }
    }
}
