using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobOrbTreeReleaseJobOrbElementHandler : GameRequestPacketHandler<C2SJobOrbTreeReleaseJobOrbElementReq, S2CJobOrbTreeReleaseJobOrbElementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobOrbTreeReleaseJobOrbElementHandler));

        public JobOrbTreeReleaseJobOrbElementHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobOrbTreeReleaseJobOrbElementRes Handle(GameClient client, C2SJobOrbTreeReleaseJobOrbElementReq request)
        {
            return new();
        }
    }
}

