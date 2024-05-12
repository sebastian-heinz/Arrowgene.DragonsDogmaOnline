using Arrowgene.Buffers;
using Arrowgene.Ddon.GameServer.Dump;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnGetStayPriceHandler : StructurePacketHandler<GameClient, C2SInnGetStayPriceReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(InnGetStayPriceHandler));

        // TODO: Different values for each req.Structure.InnId
        public static readonly WalletType PointType = WalletType.Gold;
        public static readonly uint Point = 100;

        public InnGetStayPriceHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SInnGetStayPriceReq> req)
        {
            client.Send(new S2CInnGetStayPriceRes()
            {
                PointType = InnGetStayPriceHandler.PointType,
                Point = InnGetStayPriceHandler.Point
            });
        }
    }
}
