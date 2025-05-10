using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler : GameRequestPacketHandler<C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq, S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler));

        public JobOrbTreeGetJobOrbTreeGetAllJobOrbElementListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListRes Handle(GameClient client, C2SJobOrbTreeGetJobOrbTreeGetAllJobOrbElementListReq request)
        {
            return new()
            {
                ElementList = Server.JobOrbUnlockManager.GetUpgradeList(client, request.JobId)
            };
        }
    }
}
