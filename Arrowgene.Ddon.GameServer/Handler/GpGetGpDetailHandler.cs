using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetGpDetailHandler : GameRequestPacketHandler<C2SGpGetGpDetailReq, S2CGpGetGpDetailRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetGpDetailHandler));

        public GpGetGpDetailHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpGetGpDetailRes Handle(GameClient client, C2SGpGetGpDetailReq packet)
        {
            S2CGpGetGpDetailRes res = new S2CGpGetGpDetailRes();
            uint amount = Server.WalletManager.GetWalletAmount(client.Character, WalletType.GoldenGemstones);
            DateTimeOffset offset = DateTimeOffset.UtcNow;

            res.GPList.Add(new CDataGPDetail()
            {
                GP = amount,
                Max = amount,
                IsFree = false,
                GetType = GPDetailType.OnlineShopPurchase,
                Expire = offset.AddMonths(12),
                Created = offset
            });

            return res;
        }
    }
}
