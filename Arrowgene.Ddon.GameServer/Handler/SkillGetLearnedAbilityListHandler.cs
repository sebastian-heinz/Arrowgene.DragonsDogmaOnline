using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetLearnedAbilityListHandler : GameRequestPacketHandler<C2SSkillGetLearnedAbilityListReq, S2CSkillGetLearnedAbilityListRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetLearnedAbilityListHandler));

        public SkillGetLearnedAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetLearnedAbilityListRes Handle(GameClient client, C2SSkillGetLearnedAbilityListReq request)
        {
            return new S2CSkillGetLearnedAbilityListRes()
            {
                SetAcquierementParam = client.Character.LearnedAbilities
                    .Select(x => x?.AsCDataLearnedSetAcquirementParam())
                    .Where(x => x != null)
                    .ToList()
            };            
        }
    }
}
