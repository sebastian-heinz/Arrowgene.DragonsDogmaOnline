using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using Arrowgene.Ddon.GameServer.Characters;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetOffAbilityHandler : StructurePacketHandler<GameClient, C2SSkillSetOffAbilityReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffAbilityHandler));

        private readonly JobManager jobManager;

        public SkillSetOffAbilityHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetOffAbilityReq> packet)
        {
            jobManager.RemoveAbility(Server.Database, client.Character, packet.Structure.SlotNo);

            client.Send(new S2CSkillSetOffAbilityRes() {
                SlotNo = packet.Structure.SlotNo
            });
        }
    }
}