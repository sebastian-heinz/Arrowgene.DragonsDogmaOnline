using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnNormalSkillHandler : GameRequestPacketQueueHandler<C2SSkillLearnNormalSkillReq, S2CSkillLearnNormalSkillRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnNormalSkillHandler));

        public SkillLearnNormalSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SSkillLearnNormalSkillReq request)
        {
            return Server.JobManager.UnlockLearnedNormalSkill(Server.AssetRepository, Server.Database, client, client.Character,
                                                 request.Job, request.SkillId);
        }
    }
}
