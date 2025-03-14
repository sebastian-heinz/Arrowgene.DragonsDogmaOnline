using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnGetStayPriceHandler : GameRequestPacketHandler<C2SInnGetStayPriceReq, S2CInnGetStayPriceRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnGetStayPriceHandler));

        // TODO: Different values for each req.Structure.InnId
        public static readonly WalletType PointType = WalletType.Gold;
        public static readonly uint Point = 100;

        public InnGetStayPriceHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CInnGetStayPriceRes Handle(GameClient client, C2SInnGetStayPriceReq request)
        {
            return new S2CInnGetStayPriceRes()
            {
                PointType = PointType,
                Point = Point
            };
        }
    }
}
