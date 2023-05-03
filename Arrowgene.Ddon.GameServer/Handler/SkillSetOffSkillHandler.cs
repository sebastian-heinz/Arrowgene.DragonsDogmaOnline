using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetOffSkillHandler : StructurePacketHandler<GameClient, C2SSkillSetOffSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffSkillHandler));

        private readonly JobManager jobManager;

        public SkillSetOffSkillHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetOffSkillReq> packet)
        {
            jobManager.RemoveSkill(Server.Database, client.Character, packet.Structure.Job, packet.Structure.SlotNo);

            client.Send(new S2CSkillSetOffSkillRes() {
                Job = packet.Structure.Job,
                SlotNo = packet.Structure.SlotNo
            });
        }
    }
}