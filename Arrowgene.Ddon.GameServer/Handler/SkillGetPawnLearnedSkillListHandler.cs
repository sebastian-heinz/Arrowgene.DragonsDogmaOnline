using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnLearnedSkillListHandler : GameRequestPacketHandler<C2SSkillGetPawnLearnedSkillListReq, S2CSkillGetPawnLearnedSkillListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnLearnedSkillListHandler));
        
        public SkillGetPawnLearnedSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetPawnLearnedSkillListRes Handle(GameClient client, C2SSkillGetPawnLearnedSkillListReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            return new S2CSkillGetPawnLearnedSkillListRes()
            {
                PawnId = pawn.PawnId,
                LearnedAcquierementParamList = pawn.LearnedCustomSkills.Select(x => x.AsCDataLearnedSetAcquirementParam()).ToList()
            };
        }
    }
}
