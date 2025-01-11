using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetGpHandler : GameRequestPacketHandler<C2SGpGetGpReq, S2CGpGetGpRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetGpHandler));

        public GpGetGpHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CGpGetGpRes Handle(GameClient client, C2SGpGetGpReq request)
        {
            var amount = Server.WalletManager.GetWalletAmount(client.Character, Shared.Model.WalletType.GoldenGemstones);

            return new S2CGpGetGpRes()
            {
                GP = amount,
                UseLimit = 0, ///TODO: Investigate.
                RealTime = DateTimeOffset.Now
            };
        }
    }
}
