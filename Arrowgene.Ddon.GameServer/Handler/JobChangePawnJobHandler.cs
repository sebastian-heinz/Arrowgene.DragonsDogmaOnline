using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobChangePawnJobHandler : GameStructurePacketHandler<C2SJobChangePawnJobReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobChangePawnJobHandler));
        
        private readonly JobManager jobManager;

        public JobChangePawnJobHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SJobChangePawnJobReq> packet)
        {
            (S2CJobChangePawnJobRes jobRes, S2CItemUpdateCharacterItemNtc itemNtc, S2CJobChangePawnJobNtc jobNtc) jobResult = (null, null, null);

            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            Server.Database.ExecuteInTransaction(connection =>
            {
                jobResult = ((S2CJobChangePawnJobRes, S2CItemUpdateCharacterItemNtc, S2CJobChangePawnJobNtc))
                jobManager.SetJob(client, pawn, packet.Structure.JobId, connection);
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

            client.Send(jobResult.jobRes);
        }
    }
}
