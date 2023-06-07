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
    public class SkillGetPawnLearnedNormalSkillListHandler : GameStructurePacketHandler<C2SSkillGetPawnLearnedNormalSkillListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnLearnedNormalSkillListHandler));
        
        public SkillGetPawnLearnedNormalSkillListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetPawnLearnedNormalSkillListReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            client.Send(new S2CSkillGetPawnLearnedNormalSkillListRes() {
                PawnId = pawn.PawnId,
                NormalSkillParamList = pawn.LearnedNormalSkills
            });
        }
    }
}