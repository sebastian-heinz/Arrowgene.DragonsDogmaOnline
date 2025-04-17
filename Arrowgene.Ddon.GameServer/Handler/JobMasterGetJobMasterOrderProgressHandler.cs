using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobMasterGetJobMasterOrderProgressHandler : GameRequestPacketHandler<C2SJobMasterGetJobMasterOrderProgressReq, S2CJobMasterGetJobMasterOrderProgressRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobMasterGetJobMasterOrderProgressHandler));

        public JobMasterGetJobMasterOrderProgressHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobMasterGetJobMasterOrderProgressRes Handle(GameClient client, C2SJobMasterGetJobMasterOrderProgressReq request)
        {
            if (!client.Character.HasContentReleased(request.JobId.JobTrainingReleaseId()))
            {
                return new()
                {
                    JobId = request.JobId
                };
            }

            return new()
            {
                JobId = request.JobId,
                ActiveJobOrderList = client.Character.JobMasterActiveOrders
                    .Where(x => x.Key == request.JobId)
                    .SelectMany(x => x.Value)
                    .ToList()
            };
        }
    }
}
