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
            // TODO: Do this in DB instead of in memory
            IEnumerable<CDataSetAcquirementParam> skillSlots = client.Character.CustomSkills
                .Where(skill => skill.Job == packet.Structure.Job && GetBaseSkillId(skill.AcquirementNo) == GetBaseSkillId(packet.Structure.SkillId));
            
            foreach (CDataSetAcquirementParam skillSlot in skillSlots)
            {
                skillSlot.AcquirementNo = packet.Structure.SkillId;
            }

            client.Send(new S2CSkillChangeExSkillRes() {
                Job = packet.Structure.Job,
                SkillId = packet.Structure.SkillId,
                SkillLv = 10, 
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