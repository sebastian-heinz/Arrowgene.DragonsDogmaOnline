using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetGpPeriodHandler : GameStructurePacketHandler<C2SGpGetGpPeriodReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetGpPeriodHandler));

        public GpGetGpPeriodHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SGpGetGpPeriodReq> packet)
        {
            var res = new S2CGpGetGpPeriodRes();
            var amount = Server.WalletManager.GetWalletAmount(client.Character, Shared.Model.WalletType.GoldenGemstones);

            res.GPPeriodList.Add(new CDataGPPeriod()
            {
                GP = amount,
                isFreeGP = false,
                Period = 0
            });

            client.Send(res);
        }
    }
}
