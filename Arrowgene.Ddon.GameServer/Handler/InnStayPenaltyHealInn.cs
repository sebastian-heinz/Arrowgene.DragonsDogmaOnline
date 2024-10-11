using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnStayPenaltyHealInn : GameStructurePacketHandler<C2SInnStayPenaltyHealInnReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnStayPenaltyHealInn));
        
        public InnStayPenaltyHealInn(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInnStayPenaltyHealInnReq> packet)
        {
            WalletType priceWalletType = InnGetPenaltyHealStayPrice.PointType;
            uint price = InnGetPenaltyHealStayPrice.Point;

            var walletUpdate = Server.WalletManager.RemoveFromWallet(client.Character, priceWalletType, price);

            client.Send(new S2CInnStayPenaltyHealInnRes()
            {
                PointType = priceWalletType,
                Point = walletUpdate?.Value ?? 0
            });
        }
    }
}
