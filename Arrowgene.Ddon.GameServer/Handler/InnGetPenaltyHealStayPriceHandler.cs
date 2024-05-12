using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnGetPenaltyHealStayPrice : GameStructurePacketHandler<C2SInnGetPenaltyHealStayPriceReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnGetPenaltyHealStayPrice));

        public static readonly WalletType PointType = WalletType.Gold;
        public static readonly uint Point = 1000; // TODO: The amount went up each time

        public InnGetPenaltyHealStayPrice(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInnGetPenaltyHealStayPriceReq> packet)
        {
            client.Send(new S2CInnGetPenaltyHealStayPriceRes()
            {
                PointType = InnGetPenaltyHealStayPrice.PointType,
                Point = InnGetPenaltyHealStayPrice.Point
            });
        }
    }
}