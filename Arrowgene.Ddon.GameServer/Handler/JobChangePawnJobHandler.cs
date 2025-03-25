using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.GameServer.Instance;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobChangePawnJobHandler : GameRequestPacketQueueHandler<C2SJobChangePawnJobReq, S2CJobChangePawnJobRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobChangePawnJobHandler));
        

        public JobChangePawnJobHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SJobChangePawnJobReq request)
        {
            PacketQueue queue = new();
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            Server.Database.ExecuteInTransaction(connection =>
            {
                queue.AddRange(Server.JobManager.SetJob(client, pawn, request.JobId, connection));
            });

            return queue;
        }
    }
}
