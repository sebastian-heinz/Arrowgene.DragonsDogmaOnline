using System;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

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
            // TODO: Implement
            client.Send(new S2CGpGetGpRes()
            {
                GP = 0,
                UseLimit = 0,
                RealTime = DateTimeOffset.Now
            });
        }
    }
}