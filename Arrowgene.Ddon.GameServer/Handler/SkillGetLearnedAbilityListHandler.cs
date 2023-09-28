using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetLearnedAbilityListHandler : StructurePacketHandler<GameClient, C2SSkillGetLearnedAbilityListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetLearnedAbilityListHandler));

        public SkillGetLearnedAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetLearnedAbilityListReq> packet)
        {
            client.Send(new S2CSkillGetLearnedAbilityListRes()
            {
                SetAcquierementParam = client.Character.LearnedAbilities
                    .Select(x => x?.AsCDataLearnedSetAcquirementParam())
                    .Where(x => x != null)
                    .ToList()
            });            
        }
    }
}