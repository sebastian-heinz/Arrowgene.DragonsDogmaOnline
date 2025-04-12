using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnSetSkillListHandler : GameRequestPacketHandler<C2SSkillGetPawnSetSkillListReq, S2CSkillGetPawnSetSkillListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnSetSkillListHandler));
        
        public SkillGetPawnSetSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetPawnSetSkillListRes Handle(GameClient client, C2SSkillGetPawnSetSkillListReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            // TODO: Check if its necessary to filter so only the current job skills are sent
            return new S2CSkillGetPawnSetSkillListRes()
            {
                PawnId = pawn.PawnId,
                SetAcquierementParamList = pawn.EquippedCustomSkillsDictionary[pawn.Job]
                    .Select((x, index) => x?.AsCDataSetAcquirementParam((byte)(index + 1)))
                    .Where(x => x != null)
                    .ToList()
            };
        }
    }
}
