using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetOffSkillHandler : GameRequestPacketHandler<C2SSkillSetOffSkillReq, S2CSkillSetOffSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffSkillHandler));

        private readonly JobManager jobManager;

        public SkillSetOffSkillHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override S2CSkillSetOffSkillRes Handle(GameClient client, C2SSkillSetOffSkillReq request)
        {
            jobManager.RemoveSkill(Server.Database, client.Character, request.Job, request.SlotNo);

            return new S2CSkillSetOffSkillRes() {
                Job = request.Job,
                SlotNo = request.SlotNo
            };
        }
    }
}
