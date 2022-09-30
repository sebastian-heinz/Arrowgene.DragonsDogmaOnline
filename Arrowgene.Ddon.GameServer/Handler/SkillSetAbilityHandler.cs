using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Model;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillSetAbilityHandler : StructurePacketHandler<GameClient, C2SSkillSetAbilityReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillSetAbilityHandler));

        public SkillSetAbilityHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillSetAbilityReq> packet)
        {
            if(packet.Structure.SlotNo == 0)
            {
                Logger.Error(client, $"Requesting to set a skill to slot 0\n{client.Character.Abilities}");
            }
            // TODO: Check in DB if the skill is unlocked and it's leveled up to what the packet says
            
            Ability abilitySlot = client.Character.Abilities
                .Where(ability => ability.EquippedToJob == client.Character.Job && ability.SlotNo == packet.Structure.SlotNo)
                .FirstOrDefault();
            
            if(abilitySlot == null)
            {
                abilitySlot = new Ability()
                {
                    EquippedToJob = client.Character.Job,
                    Job = packet.Structure.Job,
                    SlotNo = packet.Structure.SlotNo,
                };
                client.Character.Abilities.Add(abilitySlot);
            }
            
            abilitySlot.Job = packet.Structure.Job;
            abilitySlot.AbilityId = packet.Structure.SkillId;
            abilitySlot.AbilityLv = packet.Structure.SkillLv;

            Database.ReplaceEquippedAbility(client.Character.Id, abilitySlot);

            client.Send(new S2CSkillSetAbilityRes() {
                SlotNo = abilitySlot.SlotNo,
                AbilityId = abilitySlot.AbilityId,
                AbilityLv = abilitySlot.AbilityLv
            });

            // Inform party members of the change
            client.Party.SendToAll(new S2CSkillAbilitySetNtc()
            {
                CharacterId = client.Character.Id,
                ContextAcquirementData = new CDataContextAcquirementData()
                {
                    SlotNo = abilitySlot.SlotNo,
                    AcquirementNo = abilitySlot.AbilityId,
                    AcquirementLv = abilitySlot.AbilityLv
                }
            });
        }
    }
}