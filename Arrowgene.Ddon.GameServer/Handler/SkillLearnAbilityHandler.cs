using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnAbilityHandler : GameRequestPacketQueueHandler<C2SSkillLearnAbilityReq, S2CSkillLearnAbilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnAbilityHandler));
        
        public SkillLearnAbilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override PacketQueue Handle(GameClient client, C2SSkillLearnAbilityReq request)
        {
            return Server.JobManager.UnlockAbility(Server.Database, client, client.Character, request.Job, request.AbilityId, request.AbilityLv);
        }
    }
}
