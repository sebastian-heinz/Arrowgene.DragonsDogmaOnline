using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnSetSkillListHandler : GameStructurePacketHandler<C2SSkillGetPawnSetSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnSetSkillListHandler));
        
        public SkillGetPawnSetSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetPawnSetSkillListReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            // TODO: Check if its necessary to filter so only the current job skills are sent
            S2CSkillGetPawnSetSkillListRes res = new S2CSkillGetPawnSetSkillListRes();
            res.PawnId = pawn.PawnId;
            res.SetAcquierementParamList = pawn.CustomSkills
                .Where(x => x.Job == pawn.Job)
                .Select(x => x.AsCDataSetAcquirementParam())
                .ToList();
            client.Send(res);
        }
    }
}