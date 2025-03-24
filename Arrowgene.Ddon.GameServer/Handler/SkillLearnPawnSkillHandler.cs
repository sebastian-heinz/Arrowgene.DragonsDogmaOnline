using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnPawnSkillHandler : GameRequestPacketQueueHandler<C2SSkillLearnPawnSkillReq, S2CSkillLearnPawnSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnPawnSkillHandler));
        
        public SkillLearnPawnSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SSkillLearnPawnSkillReq request)
        {
            var (pawn, _) = client.Character.PawnById(request.PawnId, PawnType.Main);
            return Server.JobManager.UnlockSkill(Server.Database, client, pawn, request.Job, request.SkillId, request.SkillLv);
        }
    }
}
