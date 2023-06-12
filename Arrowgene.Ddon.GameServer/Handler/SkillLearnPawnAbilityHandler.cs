using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnPawnAbilityHandler : GameStructurePacketHandler<C2SSkillLearnPawnAbilityReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnPawnAbilityHandler));
        
        private readonly JobManager jobManager;

        public SkillLearnPawnAbilityHandler(DdonGameServer server) : base(server)
        {
            this.jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnPawnAbilityReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            JobId augJob = SkillGetAcquirableAbilityListHandler.AllAbilities.Where(aug => aug.AbilityNo == packet.Structure.AbilityId).Select(aug => aug.Job).Single(); // why is this not in the packet
            this.jobManager.UnlockAbility(Server.Database, client, pawn, augJob, packet.Structure.AbilityId, packet.Structure.AbilityLv);
        }
    }
}