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

        public SkillGetAbilityCostHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetAbilityCostReq> packet)
        {
            client.Send(new S2CSkillGetAbilityCostRes()
            {
                CostMax = 100
            });
        }
    }
}