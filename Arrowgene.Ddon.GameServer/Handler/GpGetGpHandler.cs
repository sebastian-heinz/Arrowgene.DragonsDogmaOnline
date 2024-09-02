using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetGpHandler : GameStructurePacketHandler<C2SGpGetGpReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetGpHandler));


        public GpGetGpHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SGpGetGpReq> request)
        {
            var amount = Server.WalletManager.GetWalletAmount(client.Character, Shared.Model.WalletType.GoldenGemstones);

            client.Send(new S2CGpGetGpRes()
            {
                GP = amount,
                UseLimit = 0, ///TODO: Investigate.
                RealTime = DateTimeOffset.Now
            });
        }
    }
}
