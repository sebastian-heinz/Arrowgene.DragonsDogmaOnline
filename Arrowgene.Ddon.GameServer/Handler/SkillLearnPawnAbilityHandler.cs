using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnPawnAbilityHandler : GameRequestPacketQueueHandler<C2SSkillLearnPawnAbilityReq, S2CSkillLearnPawnAbilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnPawnAbilityHandler));
        
        public SkillLearnPawnAbilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SSkillLearnPawnAbilityReq request)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == request.PawnId).Single();

            var allAbilities = SkillData.AllAbilities.Concat(SkillData.AllSecretAbilities);

            JobId augJob = SkillData.AllAbilities.Where(aug => aug.AbilityNo == request.AbilityId).Select(aug => aug.Job).Single(); // why is this not in the packet
            return Server.JobManager.UnlockAbility(Server.Database, client, pawn, augJob, request.AbilityId, request.AbilityLv);
        }
    }
}
