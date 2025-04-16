using Arrowgene.Ddon.GameServer.Characters;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetAbilityHandler : GameRequestPacketHandler<C2SSkillSetAbilityReq, S2CSkillSetAbilityRes>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetAbilityHandler));

        private readonly JobManager jobManager;

        public SkillSetAbilityHandler(DdonGameServer server) : base(server)
        {
            jobManager = server.JobManager;
        }

        public override S2CSkillSetAbilityRes Handle(GameClient client, C2SSkillSetAbilityReq packet)
        {
            if(packet.SlotNo == 0)
            {
                Logger.Error(client, $"Requesting to set an ability to slot 0");
            }

            Ability abilitySlot = jobManager.SetAbility(Server.Database, client, client.Character, packet.Job, packet.SlotNo, packet.SkillId, packet.SkillLv);

            return new S2CSkillSetAbilityRes() {
                SlotNo = packet.SlotNo,
                AbilityId = abilitySlot.AbilityId,
                AbilityLv = abilitySlot.AbilityLv
            };
        }
    }
}
