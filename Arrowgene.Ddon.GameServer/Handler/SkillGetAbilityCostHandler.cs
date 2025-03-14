using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetAbilityCostHandler : GameRequestPacketHandler<C2SSkillGetAbilityCostReq, S2CSkillGetAbilityCostRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetAbilityCostHandler));


        public SkillGetAbilityCostHandler(DdonGameServer server) : base(server)
        {
        }

        public override S2CSkillGetAbilityCostRes Handle(GameClient client, C2SSkillGetAbilityCostReq request)
        {
            return new S2CSkillGetAbilityCostRes()
            {
                CostMax = Server.CharacterManager.GetMaxAugmentAllocation(client.Character)
            };
        }
    }
}
