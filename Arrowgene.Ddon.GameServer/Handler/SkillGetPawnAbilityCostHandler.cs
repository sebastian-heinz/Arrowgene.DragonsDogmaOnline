using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnAbilityCostHandler : GameRequestPacketHandler<C2SSkillGetPawnAbilityCostReq, S2CSkillGetPawnAbilityCostRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnAbilityCostHandler));

        public SkillGetPawnAbilityCostHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetPawnAbilityCostRes Handle(GameClient client, C2SSkillGetPawnAbilityCostReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);

            return new S2CSkillGetPawnAbilityCostRes()
            {
                PawnId = request.PawnId,
                CostMax = Server.CharacterManager.GetMaxAugmentAllocation(pawn)
            };
        }
    }
}
