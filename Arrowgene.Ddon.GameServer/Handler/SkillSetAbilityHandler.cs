using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetAbilityHandler : StructurePacketHandler<GameClient, C2SSkillSetAbilityReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetAbilityHandler));

        private readonly JobManager jobManager;

        public SkillSetAbilityHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetAbilityReq> packet)
        {
            if(packet.Structure.SlotNo == 0)
            {
                Logger.Error(client, $"Requesting to set an ability to slot 0\n{client.Character.Abilities}");
            }
            
            Ability abilitySlot = jobManager.SetAbility(Server.Database, client, client.Character, packet.Structure.Job, packet.Structure.SlotNo, packet.Structure.SkillId, packet.Structure.SkillLv);

            client.Send(new S2CSkillSetAbilityRes() {
                SlotNo = abilitySlot.SlotNo,
                AbilityId = abilitySlot.AbilityId,
                AbilityLv = abilitySlot.AbilityLv
            });
        }
    }
}