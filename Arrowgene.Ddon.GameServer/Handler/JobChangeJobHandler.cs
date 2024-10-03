using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobChangeJobHandler : GameRequestPacketHandler<C2SJobChangeJobReq, S2CJobChangeJobRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobChangeJobHandler));

        private readonly JobManager jobManager;

        public JobChangeJobHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override S2CJobChangeJobRes Handle(GameClient client, C2SJobChangeJobReq request)
        {
            (S2CJobChangeJobRes jobRes, S2CItemUpdateCharacterItemNtc itemNtc, S2CJobChangeJobNtc jobNtc) jobResult = (null, null, null);

            Server.Database.ExecuteInTransaction(connection =>
            {
                jobResult = ((S2CJobChangeJobRes, S2CItemUpdateCharacterItemNtc, S2CJobChangeJobNtc))
                jobManager.SetJob(client, client.Character, request.JobId, connection);
            });

            if (jobResult.jobNtc != null)
            {
                foreach (GameClient otherClient in Server.ClientLookup.GetAll())
                {
                    otherClient.Send(jobResult.jobNtc);
                }
            }
            
            if (jobResult.itemNtc != null)
            {
                client.Send(jobResult.itemNtc);
            }

            return jobResult.jobRes;
        }
    }
}
