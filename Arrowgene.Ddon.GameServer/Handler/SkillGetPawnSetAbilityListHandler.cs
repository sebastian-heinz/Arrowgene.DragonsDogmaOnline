using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnSetAbilityListHandler : GameRequestPacketHandler<C2SSkillGetPawnSetAbilityListReq, S2CSkillGetPawnSetAbilityListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnSetAbilityListHandler));
        
        public SkillGetPawnSetAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetPawnSetAbilityListRes Handle(GameClient client, C2SSkillGetPawnSetAbilityListReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            // TODO: Check if its necessary to filter so only the current job skills are sent
            return new S2CSkillGetPawnSetAbilityListRes()
            {
                PawnId = pawn.PawnId,
                SetAcquierementParamList = pawn.EquippedAbilitiesDictionary[pawn.Job]
                    .Select((x, index) => x?.AsCDataSetAcquirementParam((byte)(index + 1)))
                    .Where(x => x != null)
                    .ToList()
            };
        }
    }
}
