using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class WarpWarpEndHandler : StructurePacketHandler<GameClient, C2SWarpWarpEndNtc>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(WarpWarpEndHandler));

        public WarpWarpEndHandler(DdonServer<GameClient> server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SWarpWarpEndNtc> packet)
        {
            //Do nothing?
        }
    }
}
