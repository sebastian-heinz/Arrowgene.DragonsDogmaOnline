using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnLearnedNormalSkillListHandler : GameRequestPacketHandler<C2SSkillGetPawnLearnedNormalSkillListReq, S2CSkillGetPawnLearnedNormalSkillListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnLearnedNormalSkillListHandler));
        
        public SkillGetPawnLearnedNormalSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetPawnLearnedNormalSkillListRes Handle(GameClient client, C2SSkillGetPawnLearnedNormalSkillListReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            return new S2CSkillGetPawnLearnedNormalSkillListRes() {
                PawnId = pawn.PawnId,
                NormalSkillParamList = pawn.LearnedNormalSkills
            };
        }
    }
}
