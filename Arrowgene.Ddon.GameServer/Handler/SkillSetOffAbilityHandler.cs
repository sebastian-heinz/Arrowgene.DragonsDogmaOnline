using System.Xml.Linq;
using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetOffAbilityHandler : StructurePacketHandler<GameClient, C2SSkillSetOffAbilityReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetOffAbilityHandler));

        public SkillSetOffAbilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetOffAbilityReq> packet)
        {
            CDataSetAcquirementParam ability = client.Character.Abilities
                .Where(skill => skill.Type == (byte) client.Character.Job && skill.SlotNo == packet.Structure.SlotNo)
                .Single();

            client.Character.Abilities.Remove(ability);

            // TODO: Error handling
            Database.DeleteSetAcquirementParam(client.Character.Id, ability.Job, (byte) client.Character.Job, packet.Structure.SlotNo);

            client.Send(new S2CSkillSetOffAbilityRes() {
                SlotNo = packet.Structure.SlotNo
            });

            // Same as skills, i haven't found an Ability off NTC. It may not be required
        }
    }
}