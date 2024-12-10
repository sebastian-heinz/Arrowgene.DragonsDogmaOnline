using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobChangeJobHandler : GameRequestPacketQueueHandler<C2SJobChangeJobReq, S2CJobChangeJobRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobChangeJobHandler));


        public JobChangeJobHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SJobChangeJobReq request)
        {
            PacketQueue queue = new();

            Server.Database.ExecuteInTransaction(connection =>
            {
                queue.AddRange(Server.JobManager.SetJob(client, client.Character, request.JobId, connection));
            });

            return queue;
        }
    }
}
