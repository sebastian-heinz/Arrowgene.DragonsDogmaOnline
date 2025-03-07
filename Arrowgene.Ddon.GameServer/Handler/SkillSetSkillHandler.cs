using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetSkillHandler : GameRequestPacketHandler<C2SSkillSetSkillReq, S2CSkillSetSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetSkillHandler));

        public SkillSetSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillSetSkillRes Handle(GameClient client, C2SSkillSetSkillReq request)
        {
            CustomSkill skillSlot = Server.JobManager.SetSkill(Server.Database, client, client.Character, request.Job, request.SlotNo, request.SkillId, request.SkillLv);
            return new S2CSkillSetSkillRes() {
                Job = skillSlot.Job,
                SlotNo = request.SlotNo,
                SkillId = skillSlot.SkillId,
                SkillLv = skillSlot.SkillLv
            };
        }
    }
}
