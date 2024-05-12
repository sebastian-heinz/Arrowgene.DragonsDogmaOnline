using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnLearnedSkillListHandler : GameStructurePacketHandler<C2SSkillGetPawnLearnedSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnLearnedSkillListHandler));
        
        public SkillGetPawnLearnedSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetPawnLearnedSkillListReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            client.Send(new S2CSkillGetPawnLearnedSkillListRes()
            {
                PawnId = pawn.PawnId,
                LearnedAcquierementParamList = pawn.LearnedCustomSkills.Select(x => x.AsCDataLearnedSetAcquirementParam()).ToList()
            });
        }
    }
}