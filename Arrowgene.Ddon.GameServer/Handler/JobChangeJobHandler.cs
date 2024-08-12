using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobChangeJobHandler : StructurePacketHandler<GameClient, C2SJobChangeJobReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobChangeJobHandler));

        private readonly JobManager jobManager;

        public JobChangeJobHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobChangeJobReq> packet)
        {
            jobManager.SetJob(client, client.Character, packet.Structure.JobId, DeferredOperations);
        }
    }
}
