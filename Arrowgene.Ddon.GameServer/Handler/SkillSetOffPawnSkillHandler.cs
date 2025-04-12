using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetOffPawnSkillHandler : GameRequestPacketHandler<C2SSkillSetOffPawnSkillReq, S2CSkillSetOffPawnSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffPawnSkillHandler));
        
        private readonly JobManager jobManager;

        public SkillSetOffPawnSkillHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override S2CSkillSetOffPawnSkillRes Handle(GameClient client, C2SSkillSetOffPawnSkillReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            jobManager.RemoveSkill(Server.Database, pawn, request.Job, request.SlotNo);

            return new S2CSkillSetOffPawnSkillRes() {
                PawnId = request.PawnId,
                Job = request.Job,
                SlotNo = request.SlotNo
            };
        }
    }
}
