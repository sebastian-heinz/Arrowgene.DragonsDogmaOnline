#nullable enable
using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpShopGetBuyHistoryHandler : GameRequestPacketHandler<C2SGpShopGetBuyHistoryReq, S2CGpShopGetBuyHistoryRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpShopGetBuyHistoryHandler));

        public GpShopGetBuyHistoryHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpShopGetBuyHistoryRes Handle(GameClient client, C2SGpShopGetBuyHistoryReq request)
        {
            S2CGpShopGetBuyHistoryRes res = new S2CGpShopGetBuyHistoryRes();

            // TODO: track which GP shop items were bought before in DB
            res.Items.Add(new CDataGPShopBuyHistoryElement
            {
                ID = 1,
                Name = "Adventure Passport (history)",
                Price = 1,
                AcquisitionTime = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            });

            return res;
        }
    }
}
