using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnSkillHandler : GameRequestPacketQueueHandler<C2SSkillLearnSkillReq, S2CSkillLearnSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnSkillHandler));
        
        public SkillLearnSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SSkillLearnSkillReq request)
        {
            return Server.JobManager.UnlockSkill(Server.Database, client, client.Character, request.Job, request.SkillId, request.SkillLv);
        }
    }
}
