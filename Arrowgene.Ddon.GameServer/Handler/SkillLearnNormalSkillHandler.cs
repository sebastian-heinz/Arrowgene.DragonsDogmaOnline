using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillLearnNormalSkillHandler : GameStructurePacketHandler<C2SSkillLearnNormalSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillLearnNormalSkillHandler));

        private readonly JobManager _jobManager;

        public SkillLearnNormalSkillHandler(DdonGameServer server) : base(server)
        {
            this._jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillLearnNormalSkillReq> packet)
        {
            _jobManager.UnlockLearnedNormalSkill(Server.Database, client, client.Character,
                                                 packet.Structure.Job, packet.Structure.SkillId);
        }
    }
}
