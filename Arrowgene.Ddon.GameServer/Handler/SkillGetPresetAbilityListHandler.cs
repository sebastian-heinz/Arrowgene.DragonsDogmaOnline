using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillGetPresetAbilityListHandler : StructurePacketHandler<GameClient, C2SSkillGetPresetAbilityListReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillGetPresetAbilityListHandler));

        public SkillGetPresetAbilityListHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillGetPresetAbilityListReq> packet)
        {
            client.Send(new S2CSkillGetPresetAbilityListRes());
        }
    }
}