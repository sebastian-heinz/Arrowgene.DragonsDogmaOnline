using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Entity;

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
            (S2CJobChangeJobRes jobRes, S2CItemUpdateCharacterItemNtc itemNtc, S2CJobChangeJobNtc jobNtc) jobResult = (null, null, null);

            Server.Database.ExecuteInTransaction(connection =>
            {
                jobResult = ((S2CJobChangeJobRes, S2CItemUpdateCharacterItemNtc, S2CJobChangeJobNtc))
                jobManager.SetJob(client, client.Character, packet.Structure.JobId, connection);
            });

            foreach (GameClient otherClient in Server.ClientLookup.GetAll())
            {
                otherClient.Send(jobResult.jobNtc);
            }
            client.Send(jobResult.itemNtc);
            client.Send(jobResult.jobRes);
        }
    }
}
