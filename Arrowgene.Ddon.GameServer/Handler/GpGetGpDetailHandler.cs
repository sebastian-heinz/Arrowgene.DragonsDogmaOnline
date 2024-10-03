using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System;

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
            var res = new S2CGpGetGpDetailRes();
            var amount = Server.WalletManager.GetWalletAmount(client.Character, WalletType.GoldenGemstones);

            res.GPList.Add(new Shared.Entity.Structure.CDataGPDetail()
            {
                GP = amount,
                Max = amount,
                isFree = true,
                Type = GPDetailType.None,
                Created = DateTimeOffset.Now
            });

            return res;
        }
    }
}
