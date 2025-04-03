using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobMasterReportJobOrderProgressHandler : GameRequestPacketHandler<C2SJobMasterReportJobOrderProgressReq, S2CJobMasterReportJobOrderProgressRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobMasterReportJobOrderProgressHandler));

        public JobMasterReportJobOrderProgressHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobMasterReportJobOrderProgressRes Handle(GameClient client, C2SJobMasterReportJobOrderProgressReq request)
        {
            var response = new S2CJobMasterReportJobOrderProgressRes()
            {
                JobId = request.JobId,
            };

            Server.Database.ExecuteInTransaction(connection =>
            {
                response.NewOrderIdList = Server.JobMasterManager.GetNewOrders(client, request.JobId, connection);
                response.ReleaseElementList = Server.JobMasterManager.GetNewReleasedElements(client, request.JobId, connection);
            });

            return response;
        }
    }
}
