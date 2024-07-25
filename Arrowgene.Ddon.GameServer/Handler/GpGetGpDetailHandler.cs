using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class GpGetGpDetailHandler : GameStructurePacketHandler<C2SGpGetGpDetailReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(GpGetGpDetailHandler));

        public GpGetGpDetailHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SGpGetGpDetailReq> packet)
        {
            client.Send(new S2CGpGetGpDetailRes());
        }
    }
}
