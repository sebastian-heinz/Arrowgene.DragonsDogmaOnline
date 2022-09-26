using System.Linq;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
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
            // TODO: Check in DB if the skill is unlocked and it's leveled up to what the packet says
            
            CDataSetAcquirementParam abilitySlot = client.Character.Abilities
                .Where(skill => skill.Job == packet.Structure.Job && skill.SlotNo == packet.Structure.SlotNo)
                .FirstOrDefault();
            
            if(abilitySlot == null)
            {
                abilitySlot = new CDataSetAcquirementParam()
                {
                    Job = packet.Structure.Job,
                    SlotNo = packet.Structure.SlotNo,
                    Type = (byte) client.Character.Job
                };
                client.Character.Abilities.Add(abilitySlot);
            }

            abilitySlot.AcquirementNo = packet.Structure.SkillId;
            abilitySlot.AcquirementLv = packet.Structure.SkillLv;

            Database.ReplaceSetAcquirementParam(client.Character.Id, abilitySlot);

            client.Send(new S2CSkillSetAbilityRes() {
                SlotNo = abilitySlot.SlotNo,
                AbilityId = abilitySlot.AcquirementNo,
                AbilityLv = abilitySlot.AcquirementLv
            });

            // Inform party members of the change
            client.Party.SendToAll(new S2CSkillAbilitySetNtc()
            {
                CharacterId = client.Character.Id,
                ContextAcquirementData = new CDataContextAcquirementData()
                {
                    SlotNo = abilitySlot.SlotNo,
                    AcquirementNo = abilitySlot.AcquirementNo,
                    AcquirementLv = abilitySlot.AcquirementLv
                }
            });
        }
    }
}