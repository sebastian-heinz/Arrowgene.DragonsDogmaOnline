using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnAbilityHandler : GameStructurePacketHandler<C2SSkillLearnAbilityReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnAbilityHandler));
        
        private readonly JobManager jobManager;

        public SkillLearnAbilityHandler(DdonGameServer server) : base(server)
        {
            this.jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnAbilityReq> packet)
        {
            this.jobManager.UnlockAbility(Server.Database, client, client.Character, packet.Structure.Job, packet.Structure.AbilityId, packet.Structure.AbilityLv);
        }
    }
}