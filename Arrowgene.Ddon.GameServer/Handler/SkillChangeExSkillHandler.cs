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

            foreach (CDataSetAcquirementParam skillSlot in skillSlots)
            {
                skillSlot.AcquirementNo = packet.Structure.SkillId;
                skillSlot.AcquirementLv = 1; // Must be 1 otherwise they do 0 damage

                Database.ReplaceSetAcquirementParam(client.Character.Id, skillSlot);

                // Inform party members of the change
                if(packet.Structure.Job == client.Character.Job)
                {
                    client.Party.SendToAll(new S2CSkillCustomSkillSetNtc()
                    {
                        CharacterId = client.Character.Id,
                        ContextAcquirementData = new CDataContextAcquirementData()
                        {
                            SlotNo = skillSlot.SlotNo,
                            AcquirementNo = skillSlot.AcquirementNo,
                            AcquirementLv = skillSlot.AcquirementLv
                        }
                    });
                }
            }

            client.Send(new S2CSkillChangeExSkillRes() {
                Job = packet.Structure.Job,
                SkillId = packet.Structure.SkillId,
                SkillLv = 1, // Must be 1 otherwise they do 0 damage
                Unk3 = packet.Structure.Unk0,
                SlotsToUpdate = skillSlots.Select(skill => new CDataCommonU8(skill.SlotNo)).ToList()
            });
        }

        private uint GetBaseSkillId(uint skillId)
        {
            return skillId % 100;
        }
    }
}