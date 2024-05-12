using System.Linq;
using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnPawnSkillHandler : GameStructurePacketHandler<C2SSkillLearnPawnSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnPawnSkillHandler));
        
        private readonly JobManager jobManager;

        public SkillLearnPawnSkillHandler(DdonGameServer server) : base(server)
        {
            this.jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnPawnSkillReq> packet)
        {
            Pawn pawn = client.Character.Pawns.Where(pawn => pawn.PawnId == packet.Structure.PawnId).Single();
            this.jobManager.UnlockSkill(Server.Database, client, pawn, packet.Structure.Job, packet.Structure.SkillId, packet.Structure.SkillLv);
        }
    }
}