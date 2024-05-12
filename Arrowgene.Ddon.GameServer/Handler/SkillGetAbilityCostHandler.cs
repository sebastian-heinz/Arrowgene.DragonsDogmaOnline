using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetAbilityCostHandler : StructurePacketHandler<GameClient, C2SSkillGetAbilityCostReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetAbilityCostHandler));

        private CharacterManager _CharacterManager;

        public SkillGetAbilityCostHandler(DdonGameServer server) : base(server)
        {
            _CharacterManager = server.CharacterManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetAbilityCostReq> packet)
        {
            client.Send(new S2CSkillGetAbilityCostRes()
            {
                CostMax = _CharacterManager.GetMaxAugmentAllocation(client.Character)
            });
        }
    }
}
