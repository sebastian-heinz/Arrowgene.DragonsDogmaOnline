using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Entity.Structure;
using System;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class InnStayInnHandler : StructurePacketHandler<GameClient, C2SInnStayInnReq>
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

            // Update character wallet
            CDataWalletPoint wallet = client.Character.WalletPointList.Where(wp => wp.Type == priceWalletType).Single();
            wallet.Value = Math.Max(0, wallet.Value - price);
            Database.UpdateWalletPoint(client.Character.Id, wallet);

            client.Send(new S2CInnStayInnRes()
            {
                PointType = wallet.Type,
                Point = wallet.Value
            });
        }
    }
}
