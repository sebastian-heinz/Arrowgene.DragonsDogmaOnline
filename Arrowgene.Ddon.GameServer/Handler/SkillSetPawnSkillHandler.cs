using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetPawnSkillHandler : GameRequestPacketHandler<C2SSkillSetPawnSkillReq, S2CSkillSetPawnSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetPawnSkillHandler));
        
        private readonly DdonGameServer gameServer;

        public SkillSetPawnSkillHandler(DdonGameServer server) : base(server)
        {
            gameServer = server;
        }

        public override S2CSkillSetPawnSkillRes Handle(GameClient client, C2SSkillSetPawnSkillReq request)
        {
            Pawn pawn = client.Character.PawnById(request.PawnId, PawnType.Main);
            CustomSkill skillSlot = gameServer.JobManager.SetSkill(Server.Database, client, pawn, request.Job, request.SlotNo, request.SkillId, request.SkillLv);
            return new S2CSkillSetPawnSkillRes() {
                PawnId = pawn.PawnId,
                Job = skillSlot.Job,
                SlotNo = request.SlotNo,
                SkillId = skillSlot.SkillId,
                SkillLv = skillSlot.SkillLv
            };
        }
    }
}
