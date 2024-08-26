using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobOrbTreeGetJobOrbTreeStatusListHandler : GameRequestPacketHandler<C2SJobOrbTreeGetJobOrbTreeStatusListReq, S2CJobOrbTreeGetJobOrbTreeStatusListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobOrbTreeGetJobOrbTreeStatusListHandler));

        public JobOrbTreeGetJobOrbTreeStatusListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobOrbTreeGetJobOrbTreeStatusListRes Handle(GameClient client, C2SJobOrbTreeGetJobOrbTreeStatusListReq request)
        {
            S2CJobOrbTreeGetJobOrbTreeStatusListRes response = new S2CJobOrbTreeGetJobOrbTreeStatusListRes();

            foreach (var jobId in (JobId[]) JobId.GetValues(typeof(JobId)))
            {
                if (jobId == JobId.None)
                {
                    continue;
                }

                response.TreeStatusList.Add(new CDataJobOrbTreeStatus()
                {
                    JobId = jobId,
                    IsReleased = client.Character.HasContentReleased(jobId.SkillAugmentationReleaseId()),
                    Rate = Server.JobOrbUnlockManager.CalculatePercentCompleted(client.Character, jobId)
                });
            }

            return response;
        }
    }
}
