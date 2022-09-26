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

        public SkillSetOffSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetOffSkillReq> packet)
        {
            client.Character.CustomSkills.RemoveAll(skill => skill.Job == packet.Structure.Job && skill.SlotNo == packet.Structure.SlotNo);

            // TODO: Error handling
            Database.DeleteSetAcquirementParam(client.Character.Id, packet.Structure.Job, 0, packet.Structure.SlotNo);

            client.Send(new S2CSkillSetOffSkillRes() {
                Job = packet.Structure.Job,
                SlotNo = packet.Structure.SlotNo
            });

            // I haven't found a packet to notify this to other players
            // From what I tested it doesn't seem to be necessary
        }
    }
}