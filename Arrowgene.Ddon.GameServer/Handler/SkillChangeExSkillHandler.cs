using System.Collections.Generic;
using Arrowgene.Ddon.Server;
using Arrowgene.Ddon.Server.Network;
using Arrowgene.Ddon.Shared.Entity.PacketStructure;
using Arrowgene.Ddon.Shared.Entity.Structure;
using Arrowgene.Ddon.Shared.Network;
using Arrowgene.Logging;
using System.Linq;

namespace Arrowgene.Ddon.GameServer.Handler
{
    public class SkillChangeExSkillHandler : StructurePacketHandler<GameClient, C2SSkillChangeExSkillReq>
    {
        private static readonly ServerLogger Logger = LogProvider.Logger<ServerLogger>(typeof(SkillChangeExSkillHandler));

        public SkillChangeExSkillHandler(DdonGameServer server) : base(server)
        {
        }

        public override void Handle(GameClient client, StructurePacket<C2SSkillChangeExSkillReq> packet)
        {
            // Update all equiped skills of the type to the chosen EX skill
            IEnumerable<CDataSetAcquirementParam> skillSlots = client.Character.CustomSkills
                .Where(skill => skill.Job == packet.Structure.Job && GetBaseSkillId(skill.AcquirementNo) == GetBaseSkillId(packet.Structure.SkillId));
            // TODO: Update only the skill itself rather than the entire character to avoid performance issues
            Database.UpdateCharacter(client.Character);

            foreach (CDataSetAcquirementParam skillSlot in skillSlots)
            {
                skillSlot.AcquirementNo = packet.Structure.SkillId;
                skillSlot.AcquirementLv = 1; // Must be 1 otherwise they do 0 damage
            }

            client.Send(new S2CSkillChangeExSkillRes() {
                Job = packet.Structure.Job,
                SkillId = packet.Structure.SkillId,
                SkillLv = 1, // Must be 1 otherwise they do 0 damage
                Unk3 = packet.Structure.Unk0,
                SlotsToUpdate = skillSlots.Select(skill => new CDataCommonU8(skill.SlotNo)).ToList()
            });

            // Inform party members of the change
            // There's probably a different, smaller packet precisely for this purpose (S2C_CUSTOM_SKILL_SET_NTC?)
            S2CContextGetPartyPlayerContextNtc partyPlayerContextNtc = new S2CContextGetPartyPlayerContextNtc(client.Character);
            partyPlayerContextNtc.Context.Base.MemberIndex = client.Party.Members.IndexOf(client);
            client.Party.SendToAll(partyPlayerContextNtc);
        }

        private uint GetBaseSkillId(uint skillId)
        {
            return skillId % 100;
        }
    }
}