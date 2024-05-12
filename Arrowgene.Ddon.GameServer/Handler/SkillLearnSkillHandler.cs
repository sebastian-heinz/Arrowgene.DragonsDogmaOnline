using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnSkillHandler : GameStructurePacketHandler<C2SSkillLearnSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnSkillHandler));
        
        private readonly JobManager jobManager;

        public SkillLearnSkillHandler(DdonGameServer server) : base(server)
        {
            this.jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnSkillReq> packet)
        {
            this.jobManager.UnlockSkill(Server.Database, client, client.Character, packet.Structure.Job, packet.Structure.SkillId, packet.Structure.SkillLv);
        }
    }
}