using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPawnAbilityCostHandler : GameStructurePacketHandler<C2SSkillGetPawnAbilityCostReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPawnAbilityCostHandler));
        
        public SkillGetPawnAbilityCostHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetPawnAbilityCostReq> packet)
        {
            client.Send(new S2CSkillGetPawnAbilityCostRes()
            {
                PawnId = packet.Structure.PawnId,
                CostMax = 100
            });
        }
    }
}