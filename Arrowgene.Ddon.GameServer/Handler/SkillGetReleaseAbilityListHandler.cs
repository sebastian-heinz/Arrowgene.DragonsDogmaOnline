using Arrowgene.Ddon.GameServer.Scripting.Interfaces;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetReleaseAbilityListHandler : GameRequestPacketHandler<C2SSkillGetReleaseAbilityListReq, S2CSkillGetReleaseAbilityListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetReleaseAbilityListHandler));

        public SkillGetReleaseAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetReleaseAbilityListRes Handle(GameClient client, C2SSkillGetReleaseAbilityListReq request)
        {
            return new();
        }
    }
}
