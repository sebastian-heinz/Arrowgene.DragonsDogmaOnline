using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnLearnedAbilityListHandler : GameRequestPacketHandler<C2SSkillGetPawnLearnedAbilityListReq, S2CSkillGetPawnLearnedAbilityListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnLearnedAbilityListHandler));
        
        public SkillGetPawnLearnedAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetPawnLearnedAbilityListRes Handle(GameClient client, C2SSkillGetPawnLearnedAbilityListReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            return new S2CSkillGetPawnLearnedAbilityListRes()
            {
                PawnId = pawn.PawnId,
                SetAcquierementParam = pawn.LearnedAbilities.Select(x => x.AsCDataLearnedSetAcquirementParam()).ToList()
            };
        }
    }
}
