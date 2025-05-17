using Arrowgene.Ddon.GameServer.Party;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class JobOrbTreeReleaseJobOrbElementHandler : GameRequestPacketHandler<C2SJobOrbTreeReleaseJobOrbElementReq, S2CJobOrbTreeReleaseJobOrbElementRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(JobOrbTreeReleaseJobOrbElementHandler));

        public JobOrbTreeReleaseJobOrbElementHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CJobOrbTreeReleaseJobOrbElementRes Handle(GameClient client, C2SJobOrbTreeReleaseJobOrbElementReq request)
        {
            var packets = new PacketQueue();

            JobId jobId = JobOrbUpgrade.GetJobIdFromReleaseId(request.ElementId);
            Server.Database.ExecuteInTransaction(connection =>
            {
                packets.AddRange(Server.JobOrbUnlockManager.ReleaseElement(client, jobId, request.ElementId, connection));
            });

            // Notify all players of the upgrade
            packets.AddRange(Server.CharacterManager.UpdateCharacterExtendedParamsNtc(client, client.Character));

            foreach (var member in client.Party.Members)
            {
                if (member is PawnPartyMember pawnMember && pawnMember.Pawn.CharacterId == client.Character.CharacterId)
                {
                    packets.AddRange(Server.CharacterManager.UpdateCharacterExtendedParamsNtc(client, pawnMember.Pawn));
                }
            }

            packets.Send();

            return new()
            {
                JobId = jobId,
                RestOrb = Server.WalletManager.GetWalletAmount(client.Character, WalletType.BloodOrbs),
                TreeStatus = new()
                {
                    IsReleased = true,
                    JobId = jobId,
                    Rate = Server.JobOrbUnlockManager.CalculatePercentCompleted(client.Character, jobId)
                }
            };
        }
    }
}

